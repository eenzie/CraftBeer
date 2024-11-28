using Dapr.Workflow;
using OrderService.Application.Activities;
using OrderService.Application.CompensatingActivities;
using OrderService.Application.ExternalWorkflows;
using OrderService.Domain.Entities;
using Shared.DTOs;
using Shared.IntegrationEventsIncoming;

namespace OrderService.Application.Workflows;

public class OrderWorkflow : Workflow<OrderDto, OrderResult>
{
    public override async Task<OrderResult> RunAsync(WorkflowContext context, OrderDto orderDto)
    {
        #region Create order

        var newOrder = orderDto with { StatusDto = OrderStatusDto.OrderReceived, OrderId = context.InstanceId };

        await context.CallActivityAsync(
                        nameof(OrderCreationActivity),
                        newOrder);

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification($"Received order {newOrder.OrderId} from {newOrder.CustomerDto.Name}.", newOrder));

        //Add logic for Success/Failed integration events if order creation fails.
        //Requires a pub/sub on order service and related activities / events
        //Could perhaps run through the WorkflowChannel, but likely best to create an OrderChannel?

        #endregion

        #region Reserve Item(s)

        newOrder = newOrder with { StatusDto = OrderStatusDto.CheckingStock };

        var itemsToReserve = orderDto.OrderItemsDto
                                        .Select(x => new OrderItemDto(
                                            x.Id,
                                            x.StockTypeDto,
                                            x.Quantity,
                                            x.Total))
                                        .ToList();
        await context.CallActivityAsync(
            nameof(ReservationActivity),
            itemsToReserve);

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification
                ($"Waiting for reservation: Order {orderDto.OrderId} from {orderDto.CustomerDto.Name}.",
                orderDto));

        var reservationResult = await context.WaitForExternalEventAsync<ReservationResultEvent>(
            ExternalEvents.ReservationEvent,
            TimeSpan.FromDays(3));

        if (reservationResult.Status == ResultStatus.Failed)
        {
            newOrder = newOrder with { StatusDto = OrderStatusDto.InsufficientStock };

            await context.CallActivityAsync(
                nameof(NotificationActivity),
                new Notification(
                    $"Failed: Order {newOrder.OrderId} from {newOrder.CustomerDto.Name}. Reservation failed.",
                    newOrder));

            //Compensating transaction - Cancel order
            await context.CallActivityAsync(
                nameof(CancelOrderActivity),
                orderDto);

            return new OrderResult(orderDto.StatusDto, orderDto, "Reservation failed.");
        }

        newOrder = newOrder with { StatusDto = OrderStatusDto.SufficientStock };

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Reservation Completed: Order {newOrder.OrderId} from {newOrder.CustomerDto.Name}.",
                newOrder));

        #endregion

        #region Process Payment

        newOrder = newOrder with { StatusDto = OrderStatusDto.CheckingPayment };

        var paymentDto = new PaymentDto(orderDto.Total);

        await context.CallActivityAsync(
            nameof(PaymentActivity),
            paymentDto);

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Wating for payment of {paymentDto.Amount.ToString("C")} " +
                $"on order {newOrder.OrderId} f" +
                $"rom {newOrder.CustomerDto.Name}",
                newOrder));

        var paymentResult = await context.WaitForExternalEventAsync<PaymentResultEvent>(
            ExternalEvents.PaymentEvent,
            TimeSpan.FromDays(3));

        if (paymentResult.Status == ResultStatus.Failed)
        {
            newOrder = newOrder with { StatusDto = OrderStatusDto.PaymentFailed };

            await context.CallActivityAsync(
                nameof(NotificationActivity),
                new Notification(
                    $"Payment of {newOrder.Total} failed for order {newOrder.OrderId} from {newOrder.CustomerDto.Name}",
                    newOrder));

            //Compensating transaction - Unreserve the items
            await context.CallActivityAsync(
                nameof(UnreserveItemsActivity),
                itemsToReserve);

            //Compensating transaction - Cancel order
            await context.CallActivityAsync(
                nameof(CancelOrderActivity),
                orderDto);

            return new OrderResult(orderDto.StatusDto, orderDto, "Payment failed.");
        }

        newOrder = newOrder with { StatusDto = OrderStatusDto.PaymentSuccess };

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Payment Completed: Order {newOrder.OrderId} from {newOrder.CustomerDto.Name}.",
                newOrder));

        #endregion

        #region Shipping

        newOrder = newOrder with { StatusDto = OrderStatusDto.Shipping };

        await context.CallActivityAsync(
                       nameof(ShippingActivity),
                       newOrder);

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification($"Shipping order {newOrder.OrderId} to {newOrder.CustomerDto.Name}.", newOrder));

        var shippingResult = await context.WaitForExternalEventAsync<ShippingResultEvent>(
            ExternalEvents.ShippingEvent,
            TimeSpan.FromDays(3));

        if (shippingResult.Status == ResultStatus.Failed)
        {
            newOrder = newOrder with { StatusDto = OrderStatusDto.ShippingFailed };

            await context.CallActivityAsync(
                nameof(NotificationActivity),
                new Notification(
                    $"Failed: Order {newOrder.OrderId} to {newOrder.CustomerDto.Name}. Shipping failed.",
                    newOrder));

            //Compensating transaction - Refund payment
            await context.CallActivityAsync(
                nameof(RefundPaymentActivity),
                paymentDto);

            //Compensating transaction - Unreserve the items
            await context.CallActivityAsync(
                nameof(UnreserveItemsActivity),
                itemsToReserve);

            //Compensating transaction - Cancel order
            await context.CallActivityAsync(
                nameof(CancelOrderActivity),
                orderDto);

            return new OrderResult(orderDto.StatusDto, orderDto, "Shipping failed.");
        }

        #endregion

        await context.CallActivityAsync(
        nameof(NotificationActivity),
        new Notification(
            $"Completed: Order {newOrder.OrderId} from {newOrder.CustomerDto.Name}. Paid ({newOrder.Total.ToString("C")}) and shipped",
            newOrder));

        return new OrderResult(orderDto.StatusDto, orderDto);
    }
}

using Dapr.Workflow;
using OrderService.Application.Activities;
using OrderService.Application.ExternalWorkflows;
using OrderService.Domain.Entities;
using Shared.DTOs;
using Shared.IntegrationEventsIncoming;

namespace OrderService.Application.Workflows;

public class OrderWorkflow : Workflow<Order, OrderResult>
{
    public override async Task<OrderResult> RunAsync(WorkflowContext context, Order order)
    {
        #region Create order

        var newOrder = order with { Status = OrderStatus.OrderReceived, OrderId = context.InstanceId };

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification
                ($"Received order {order.ShortId} from {order.CustomerDto.Name}.",
                newOrder));

        #endregion

        #region Reserve Item(s)

        newOrder = newOrder with { Status = OrderStatus.CheckingStock };

        var itemsToReserve = new StockRequestDto(
            newOrder.OrderItems
            .Select(orderItem => new OrderItemDto(
                (StockTypeDto)orderItem.ItemType,
                orderItem.Quantity))
            .ToArray()
        );

        await context.CallActivityAsync(
            nameof(ReservationActivity),
            itemsToReserve);

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification
                ($"Waiting for reservation: Order {order.ShortId} from {order.CustomerDto.Name}.",
                newOrder));

        var reservationResult = await context.WaitForExternalEventAsync<ReservationResultEvent>(
            ExternalEvents.ReservationEvent,
            TimeSpan.FromDays(3));

        if (reservationResult.Status == ResultStatus.Failed)
        {
            newOrder = newOrder with { Status = OrderStatus.InsufficientStock };

            await context.CallActivityAsync(
                nameof(NotificationActivity),
                new Notification(
                    $"Failed: Order {order.ShortId} from {order.CustomerDto.Name}. Reservation failed.",
                    newOrder));

            return new OrderResult(
                newOrder.Status,
                newOrder,
                "Reservation failed.");
        }

        newOrder = newOrder with { Status = OrderStatus.SufficientStock };
        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Reservation Completed: Order {order.ShortId} from {order.CustomerDto.Name}.",
                newOrder));

        #endregion

        #region Process Payment

        newOrder = newOrder with { Status = OrderStatus.CheckingPayment };

        var paymentDto = new PaymentDto(newOrder.TotalAmount);

        await context.CallActivityAsync(
            nameof(PaymentActivity),
            paymentDto);

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Wating for Payment: Order {order.ShortId} from {order.CustomerDto.Name}.",
                newOrder));

        var paymentResult = await context.WaitForExternalEventAsync<PaymentResultEvent>(
            ExternalEvents.PaymentEvent,
            TimeSpan.FromDays(3));

        if (paymentResult.Status == ResultStatus.Failed)
        {
            newOrder = newOrder with { Status = OrderStatus.PaymentFailed };
            await context.CallActivityAsync(
                nameof(NotificationActivity),
                new Notification(
                    $"Failed: Order {order.ShortId} from {order.CustomerDto.Name}. Payment failed.",
                    newOrder));

            // TODO: Compensating transaction - Unreserve the items
            //await context.CallActivityAsync(
            //    nameof(UnReserveItemsActivity),
            //    itemsToReserve // Pass the items to be unreserved
            //);

            // Compensating transaction via message queue?
            //await _daprClient.PublishEventAsync(WorkflowChannel.Channel, WorkflowChannel.Topics.PaymentFailed,
            //itemsReservedResponse);


            return new OrderResult(newOrder.Status, newOrder, "Payment failed.");
        }

        newOrder = newOrder with { Status = OrderStatus.PaymentSuccess };
        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Payment Completed: Order {order.ShortId} from {order.CustomerDto.Name}.",
                newOrder));

        #endregion

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Completed: Order {order.ShortId} from {order.CustomerDto.Name}.",
                newOrder));

        return new OrderResult(newOrder.Status, newOrder);
    }
}

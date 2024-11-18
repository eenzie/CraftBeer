using Dapr.Workflow;
using OrderService.Application.Activities;
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

        orderDto.OrderId = context.InstanceId;
        orderDto.StatusDto = OrderStatusDto.OrderReceived;

        await context.CallActivityAsync(
            nameof(OrderCreationActivity),
            orderDto);


        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification
                ($"Received order {orderDto.OrderId} from {orderDto.CustomerDto.Name}.",
                orderDto));

        #endregion

        #region Reserve Item(s)

        orderDto.StatusDto = OrderStatusDto.OrderReceived;

        var itemsToReserve = new StockRequestDto(
            orderDto.OrderItemsDto
            .Select(x => new OrderItemDto(
                x.Id,
                x.StockType,
                x.Quantity,
                x.Total))
            .ToList()
        );

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
            orderDto.StatusDto = OrderStatusDto.InsufficientStock;

            await context.CallActivityAsync(
                nameof(NotificationActivity),
                new Notification(
                    $"Failed: Order {orderDto.OrderId} from {orderDto.CustomerDto.Name}. Reservation failed.",
                    orderDto));

            return new OrderResult(
                orderDto.StatusDto,
                orderDto,
                "Reservation failed.");
        }

        orderDto.StatusDto = OrderStatusDto.SufficientStock;

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Reservation Completed: Order {orderDto.OrderId} from {orderDto.CustomerDto.Name}.",
                orderDto));

        #endregion

        #region Process Payment

        orderDto.StatusDto = OrderStatusDto.CheckingPayment;

        var paymentDto = new PaymentDto(orderDto.Total);

        await context.CallActivityAsync(
            nameof(PaymentActivity),
            paymentDto);

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Wating for Payment: Order {orderDto.OrderId} from {orderDto.CustomerDto.Name}.",
                orderDto));

        var paymentResult = await context.WaitForExternalEventAsync<PaymentResultEvent>(
            ExternalEvents.PaymentEvent,
            TimeSpan.FromDays(3));

        if (paymentResult.Status == ResultStatus.Failed)
        {
            orderDto.StatusDto = OrderStatusDto.PaymentFailed;
            await context.CallActivityAsync(
                nameof(NotificationActivity),
                new Notification(
                    $"Failed: Order {orderDto.OrderId} from {orderDto.CustomerDto.Name}. Payment failed.",
                    orderDto));

            // TODO: Compensating transaction - Unreserve the items
            //await context.CallActivityAsync(
            //    nameof(UnReserveItemsActivity),
            //    itemsToReserve // Pass the items to be unreserved
            //);

            // Compensating transaction via message queue?
            //await _daprClient.PublishEventAsync(WorkflowChannel.Channel, WorkflowChannel.Topics.PaymentFailed,
            //itemsReservedResponse);


            return new OrderResult(orderDto.StatusDto, orderDto, "Payment failed.");
        }

        orderDto.StatusDto = OrderStatusDto.PaymentSuccess;
        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Payment Completed: Order {orderDto.OrderId} from {orderDto.CustomerDto.Name}.",
                orderDto));

        #endregion

        await context.CallActivityAsync(
            nameof(NotificationActivity),
            new Notification(
                $"Completed: Order {orderDto.OrderId}  from  {orderDto.CustomerDto.Name}.",
                orderDto));

        return new OrderResult(orderDto.StatusDto, orderDto);
    }
}

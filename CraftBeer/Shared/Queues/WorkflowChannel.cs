namespace Shared.Queues;

public class WorkflowChannel
{
    public const string Channel = "workflowchannel";
    public class Topics
    {
        public const string PaymentResult = "paymentresult";
        public const string ReservationResult = "reservationresult";
        public const string ShippingResult = "shippingresult";
    }
}

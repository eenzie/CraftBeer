namespace Shared.Queues;

public class PaymentChannel
{
    public const string Channel = "paymentchannel";
    public class Topics
    {
        public const string Payment = "payment";
        public const string Refund = "refund";
    }
}

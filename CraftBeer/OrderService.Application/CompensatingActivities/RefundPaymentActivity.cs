using Dapr.Client;
using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using OrderService.Application.Activities;
using Shared.DTOs;
using Shared.IntegrationEventsIncoming;
using Shared.Queues;

namespace OrderService.Application.CompensatingActivities;

public class RefundPaymentActivity : WorkflowActivity<PaymentDto, object?>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<NotificationActivity> _logger;

    public RefundPaymentActivity(DaprClient daprClient, ILogger<NotificationActivity> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public override async Task<object?> RunAsync(WorkflowActivityContext context, PaymentDto input)
    {
        _logger.LogInformation($"About to publish refund of: {input.Amount.ToString("C")}");

        var refundRequestMessage = new PaymentResultEvent { CorrelationId = context.InstanceId, Amount = input.Amount };

        await _daprClient.PublishEventAsync(PaymentChannel.Channel,
                                            PaymentChannel.Topics.Refund,
                                            refundRequestMessage);

        return null;
    }
}

using Dapr.Client;
using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.IntegrationEventsIncoming;
using Shared.Queues;

namespace OrderService.Application.Activities;

public class PaymentActivity : WorkflowActivity<PaymentDto, object?>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<NotificationActivity> _logger;

    public PaymentActivity(DaprClient daprClient, ILogger<NotificationActivity> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public override async Task<object?> RunAsync(WorkflowActivityContext context, PaymentDto input)
    {
        _logger.LogInformation($"About to publish payment of: {input.Amount.ToString("C")}");

        var paymentRequestMessage = new PaymentResultEvent { CorrelationId = context.InstanceId, Amount = 100 };

        await _daprClient.PublishEventAsync(PaymentChannel.Channel,
                                            PaymentChannel.Topics.Payment,
                                            paymentRequestMessage);

        return null;  //Hvorfor virker det her, men ikke i OrderCreationActivity?
    }
}

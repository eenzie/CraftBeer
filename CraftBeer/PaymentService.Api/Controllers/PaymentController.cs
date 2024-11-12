using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.IntegrationEventsIncoming;
using Shared.IntegrationEventsOutgoing;
using Shared.Queues;

namespace PaymentService.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly DaprClient _daprClient;

    public PaymentController(ILogger<PaymentController> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    [Topic(PaymentChannel.Channel, PaymentChannel.Topics.Payment)]
    [HttpPost("payment/do")]
    public async Task<IActionResult> DoPayment([FromBody] PaymentProcessEvent paymentRequest)
    {
        _logger.LogInformation("Payment request received: {CorrelationId}, {Amount}", paymentRequest.CorrelationId,
            paymentRequest.Amount);

        var paymentResponse = new PaymentResultEvent
        {
            CorrelationId = paymentRequest.CorrelationId,
            Amount = paymentRequest.Amount,
            Status = ResultStatus.Succeeded
        };

        await _daprClient.PublishEventAsync(WorkflowChannel.Channel, WorkflowChannel.Topics.PaymentResult,
            paymentResponse);

        _logger.LogInformation("Payment processed: {CorrelationId}, {Amount}, {State}", paymentResponse.CorrelationId,
            paymentResponse.Amount, paymentResponse.Status);

        return Ok();
    }
}

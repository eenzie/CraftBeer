using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;

namespace OrderService.Application.Activities;

public class NotificationActivity : WorkflowActivity<Notification, object?>
{
    private readonly ILogger _logger;

    public NotificationActivity(ILogger<NotificationActivity> logger)
    {
        _logger = logger;
    }

    public override async Task<object?> RunAsync(WorkflowActivityContext context, Notification notification)
    {
        _logger.LogInformation(notification.Message);

        await Task.CompletedTask;
        return null;
    }
}

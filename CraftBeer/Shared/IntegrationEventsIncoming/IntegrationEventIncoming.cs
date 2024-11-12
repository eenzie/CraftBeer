namespace Shared.IntegrationEventsIncoming;

public abstract record IntegrationEventIncoming
{
    public string CorrelationId { get; init; } = string.Empty;
    public ResultStatus Status { get; init; } = ResultStatus.Failed;
}

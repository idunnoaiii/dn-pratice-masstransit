namespace EventBus.Message.IntegrationEvent;

public interface INotificationEvent
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
}


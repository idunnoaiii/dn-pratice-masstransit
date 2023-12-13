using EventBus.Message.IntegrationEvent;
using MassTransit;

namespace Consumer.Api.IntegrationEventHandler;

public class SmsConsumer : IConsumer<INotificationEvent>

{
    public Task Consume(ConsumeContext<INotificationEvent> context)
    {
        return Task.CompletedTask;
    }
}

public class EmailConsumer : IConsumer<INotificationEvent>

{
    public Task Consume(ConsumeContext<INotificationEvent> context)
    {
        return Task.CompletedTask;
    }
}
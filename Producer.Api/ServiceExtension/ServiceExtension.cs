using EventBus.Message.IntegrationEvent;
using Infrastructure;
using MassTransit;
using RabbitMQ.Client;

namespace Producer.Api.ServiceExtension;

public static class ServiceExtension
{
    public static IServiceCollection AddMasstransitConfiguration(this IServiceCollection services, IConfiguration configuration)
    {

        var masstransitConfig = new MasstransitConfiguration();
        configuration.GetSection(nameof(MasstransitConfiguration)).Bind(masstransitConfig);

        services.AddMassTransit(mt =>
        {
            mt.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(masstransitConfig.Host, masstransitConfig.VHost, h =>
                {
                    h.Username(masstransitConfig.Username);
                    h.Password(masstransitConfig.Password);
                });

                cfg.Message<INotificationEvent>(e => e.SetEntityName(masstransitConfig.ExchangeName));

                cfg.Publish<INotificationEvent>(e =>
                {
                    e.ExchangeType = ExchangeType.Topic;
                });
                cfg.Send<INotificationEvent>(e =>
                {
                    e.UseRoutingKeyFormatter(context => context.Message.Type.ToString());
                });
            });

        });

        return services;
    }
}

using Consumer.Api.IntegrationEventHandler;
using Infrastructure;
using Infrastructure.Constants;
using MassTransit;
using RabbitMQ.Client;

namespace Consumer.Api.ServiceExtension;

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
                cfg.Host(masstransitConfig.Host, masstransitConfig.VHost, (h) =>
                {
                    h.Username(masstransitConfig.Username);
                    h.Password(masstransitConfig.Password);
                });

                cfg.ReceiveEndpoint(masstransitConfig.SmsQueueName, rcf =>
                {

                    // turn of default fanout settings
                    rcf.ConfigureConsumeTopology = false; //optional

                    // a relicated queue to provide availability and data safety
                    rcf.SetQuorumQueue(); //optional

                    rcf.SetQueueArgument("declare", "lazy");

                    rcf.Consumer<SmsConsumer>();

                    rcf.Bind(masstransitConfig.ExchangeName, e =>
                    {
                        e.RoutingKey = NotificationTypeConst.Sms;
                        e.ExchangeType = ExchangeType.Topic;
                    });
                });

                cfg.ReceiveEndpoint(masstransitConfig.EmailQueueName, rcf =>
                {

                    // turn of default fanout settings
                    rcf.ConfigureConsumeTopology = false; //optional

                    // a relicated queue to provide availability and data safety
                    rcf.SetQuorumQueue(); //optional

                    rcf.SetQueueArgument("declare", "lazy");

                    rcf.Consumer<EmailConsumer>();

                    rcf.Bind(masstransitConfig.ExchangeName, e =>
                    {
                        e.RoutingKey = NotificationTypeConst.Email;
                        e.ExchangeType = ExchangeType.Topic;
                    });
                });
            });
        });

        return services;
    }
}

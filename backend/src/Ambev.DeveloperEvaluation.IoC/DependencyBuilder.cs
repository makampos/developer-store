using Ambev.DeveloperEvaluation.Application.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Ambev.DeveloperEvaluation.IoC;

public static class DependencyBuilder
{
    public static void AddMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(x =>
        {
            var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMQ");

            x.AddConsumer<SaleCreatedConsumer>();
            x.AddConsumer<SaleModifiedConsumer>();
            x.AddConsumer<SaleCancelledConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqConnectionString!),  h =>
                {
                    h.Username("developer");
                    h.Password("ev@luAt10n");
                });

                cfg.ReceiveEndpoint("Sale_created_queue", e =>
                {
                    e.ConfigureConsumer<SaleCreatedConsumer>(context);
                });

                cfg.ReceiveEndpoint("Sale_modified_queue", e =>
                {
                    e.ConfigureConsumer<SaleModifiedConsumer>(context);
                });

                cfg.ReceiveEndpoint("Sale_cancelled_queue", e =>
                {
                    e.ConfigureConsumer<SaleCancelledConsumer>(context);
                });
            });
        });
    }
}
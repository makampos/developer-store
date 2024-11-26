using Ambev.DeveloperEvaluation.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Consumers;

public class SaleCreatedConsumer : IConsumer<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedConsumer> _logger;

    public SaleCreatedConsumer(ILogger<SaleCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SaleCreatedEvent> context)
    {
        _logger.LogInformation("{SaleCreatedEvent} received with id: {SaleId}", nameof(SaleCreatedEvent), context.Message.ToString());
    }
}
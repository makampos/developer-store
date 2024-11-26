using Ambev.DeveloperEvaluation.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Consumers;

public class SaleModifiedConsumer : IConsumer<SaleModifiedEvent>
{
    private readonly ILogger<SaleModifiedConsumer> _logger;

    public SaleModifiedConsumer(ILogger<SaleModifiedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SaleModifiedEvent> context)
    {
        _logger.LogInformation("{SaleModifiedEvent} received with id: {SaleId}", nameof(SaleModifiedEvent),
            context.Message.ToString());
    }
}
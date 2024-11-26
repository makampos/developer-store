using Ambev.DeveloperEvaluation.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Consumers;

public class SaleCancelledConsumer : IConsumer<SaleCancelledEvent>
{
    private readonly ILogger<SaleCancelledConsumer> _logger;

    public SaleCancelledConsumer(ILogger<SaleCancelledConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SaleCancelledEvent> context)
    {
        _logger.LogInformation("{SaleCancelledEvent} received with id: {SaleId}", nameof(SaleCancelledEvent), context.Message.ToString());
    }
}

using Application.Client.Commands.ProcessClientImport;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace BackgroundServices
{
    public class ClientImportBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ClientImportBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var context = scope.ServiceProvider.GetRequiredService<IClientControlContext>();
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var pendingImports = await context.ClientImports
                        .Where(x => x.Status == Domain.ClientImportStatus.Pending)
                        .OrderBy(x => x.CreatedAt)
                        .Select(x => x.Id)
                        .ToListAsync(stoppingToken);

                    foreach (var importId in pendingImports)
                    {
                        await _mediator.Send(new ProcessClientImportCommandRequest
                        {
                            ImportId = importId
                        }, stoppingToken);
                    }
                }
                catch
                {
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}

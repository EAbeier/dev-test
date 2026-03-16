using Application.Common.Interfaces;
using Domain;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Client.Commands.ImportClient
{
    public class ClientImportCommandHandler : IRequestHandler<ClientImportCommandRequest, Guid>
    {
        private readonly IClientControlContext _context;

        public ClientImportCommandHandler(IClientControlContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(ClientImportCommandRequest request, CancellationToken cancellationToken)
        {
            var clientImport = new ClientImport(request.FileName, request.StoredFilePath);

            await _context.ClientImports.AddAsync(clientImport, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return clientImport.Id;
        }
    }
}

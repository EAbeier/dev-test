using Application.Client.Queries.ClientByIdQuery;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Client.Queries.ClientByDocumentQuery
{
    public class ClientByDocumentQueryHandler : IRequestHandler<ClientByDocumentQueryRequest, IEnumerable<ClientByDocumentQueryResponse>>
    {
        private readonly IClientControlContext _context;

        public ClientByDocumentQueryHandler(IClientControlContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClientByDocumentQueryResponse>> Handle(ClientByDocumentQueryRequest request, CancellationToken cancellationToken)
        {
            var client = await _context.Clients
                .Where(x => x.DocumentNumber.Contains(request.DocumentNumber))
                .Select(x => new ClientByDocumentQueryResponse
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    DocumentNumber = x.DocumentNumber
                }).ToListAsync();

            return client;
        }
    }
}

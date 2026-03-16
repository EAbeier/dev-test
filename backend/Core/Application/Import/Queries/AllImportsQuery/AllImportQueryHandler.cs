using Application.Client.Queries.AllClientsQuery;
using Application.Client.Queries.ClientByIdQuery;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Import.Queries.AllImportsQuery
{
    public class AllImportQueryHandler : IRequestHandler<AllImportsQueryRequest, IEnumerable<AllImportsQueryResponse>>
    {

        private readonly IClientControlContext _context;
        public AllImportQueryHandler(IClientControlContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AllImportsQueryResponse>> Handle(AllImportsQueryRequest request, CancellationToken cancellationToken)
        {
            var clients = await _context.ClientImports
                 .Select(x => new AllImportsQueryResponse
                 {
                     Id = x.Id,
                     FileName = x.FileName,
                     TotalRows = x.TotalRows,
                     SuccesRows = x.SuccessRows,
                     FailedRows = x.FailedRows,
                     CreatedAt = x.CreatedAt,
                     FinishedAt = x.FinishedAt
                 })
                 .OrderBy(x => x.FinishedAt)
                 .ToListAsync();

            return clients;
        }
    }
}

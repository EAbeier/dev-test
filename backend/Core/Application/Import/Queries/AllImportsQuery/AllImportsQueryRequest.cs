using Application.Client.Queries.AllClientsQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Import.Queries.AllImportsQuery
{
    public class AllImportsQueryRequest : IRequest<IEnumerable<AllImportsQueryResponse>>
    {
    }
}

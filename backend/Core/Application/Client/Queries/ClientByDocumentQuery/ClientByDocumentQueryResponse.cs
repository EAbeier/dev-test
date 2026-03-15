using Application.Client.Models;
using Application.Client.Queries.ClientByIdQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Client.Queries.ClientByDocumentQuery
{
    public class ClientByDocumentQueryResponse: ClientModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

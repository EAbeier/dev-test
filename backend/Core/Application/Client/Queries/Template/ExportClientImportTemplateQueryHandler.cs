using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Client.Queries.Template
{
    public class ExportClientImportTemplateQueryHandler : IRequestHandler<ExportClientImportTemplateRequest, byte[]>
    {
        public Task<byte[]> Handle(
          ExportClientImportTemplateRequest request,
          CancellationToken cancellationToken)
        {
            var csv =
                "FirstName,LastName,PhoneNumber,DocumentNumber,Email,BirthDate,PostalCode,AddressLine,Number,Complement,Neighborhood,City,State\n" +
                "Joao,Silva,51999999999,12345678900,joao@email.com,03-15-2026,90000-000,Rua A,123,Apto 101,Centro,Porto Alegre,RS";

            var bytes = Encoding.UTF8.GetBytes(csv);

            return Task.FromResult(bytes);
        }
    }
}

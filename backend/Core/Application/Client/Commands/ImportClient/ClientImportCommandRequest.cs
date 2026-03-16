using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Client.Commands.ImportClient
{
    public class ClientImportCommandRequest : IRequest<Guid>
    {
        public string FileName { get; set; } = default!;
        public string StoredFilePath { get; set; } = default!;
    }
}

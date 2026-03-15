using Application.Client.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Client.Commands.UpdateClient
{
    public class UpdateClientCommandRequest : ClientModel, IRequest
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

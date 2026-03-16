using Application.Client.Commands.CreateClient;
using Application.Client.Commands.ImportClient;
using Application.Client.Commands.UpdateClient;
using Application.Client.Queries.AllClientsQuery;
using Application.Client.Queries.ClientByDocumentQuery;
using Application.Client.Queries.ClientByIdQuery;
using Application.Client.Queries.Template;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _mediator.Send(new Application.Import.Queries.AllImportsQuery.AllImportsQueryRequest());
            return Ok(users);
        }

    }
}
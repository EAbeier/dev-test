using Application.Client.Commands.CreateClient;
using Application.Client.Queries.AllClientsQuery;
using Application.Client.Queries.ClientByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateClientCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClientByIdQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new ClientByIdQueryRequest { Id = id });

            return response is null ? NotFound() : Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AllClientsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromQuery] string? document)
        {
            if (!string.IsNullOrWhiteSpace(document))
            {
                var client = await _mediator.Send(new ClientByDocumentQueryRequest
                {
                    DocumentNumber = document
                });

                return client is null ? NotFound() : Ok(client);
            }

            var clients = await _mediator.Send(new AllClientsQueryRequest());
            return Ok(clients);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateClientCommandRequest request)
        {
            if (id != request.Id)
                return BadRequest("O id da rota deve ser igual ao id do corpo da requisição.");

            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClientByIdQueryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new ClientByIdQueryRequest { Id = id });

            return Ok(response);
        }
    }
}

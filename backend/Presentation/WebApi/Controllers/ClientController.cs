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
        public async Task<IActionResult> Get([FromQuery] string documentNumber)
        {
            if (!string.IsNullOrWhiteSpace(documentNumber))
            {
                var client = await _mediator.Send(new ClientByDocumentQueryRequest
                {
                    DocumentNumber = documentNumber
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


        [HttpPost("import")]
        public async Task<IActionResult> ImportClients(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Arquivo CSV inválido.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var storedFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var storedFilePath = Path.Combine(uploadsFolder, storedFileName);

            await using (var stream = new FileStream(storedFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            var importId = await _mediator.Send(new ClientImportCommandRequest
            {
                FileName = file.FileName,
                StoredFilePath = storedFilePath
            }, cancellationToken);

            return Accepted(new
            {
                importId,
                message = "Importação recebida e será processada em background."
            });

        }

        [HttpGet("Template")]
        public async Task<IActionResult> DownloadImportTemplate(CancellationToken cancellationToken)
        {
            var bytes = await _mediator.Send(new ExportClientImportTemplateRequest(), cancellationToken);

            return File(bytes, "text/csv", "client-import-template.csv");
        }
    }
}
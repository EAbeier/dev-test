using Application.Client.Commands.CreateClient;
using Application.Client.Models;
using Application.Common.Interfaces;
using CsvHelper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Client.Commands.ProcessClientImport
{
    public class ProcessClientImportCommandHandler : IRequestHandler<ProcessClientImportCommandRequest>
    {
        private readonly IClientControlContext _context;
        private readonly IMediator _mediator;

        public ProcessClientImportCommandHandler(IClientControlContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ProcessClientImportCommandRequest request, CancellationToken cancellationToken)
        {
            var import = await _context.ClientImports
                .FirstOrDefaultAsync(x => x.Id == request.ImportId, cancellationToken);

            if (import == null)
                return Unit.Value;

            if (import.Status != ClientImportStatus.Pending)
                return Unit.Value;

            try
            {
                import.Start();
                await _context.SaveChangesAsync(cancellationToken);

                var rows = await ReadCsvAsync(import.FilePath, cancellationToken);

                import.SetTotalRows(rows.Count);
                await _context.SaveChangesAsync(cancellationToken);

                foreach (var row in rows)
                {
                    try
                    {
                        await _mediator.Send(new CreateClientCommandRequest
                        {
                            FirstName = row.FirstName,
                            LastName = row.LastName,
                            PhoneNumber = row.PhoneNumber,
                            DocumentNumber = row.DocumentNumber,
                            Email = row.Email,
                            BirthDate = DateTime.Parse(row.BirthDate),
                            Address = new AddressModel{
                                PostalCode = row.PostalCode,
                                AddressLine = row.AddressLine,
                                Number = row.Number,
                                Complement = row.Complement,
                                Neighborhood = row.Neighborhood,
                                City = row.City,
                                State = row.State
                            }
                        }, cancellationToken);

                        import.RegisterSuccess();
                    }
                    catch
                    {
                        import.RegisterFailure();
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }

                import.Complete();
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                import.Fail(ex.Message);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }

        private static async Task<List<ClientImportCsvRow>> ReadCsvAsync(string filePath, CancellationToken cancellationToken)
        {
            var rows = new List<ClientImportCsvRow>();

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            await foreach (var record in csv.GetRecordsAsync<ClientImportCsvRow>(cancellationToken))
            {
                rows.Add(record);
            }

            return rows;
        }
    }
}

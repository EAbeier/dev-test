using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ClientImportConfiguration : IEntityTypeConfiguration<ClientImport>
    {
        public void Configure(EntityTypeBuilder<ClientImport> builder)
        {
            builder.ToTable(nameof(ClientImport));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(x => x.FilePath)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.TotalRows)
                .IsRequired();

            builder.Property(x => x.ProcessedRows)
                .IsRequired();

            builder.Property(x => x.SuccessRows)
                .IsRequired();

            builder.Property(x => x.FailedRows)
                .IsRequired();

            builder.Property(x => x.ErrorMessage)
                .HasColumnType("varchar(1000)");

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.StartedAt);

            builder.Property(x => x.FinishedAt);
        }
    }
}
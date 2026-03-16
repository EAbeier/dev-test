using System;

namespace Domain
{
    public class ClientImport
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public int Status { get; private set; }
        public int TotalRows { get; private set; }
        public int ProcessedRows { get; private set; }
        public int SuccessRows { get; private set; }
        public int FailedRows { get; private set; }
        public string? ErrorMessage { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }

        private ClientImport() { }

        public ClientImport(string fileName, string filePath)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            FilePath = filePath;
            Status = ClientImportStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void Start()
        {
            Status = ClientImportStatus.Processing;
            StartedAt = DateTime.UtcNow;
        }

        public void SetTotalRows(int totalRows)
        {
            TotalRows = totalRows;
        }

        public void RegisterSuccess()
        {
            ProcessedRows++;
            SuccessRows++;
        }

        public void RegisterFailure()
        {
            ProcessedRows++;
            FailedRows++;
        }

        public void Complete()
        {
            Status = ClientImportStatus.Completed;
            FinishedAt = DateTime.UtcNow;
        }

        public void Fail(string errorMessage)
        {
            Status = ClientImportStatus.Failed;
            ErrorMessage = errorMessage;
            FinishedAt = DateTime.UtcNow;
        }
    }

    public static class ClientImportStatus
    {
        public const int Pending = 1;
        public const int Processing = 2;
        public const int Completed = 3;
        public const int Failed = 4;
    }
}
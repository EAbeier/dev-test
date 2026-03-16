using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Import.Models
{
    public class ImportModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public int Status { get; set; }
        public int TotalRows { get; set; }
        public int SuccesRows { get; set; }
        public int FailedRows { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }

    }
}

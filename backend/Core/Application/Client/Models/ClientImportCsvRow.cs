using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Client.Models
{
    public class ClientImportCsvRow
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string DocumentNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string BirthDate { get; set; }
        public string PostalCode { get; set; } = default!;
        public string AddressLine { get; set; } = default!;
        public string Number { get; set; } = default!;
        public string Complement { get; set; }
        public string Neighborhood { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
    }
}

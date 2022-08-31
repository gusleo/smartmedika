using dna.core.auth.Infrastructure;
using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dna.core.service.Models
{
    public class UserModel : IModelBase
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public UserStatus Status { get; set; }
        public int AccessFailedCount { get; set; }
        public string ProviderName { get; set; }

        // not mapped to database
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

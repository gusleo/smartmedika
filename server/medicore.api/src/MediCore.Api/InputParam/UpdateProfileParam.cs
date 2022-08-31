using System;
using MediCore.Data.Infrastructure;

namespace MediCore.Api.InputParam
{
    public class UpdateProfileParam
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public int? RegencyId { get; set; }
        public int? PatientId { get; set; }
    }
}

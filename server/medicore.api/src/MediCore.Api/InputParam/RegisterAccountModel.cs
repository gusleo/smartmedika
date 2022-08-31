using System;
namespace MediCore.Api.InputParam
{
    public class RegisterAccountModel
    {
        public string FirstName {get;set;}
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

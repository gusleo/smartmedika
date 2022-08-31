using System;
using System.Collections.Generic;
using System.Text;

namespace dna.core.auth.Model
{
    public class VerifyPhoneCodeModel
    {
        public string PhoneNumber { get; set; }
        public string PhoneCode { get; set; }
    }
}

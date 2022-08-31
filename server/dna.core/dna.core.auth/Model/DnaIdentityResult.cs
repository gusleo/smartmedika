using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace dna.core.auth.Model
{
    public class DnaIdentityResult
    {
        public DnaIdentityResult(){}
        public DnaIdentityResult(IdentityResult result){
            this.Result = result;
        }
        public IdentityResult Result { get; set; }
        public int UserId { get; set; }
        public string GeneratedToken { get; set; }
        public string GenerateSMSToken { get; set; }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth;
using Microsoft.AspNetCore.Identity;
using dna.core.auth.Entity;
using dna.core.auth.Infrastructure;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    [Route("admin/[controller]")]
    [AuthorizeRoles(MembershipConstant.SuperAdmin, MembershipConstant.Admin, MembershipConstant.Operator, 
        MembershipConstant.Owner, MembershipConstant.Staff)]
    public class IdentityController : Controller
    {
        private readonly IAuthenticationService _auth;
        private readonly UserManager<ApplicationUser> userManager;
        public IdentityController(IAuthenticationService auth, UserManager<ApplicationUser> userManager)
        {
            _auth = auth;
            this.userManager = userManager;
        }
        [HttpGet]
        public Task<int> Get()
        {
            return Task.FromResult(_auth.GetUserId() ?? 0);
        }
    }
}

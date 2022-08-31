using dna.core.auth.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace dna.core.data.Repositories
{
    public class UserRepository: IUserRepository
    {
        readonly IDnaCoreContext _context;
        public UserRepository(IDnaCoreContext context)
        {
            _context = context;
        }

        public bool IsUserPhoneNumberExist(string phoneNumber)
        {
            var result = _context.Users.Where(xx => xx.PhoneNumber == phoneNumber).Select(x => x.PhoneNumber).FirstOrDefault();
            return !String.IsNullOrEmpty(result);
        }
    }
    public interface IUserRepository
    {
        bool IsUserPhoneNumberExist(string phoneNumber);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.AuthServices
{
    public interface IAccountService
    {
        public Task<User?> Login(string email, string password);
        public Task<bool> Register(string name, string surname, string email, string password);
    }
}

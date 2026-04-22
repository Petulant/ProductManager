using ProductManager.Application.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Application.Interface
{
    public interface IAuthService
    {
        Task<long> Register(UserRegisterDto request);
        Task<string> Login(UserLoginDto request); 
        Task<bool> UserExists(string username);
    }
}

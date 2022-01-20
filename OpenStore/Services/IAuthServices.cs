using OpenStore.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Services
{
    public interface IAuthServices
    {
        Task<AuthModel> Register(RegisterModel model);
        Task<AuthModel> GetToken(TokenRequestModel model);
    }
}

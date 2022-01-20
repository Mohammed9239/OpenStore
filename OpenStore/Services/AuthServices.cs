using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenStore.Helper;
using OpenStore.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OpenStore.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly UserManager<User> _userManager;
        private readonly JWT _jwt;

        public AuthServices(UserManager<User> userManager , IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        public async Task<AuthModel> Register(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new AuthModel { Message = "الايميل موجود" };

            if (await _userManager.FindByNameAsync(model.UserName) != null)
                return new AuthModel { Message = "اسم المستخدم موجود" };

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                ProvinceID = model.ProvinceID,
                PhoneNumber = model.Phone,
                Street = model.Streat
            };

           var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var erorrs = string.Empty;
                foreach (var erorr in result.Errors)
                {
                    erorrs += erorr.Description;
                }

                return new AuthModel { Message = erorrs };
            }

            await _userManager.AddToRoleAsync(user, "مشتري");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                IsAuthenticated = true,
                UserName = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };
        }


        public async Task<AuthModel> GetToken(TokenRequestModel model)
        {
            var authModel = new AuthModel();

            var user =await _userManager.FindByNameAsync(model.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "اسم المستخدم او كلمة المرور خاطئة";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }


        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);


            return jwtSecurityToken;
        }
    }
}

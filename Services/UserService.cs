using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtAPI.Models;
using JwtAPI.Helpers;

namespace JwtAPI.Services
{

    public interface IUserService
    {
        string Login(string username, string password);
    }

    public class  UserService: IUserService
    {
        //users list 
        private List<User> _users=new List<User>
        {
            new User{ Id=1, Username="test", Password="test"}
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string Login(string username, string password)
        {
            //check user name and password
            var user = _users.SingleOrDefault(u=> u.Username == username && u.Password == password);

            //return null if user is not found
            if( user == null)
                return null;
            
            //authentication successful, generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
        
    }

}

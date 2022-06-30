using API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Utils
{
    public class ApplyJwt
    {
        //sdsd
        public static string GetJwt(Employees employees, IConfiguration _configuration)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("Email", employees.email));
            claims.Add(new Claim("Name", employees.name));
            claims.Add(new Claim("employee_id", employees.employee_id));
            claims.Add(new Claim("roles", employees.role.roleName));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn
                );
            if (token != null)
            {
                var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
                claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
                return idtoken;
            }

            return "Token Tidak Tersedia";

        }
    }
}

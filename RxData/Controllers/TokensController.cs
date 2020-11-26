using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RxData.Data;
using RxData.DTO;
using RxData.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RxData.Controllers
{
    [Route("api/[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly RxContext _context;

        public TokensController(IConfiguration config, RxContext context)
        {
            _config = config;
            _context = context;
        }

        // POST: api/Tokens
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<AuthToken> CreateToken([FromBody] Login login)
        {
            var user = _context.Users
                .Where(u => u.Email == login.Email)
                .SingleOrDefault();

            if (user != null && login.Password == "test")
            {
                var tokenString = BuildToken(user);

                return Ok(new AuthToken { User = user, Token = tokenString });
            }

            return Unauthorized();
        }

        private string BuildToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.UserData, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

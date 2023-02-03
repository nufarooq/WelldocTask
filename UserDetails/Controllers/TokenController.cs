using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserDetails.Models;
using UserDetails.Repository;
using UserDetails.Repository.Interface;

namespace UserDetails.Controllers
{
    [EnableCors("corsapp")]
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IUser _user;

        public TokenController(IConfiguration config, IUser user)
        {
            _configuration = config;
            _user = user;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserLogin login)
        {
            var result = await _user.GetUser(login.EmailId, login.Password);

            if (result != null)
            {
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", result.UserId.ToString()),
                        new Claim("FirstName", result.FirstName),
                        new Claim("Email", result.EmailId)
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(1),
                    signingCredentials: signIn);

                var tokenresult = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(tokenresult);
            }
            else
            {
                return BadRequest("Invalid credentials");
            }
        }

        public class UserLogin
        {
            public string EmailId { get; set; }
            public string Password { get; set; }
        }
    }
}

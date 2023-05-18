using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MundoIndigoAPI.Services
{
    public class UserServices
    {
        private readonly IConfiguration _config; 

        public UserServices(IConfiguration config)
        {
            _config = config;
        }
        public string Authenticate(string username, string role)
        {
            var secretKey = _config.GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Role, role)
            });

            //claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(createdToken); 
        }
    }
}

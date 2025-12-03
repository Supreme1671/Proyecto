using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;


namespace Proyecto.Core.Servicios;

public class TokenService
{
    private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, string.IsNullOrEmpty(usuario.Rol.ToString()) ? "Cliente" : usuario.Rol.ToString()),
                new Claim("NombreUsuario", usuario.NombreUsuario)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerarRefreshToken() => Guid.NewGuid().ToString();

        public RefreshDTO GenerarTokens(Usuario usuario)
        {
            var Token = GenerarToken(usuario);
            var RefreshToken = GenerarRefreshToken();

            return new RefreshDTO
            {
                Token = Token,
                TokenRefresh = RefreshToken
            };
        }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BancoArboleda.Data;
using BancoArboleda.Models;

namespace BancoArboleda.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly DataStore _store;
    private readonly IConfiguration _config;

    public AuthController(DataStore store, IConfiguration config)
    {
        _store = store;
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Usuario))
            return BadRequest(new ApiResponse<object> { Success = false, Message = "El usuario es obligatorio." });

        if (string.IsNullOrWhiteSpace(req.Password))
            return BadRequest(new ApiResponse<object> { Success = false, Message = "La contraseña es obligatoria." });

        var cliente = _store.Clientes.FirstOrDefault(c =>
            c.Usuario.Equals(req.Usuario.Trim(), StringComparison.OrdinalIgnoreCase));

        if (cliente == null || !BCrypt.Net.BCrypt.Verify(req.Password, cliente.PasswordHash))
            return Unauthorized(new ApiResponse<object> { Success = false, Message = "Credenciales incorrectas. Verifique sus datos." });

        var token = GenerarToken(cliente);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Autenticación exitosa.",
            Data = new { token, nombre = cliente.NombreCompleto, id = cliente.Id }
        });
    }

    private string GenerarToken(Cliente cliente)
    {
        var secret = _config["Jwt:Secret"] ?? "banco_arboleda_secret_key_2026_seguro";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
            new Claim(ClaimTypes.Name, cliente.Usuario),
            new Claim("nombre", cliente.NombreCompleto)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

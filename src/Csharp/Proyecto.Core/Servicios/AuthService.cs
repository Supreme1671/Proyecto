using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Proyecto.Core.Repositorios;
using Proyecto.Core.Servicios;

namespace Proyecto.Core.Servicios;

public class AuthService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ITokenRepostory _tokenRepo;
    private readonly TokenService _tokenService;
    private readonly IHttpContextAccessor _http;

    public AuthService(
        IUsuarioRepository usuarioRepo,
        ITokenRepostory tokenRepo,
        TokenService tokenService,
        IHttpContextAccessor http)
    {
        _usuarioRepo = usuarioRepo;
        _tokenRepo = tokenRepo;
        _tokenService = tokenService;
        _http = http;
    }

    // ---------------------
    // REGISTER
    // ---------------------
    public object Register(UsuarioRegisterDTO dto)
    {
        if (_usuarioRepo.ExisteUsuario(dto.Email))
            return new { success = false, message = "El email ya está en uso" };

        var usuario = new Usuario
        {
            NombreUsuario = dto.Nombre,
            Email = dto.Email,
            Contrasena = dto.Contrasena,
            Activo = true,
            Rol = "Cliente"

        };

        _usuarioRepo.InsertarUsuario(usuario);

        return new { success = true, message = "Usuario registrado correctamente" };
    }

    // ---------------------
    // LOGIN
    // ---------------------
    public object Login(UsuarioLoginDTO dto)
    {
        var usuario = _usuarioRepo.ObtenerUsuarioPorEmail(dto.Email);

        if (usuario == null || usuario.Contrasena != dto.Contrasena)
            return new { success = false, message = "Credenciales inválidas" };

        var tokens = _tokenService.GenerarTokens(usuario);

        var tokenEntity = new Token
        {
            TokenHash = tokens.TokenRefresh,
            Email = usuario.Email,
            Expiracion = DateTime.UtcNow.AddMinutes(30)
        };

        _tokenRepo.InsertarToken(tokenEntity);

        return new
        {
            success = true,
            usuario = new
            {
                usuario.IdUsuario,
                usuario.NombreUsuario,
                usuario.Email,
                usuario.Rol
            },
            tokens.Token,
            tokens.TokenRefresh
        };
    }

    // ---------------------
    // REFRESH TOKEN
    // ---------------------
    public object Refresh(RefreshDTO dto)
    {
        var token = _tokenRepo.ObtenerToken(dto.TokenRefresh);

        if (token == null || token.Expiracion < DateTime.UtcNow)
            return new { success = false, message = "Refresh token inválido o vencido" };

        var usuario = _usuarioRepo.ObtenerUsuarioPorEmail(token.Email);

        if (usuario == null)
            return new { success = false, message = "Usuario no encontrado" };

        var nuevosTokens = _tokenService.GenerarTokens(usuario);

        _tokenRepo.ReemplazarToken(usuario.IdUsuario, nuevosTokens.TokenRefresh, DateTime.UtcNow.AddMinutes(30));

        return new
        {
            success = true,
            nuevosTokens.Token,
            nuevosTokens.TokenRefresh
        };
    }

    // ---------------------
    // LOGOUT
    // ---------------------
    public object Logout(RefreshDTO dto)
    {
        _tokenRepo.EliminarToken(dto.TokenRefresh);
        return new { success = true, message = "Sesión cerrada" };
    }

    // ---------------------
    // GET /auth/me
    // ---------------------
    public object Me()
    {
        var user = _http.HttpContext?.User;

        if (user == null || !user.Identity!.IsAuthenticated)
            return new { success = false, message = "No estás autenticado" };

        return new
        {
            Email = user.Identity!.Name,
            Rol = user.FindFirst(ClaimTypes.Role)?.Value,
            NombreUsuario = user.FindFirst("NombreUsuario")?.Value
        };
    }

    // ---------------------
    // GET /auth/roles
    // ---------------------
    public object Roles()
    {
        return _usuarioRepo.ObtenerTodosLosRoles();
    }

    // ---------------------
    // POST /usuarios/{id}/roles
    // ---------------------
    public object AsignarRol(int idUsuario, string rol)
    {
        var usuarioActual = _http.HttpContext?.User;

        if (usuarioActual == null || !usuarioActual.IsInRole("Admin"))
            return new { success = false, message = "Solo un Admin puede cambiar roles" };

        var usuario = _usuarioRepo.ObtenerUsuarioPorId(idUsuario);

        if (usuario == null)
            return new { success = false, message = "Usuario no encontrado" };

        _usuarioRepo.ActualizarRol(idUsuario, rol);

        return new { success = true, message = "Rol actualizado correctamente" };
    }
}

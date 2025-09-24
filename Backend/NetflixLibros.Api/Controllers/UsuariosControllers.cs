using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var lista = new[] {
            new { Id = 1, Nombre = "Juan" },
            new { Id = 2, Nombre = "María" }
        };
        return Ok(lista);
    }
}

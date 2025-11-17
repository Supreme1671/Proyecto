using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Core.Entidades;
using Proyecto.Core.Repositorios;
using Proyecto.Core.Servicios;
using Proyecto.Core.Repositorios.ReposDapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Proyecto.Core.DTOs;
using src.Proyecto.Dappers;
using Proyecto.Core.Servicios.Interfaces;
using Proyecto.Dapper;
using System.Data;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Eventos API",
        Version = "v1",
        Description = "API para sistema de gestión de entradas QR",
        Contact = new OpenApiContact
        {
            Name = "sisas Team",
            Email = "soporte@appqr.com"
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IOrdenRepository, OrdenRepository>();
builder.Services.AddScoped<ILocalRepository, LocalRepository>();
builder.Services.AddScoped<IFuncionRepository, FuncionRepository>();
builder.Services.AddScoped<ISectorRepository, SectorRepository>();
builder.Services.AddScoped<IEntradaRepository, EntradaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITarifaRepository, TarifaRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IQRRepository, QRRepository>();
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new MySqlConnection(config.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IQRRepository, QRRepository>();


builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<QrService>();
builder.Services.AddScoped<IQrService, QrService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateLifetime = true
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "Eventos API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/swagger"));

#region CLIENTES
app.MapPost("/api/clientes", (ClienteCreateDTO dto, IClienteRepository repo) =>
{
    var cliente = new Cliente
    {
        DNI = dto.DNI,
        Nombre = dto.Nombre,
        Apellido = dto.Apellido,
        Email = dto.Email,
        Telefono = dto.Telefono
    };
    repo.Add(cliente);
    return Results.Created($"/api/clientes/{cliente.idCliente}", cliente);
}).WithTags("Cliente");

app.MapGet("/api/clientes", (IClienteRepository repo) =>
{
    var clientes = repo.GetAll();
    return Results.Ok(clientes.Select(c => new ClienteDTO
    {
        idCliente = c.idCliente,
        DNI = c.DNI,
        Nombre = c.Nombre,
        Apellido = c.Apellido,
        Email = c.Email,
        Telefono = c.Telefono
    }));
}).WithTags("Cliente");

app.MapGet("/api/clientes/{idCliente}", (int idCliente, IClienteRepository repo) =>
{
    var c = repo.GetById(idCliente);
    if (c is null) return Results.NotFound();
    return Results.Ok(new ClienteDTO
    {
        idCliente = c.idCliente,
        DNI = c.DNI,
        Nombre = c.Nombre,
        Apellido = c.Apellido,
        Email = c.Email,
        Telefono = c.Telefono
    });
}).WithTags("Cliente");

app.MapPut("/api/clientes/{id}", (int idCliente, ClienteUpdateDTO dto, IClienteRepository repo) =>
{
    var c = repo.GetById(idCliente);
    if (c is null) return Results.NotFound();
    c.DNI = dto.DNI;
    c.Nombre = dto.Nombre;
    c.Apellido = dto.Apellido;
    c.Email = dto.Email;
    c.Telefono = dto.Telefono;
    repo.Update(c);
    return Results.NoContent();
}).WithTags("Cliente");

app.MapDelete("/api/clientes/{id}", (int idCliente, IClienteRepository repo) =>
{
    repo.Delete(idCliente);
    return Results.NoContent();
}).WithTags("Cliente");
#endregion

#region ORDENES
app.MapPost("/api/ordenes", (OrdenCreateDTO dto, IOrdenRepository repo) =>
{
    var nueva = new Orden
    {
        idCliente = dto.idCliente,
        Fecha = DateTime.Now,
        Total = 0,
        Detalles = new List<DetalleOrden>()
    };
    repo.Add(nueva);
    return Results.Created($"/api/ordenes/{nueva.idOrden}", nueva);
}).WithTags("Orden");

app.MapGet("/api/ordenes", (IOrdenRepository repo) =>
{
    var ordenes = repo.GetAll();
    return Results.Ok(ordenes.Select(o => new OrdenDTO
    {
        idOrden = o.idOrden,
        idCliente = o.idCliente,
        Fecha = o.Fecha,
        Total = o.Total,
        Detalles = (o.Detalles ?? new List<DetalleOrden>()).Select(d => new DetalleOrdenDTO
        {
            IdDetalleOrden = d.IdDetalleOrden,
            Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario
        }).ToList()
    }));
}).WithTags("Orden");

app.MapGet("/api/ordenes/{id}", (int idOrden, IOrdenRepository repo) =>
{
    var o = repo.GetById(idOrden);
    if (o is null) return Results.NotFound();
    return Results.Ok(new OrdenDTO
    {
        idOrden = o.idOrden,
        idCliente = o.idCliente,
        Fecha = o.Fecha,
        Total = o.Total,
        Detalles = (o.Detalles ?? new List<DetalleOrden>()).Select(d => new DetalleOrdenDTO
        {
            IdDetalleOrden = d.IdDetalleOrden,
            Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario
        }).ToList()
    });
}).WithTags("Orden");


app.MapPost("/ordenes/{ordenId}/pagar", (int IdOrden, IOrdenRepository repo) =>
{
    var ok = repo.Pagar(IdOrden);
    return ok ? Results.Ok() : Results.BadRequest("No se puede pagar");
}).WithTags("Orden");

app.MapPost("/ordenes/{ordenId}/cancelar", (int IdOrden, IOrdenRepository repo) =>
{
    repo.Cancelar(IdOrden);
    return Results.Ok();
}).WithTags("Orden");

#endregion 
#region ENTRADAS
app.MapGet("/api/entradas", (IEntradaRepository repo) =>
{
    var entradas = repo.GetAll();
    return Results.Ok(entradas.Select(e => new EntradaDTO
    {
        idEntrada = e.IdEntrada,
        Precio = (int)e.Precio,
        Numero = e.Numero,
        Usada = e.Usada,
        Anulada = e.Anulada,
        QR = e.QR
    }));
}).WithTags("Entradas");

app.MapGet("/entradas/{entradaId}", (int IdEntrada, IEntradaRepository repo) =>
{
    var e = repo.GetById(IdEntrada);
    return e is null ? Results.NotFound() : Results.Ok(e);
}).WithTags("Entradas");

app.MapPost("/api/entradas", (Entrada e, IEntradaRepository repo) =>
{
    repo.Add(e);
    return Results.Created($"/api/entradas/{e.IdEntrada}", e);
}).WithTags("Entradas");
#endregion

#region EVENTOS
app.MapPost("/eventos", (EventoDTO dto, IEventoRepository repo) =>
{
   var evento = new Evento
{
    Nombre = dto.Nombre,
    Fecha = dto.Fecha,
    Activo = dto.Activo,
    idLocal = dto.idLocal
};

var idEvento = repo.Add(evento);
return Results.Created($"/eventos/{idEvento}", evento);

}).WithTags("Eventos");

app.MapGet("/api/eventos", (IEventoRepository repo) =>
{
    var eventos = repo.GetAll();
    return Results.Ok(eventos.Select(e => new EventoDTO
    {
        idEvento = e.idEvento,
        Nombre = e.Nombre,
        Fecha = e.Fecha,
        Activo = e.Activo,
        idLocal = e.Local?.idLocal ?? 0
    }));
}).WithTags("Eventos");

app.MapGet("/eventos/{eventoId}", (int eventoId, IEventoRepository repo) =>
{
    var e = repo.GetById(eventoId);
    return e is null ? Results.NotFound() : Results.Ok(e);
}).WithTags("Eventos");

app.MapPut("/eventos/{eventoId}", (int IdEvento, EventoUpdateDTO dto, IEventoRepository repo) =>
{
    var ok = repo.Update(IdEvento, dto);
    return ok ? Results.NoContent() : Results.NotFound();
}).WithTags("Eventos");

app.MapPost("/api/eventos", (EventoCreateDTO dto, IEventoRepository repo) =>
{
    var ev = new Evento
    {
        Nombre = dto.Nombre,
        Fecha = dto.Fecha,
        Activo = true,
        Lugar = dto.Lugar,
        Tipo = dto.Tipo
    };
    repo.Add(ev);
    return Results.Created($"/api/eventos/{ev.idEvento}", ev);
}).WithTags("Eventos");

app.MapPost("/eventos/{eventoId}/cancelar", (int IdEvento, IEventoRepository repo) =>
{
    var ok = repo.Cancelar(IdEvento);
    return ok ? Results.Ok() : Results.NotFound();
}).WithTags("Eventos");
#endregion

#region FUNCIONES


app.MapPost("/api/funciones", (FuncionCreateDTO dto, IFuncionRepository repo) =>
{
    var f = new Funcion
    {
        IdEvento = dto.idEvento,
        FechaHora = dto.Fecha,
        IdLocal = dto.idLocal
    };
    repo.Add(f);
    return Results.Created($"/api/funciones/{f.IdFuncion}", f);
}).WithTags("Funcion");
app.MapGet("/api/funciones", (IFuncionRepository repo) =>
{
    var funciones = repo.GetAll();
    return Results.Ok(funciones.Select(f => new FuncionDTO
    {
        idFuncion = f.IdFuncion,
        idEvento = f.IdEvento,
        Fecha = f.FechaHora,
        idLocal = f.IdLocal
    }));
}).WithTags("Funcion");

app.MapGet("/funciones/{funcionId}", (int IdFuncion, IFuncionRepository repo) =>
{
    var f = repo.GetById(IdFuncion);
    return f is null ? Results.NotFound() : Results.Ok(f);
}).WithTags("Funcion");

app.MapPut("/funciones/{funcionId}", (int IdFuncion, FuncionUpdateDTO dto, IFuncionRepository repo) =>
{
    var ok = repo.Update(IdFuncion, dto);
    return ok ? Results.NoContent() : Results.NotFound();
}).WithTags("Funciones");

app.MapPost("/funciones/{funcionId}/cancelar", (int IdFuncion, IFuncionRepository repo) =>
{
   var existente = repo.GetById(IdFuncion);
if (existente == null) return Results.NotFound();

repo.Update(existente);

return Results.NoContent();

}).WithTags("Funciones");
#endregion

#region LOCAL

app.MapPost("/api/locales", (LocalCreateDTO dto, ILocalRepository repo) =>
{
    var local = new Local
    {
        Nombre = dto.Nombre,
        Direccion = dto.Direccion,
        Capacidad = dto.Capacidad,
        Telefono = dto.Telefono
    };
    var id = repo.Add(local);
    return Results.Created($"/api/locales/{id}", local);
}).WithTags("Local");

app.MapGet("/api/locales", (ILocalRepository repo) =>
{
    var locales = repo.GetAll();
    return Results.Ok(locales.Select(l => new LocalDTO
    {
        idLocal = l.idLocal,
        Nombre = l.Nombre,
        Direccion = l.Direccion,
        Capacidad = l.Capacidad,
        Telefono = l.Telefono
    }));
}).WithTags("Local");


app.MapGet("/locales/{localId}", (int IdLocal, ILocalRepository repo) =>
{
    var l = repo.GetById(IdLocal);
    return l is null ? Results.NotFound() : Results.Ok(l);
}).WithTags("Local");

app.MapPut("/locales/{localId}", async (int IdLocal, Local local, ILocalRepository repo) =>
{
    local.idLocal = IdLocal;

    var existente = repo.GetById(IdLocal);
    if (existente == null)
        return Results.NotFound("Local no encontrado.");

    repo.Update(local); 

    return Results.Ok(local);
}).WithTags("Local");
app.MapDelete("/locales/{localId}", (int IdLocal, ILocalRepository repo) =>
{
    var ok = repo.Delete(IdLocal);
    return ok ? Results.NoContent() : Results.BadRequest("No se puede eliminar (tiene funciones vigentes)");
}).WithTags("Local");
#endregion

#region SECTOR
app.MapPost("/api/sectores", (SectorCreateDTO dto, ISectorRepository repo) =>
{
    var s = new Sector
    {
        Nombre = dto.Nombre,
        idLocal = dto.idLocal,
        Capacidad = dto.Capacidad, 
        Precio = dto.Precio
    };
    repo.Add(s);
    return Results.Created($"/api/sectores/{s.idSector}", s);
}).WithTags("Sector");
app.MapGet("/api/sectores", (ISectorRepository repo) =>
{
    var sectores = repo.GetAll();
    return Results.Ok(sectores.Select(s => new SectorDTO
    {
        idSector = s.idSector,
        Nombre = s.Nombre,
        idLocal = s.idLocal
    }));
}).WithTags("Sector");

app.MapPut("/sectores/{sectorId}", (int IdSector, SectorDTO dto, ISectorRepository repo) =>
{
    var ok = repo.Update(IdSector, dto);
    return ok ? Results.NoContent() : Results.NotFound();
}).WithTags("Sector");


app.MapDelete("/sectores/{sectorId}", (int IdSector, ISectorRepository repo) =>
{
    var ok = repo.Delete(IdSector);
    return ok ? Results.NoContent() : Results.BadRequest("Sector con tarifas/funciones asociadas");
}).WithTags("Sectores");
#endregion

#region TARIFAS
app.MapPost("/api/tarifas", (TarifaCreateDTO dto, ITarifaRepository repo) =>
{
    var t = new Tarifa
    {
        Precio = dto.Precio,
        IdFuncion = dto.idFuncion
    };
    repo.Add(t);
    return Results.Created($"/api/tarifas/{t.idTarifa}", t);
}).WithTags("Tarifas");
app.MapGet("/api/funciones/{funcionId}/tarifas", (int funcionId, ITarifaRepository repo) =>
{
    var tarifas = repo.GetByFuncionId(funcionId);
    return Results.Ok(tarifas.Select(t => new TarifaDTO
    {
        idTarifa = t.idTarifa,
        Precio = t.Precio,
        idFuncion = t.IdFuncion
    }));
}).WithTags("Tarifas");


app.MapPut("/tarifas/{tarifaId}", (int IdTarifa, TarifaUpdateDTO dto, ITarifaRepository repo) =>
{
    var ok = repo.Update(IdTarifa, dto);
    return ok ? Results.NoContent() : Results.NotFound();
}).WithTags("Tarifas");

app.MapGet("/tarifas/{tarifaId}", (int tarifaId, ITarifaRepository repo) =>
{
    var t = repo.GetById(tarifaId);
    return t is null ? Results.NotFound() : Results.Ok(t);
}).WithTags("Tarifas");
#endregion

#region ROLES
app.MapGet("/api/roles", (IRolRepository repo) => Results.Ok(repo.GetAll()))
.WithTags("Roles");
app.MapPost("/api/roles", (Rol r, IRolRepository repo) =>
{
    repo.Add(r);
    return Results.Created($"/api/roles/{r.IdRol}", r);
}) .WithTags("Roles");
#endregion

#region USUARIOS
app.MapPost("/Usuario/register", (UsuarioRegisterDTO dto, AuthService auth) =>
{
    var id = auth.Register(dto);
    return Results.Created($"/usuarios/{id}", dto);
}).WithTags("Usuario");

app.MapPost("/auth/login", (UsuarioLoginDTO login, IUsuarioRepository repo) =>
{
    var usuario = repo.Login(login.NombreUsuario, login.Contrasena);
    if (usuario is null) return Results.Unauthorized();

    return Results.Ok(new UsuarioDTO
    {
        idUsuario = usuario.IdUsuario,
        NombreUsuario = usuario.NombreUsuario,
    });
}).WithTags("Usuario");

app.MapPost("/usuarios", (Usuario nuevoUsuario, IUsuarioRepository repo) =>
{
    repo.Add(nuevoUsuario);
    return Results.Created($"/usuarios/{nuevoUsuario.IdUsuario}", nuevoUsuario);
}).WithTags("Usuario");


app.MapPost("/auth/refresh", (RefreshDTO dto, AuthService auth) =>
{
    var token = auth.Refresh(dto);
    return token is null ? Results.Unauthorized() : Results.Ok(token);
}).WithTags("Usuario");

app.MapGet("/usuarios/{id}/roles", (int idUsuario, IUsuarioRepository repo) =>
{
    var roles = repo.GetRoles(idUsuario);
    return Results.Ok(roles);
}).WithTags("Usuario");

app.MapGet("/roles", (IUsuarioRepository repo) => Results.Ok(repo.GetAllRoles()))
.WithTags("Usuario");

app.MapPost("/usuarios/{id}/roles/{rolId}", (int idUsuario, int rolId, IUsuarioRepository repo) =>
{
    repo.AsignarRol(idUsuario, rolId);
    return Results.Ok(new { mensaje = "Rol asignado correctamente" });
}).WithTags("Rol");
#endregion

#region QR
app.MapGet("/entradas/{idEntrada}/qr", (int idEntrada, QrService qrService, IEntradaRepository repo) =>
{
    var entrada = repo.GetById(idEntrada);
    if (entrada == null) return Results.NotFound("Entrada no existe");

    string qrContent = $"{entrada.IdEntrada}|{entrada.IdFuncion}|{builder.Configuration["Qr:Key"]}";
    var qrBytes = qrService.GenerarQrEntradaImagen(qrContent);
    return Results.File(qrBytes, "image/png");
}) .WithTags("QR");

app.MapPost("/qr/lote", (List<int> idEntradas, QrService qrService, IEntradaRepository repo) =>
{
    var resultados = new Dictionary<int, byte[]>();
    foreach (var idEntrada in idEntradas)
    {
        var entrada = repo.GetById(idEntrada);
        if (entrada == null) continue;
        string qrContent = $"{entrada.IdEntrada}|{entrada.IdFuncion}|{builder.Configuration["Qr:Key"]}";
        var qrBytes = qrService.GenerarQrEntradaImagen(qrContent);
        resultados.Add(entrada.IdEntrada, qrBytes);
    }
    return Results.Ok(resultados);
}).WithTags("QR");

app.MapPost("/qr/validar", (string qrContent, QrService qrService) =>
{
    var resultado = qrService.ValidarQr(qrContent);
    return Results.Ok(resultado);
});

app.MapPost("/api/qr/{idEntrada}", (int idEntrada, IQrService qrService) =>
{
    var qr = qrService.GenerarQr(idEntrada);
    return Results.Ok(qr);
}).WithTags("QR");

#endregion

app.Run();


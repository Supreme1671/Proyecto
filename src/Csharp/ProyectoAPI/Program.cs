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
using Repositorios.Repos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
builder.Services.AddScoped<ITokenRepostory, TokenRepository>();
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
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddHttpContextAccessor();

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
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        ),

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
    if (dto.DNI <= 0)
        return Results.BadRequest("El DNI debe ser mayor a 0.");

    if (string.IsNullOrWhiteSpace(dto.Nombre))
        return Results.BadRequest("El nombre es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.Apellido))
        return Results.BadRequest("El apellido es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.Email))
        return Results.BadRequest("El email es obligatorio.");

 var cliente = new Cliente
{
    DNI = dto.DNI,
    Nombre = dto.Nombre,
    Apellido = dto.Apellido,
    Email = dto.Email,
    Telefono = dto.Telefono
};


    repo.Add(cliente);

    var clienteDTO = new ClienteDTO
    {
        DNI = cliente.DNI,
        Nombre = cliente.Nombre,
        Apellido = cliente.Apellido,
        Email = cliente.Email,
        Telefono = cliente.Telefono
    };

    return Results.Created($"/api/clientes/{cliente.idCliente}", clienteDTO);
})
.WithTags("Cliente");

app.MapGet("/api/clientes", (IClienteRepository repo) =>
{
    var clientes = repo.GetAll();

    var clientesDTO = clientes.Select(c => new ClienteDTO
    {
        idCliente = c.idCliente,
        DNI = c.DNI,
        Nombre = c.Nombre,
        Apellido = c.Apellido,
        Email = c.Email,
        Telefono = c.Telefono
    });

    return Results.Ok(clientesDTO);
})
.WithTags("Cliente");

app.MapGet("/api/clientes/{idCliente}", (int idCliente, IClienteRepository repo) =>
{
    var c = repo.GetById(idCliente);
    if (c is null)
        return Results.NotFound("Cliente no encontrado.");

    var dto = new ClienteDTO
    {
        idCliente = c.idCliente,
        DNI = c.DNI,
        Nombre = c.Nombre,
        Apellido = c.Apellido,
        Email = c.Email,
        Telefono = c.Telefono
    };

    return Results.Ok(dto);
})
.WithTags("Cliente");

app.MapPut("/api/clientes/{idCliente}", (int idCliente, ClienteUpdateDTO dto, IClienteRepository repo) =>
{
var cliente = repo.GetById(idCliente);
if (cliente is null)
return Results.NotFound("Cliente no encontrado.");

if (dto.DNI <= 0)
    return Results.BadRequest("El DNI debe ser mayor a 0.");

if (string.IsNullOrWhiteSpace(dto.Nombre))
    return Results.BadRequest("El nombre es requerido.");

if (string.IsNullOrWhiteSpace(dto.Apellido))
    return Results.BadRequest("El apellido es requerido.");

if (string.IsNullOrWhiteSpace(dto.Email))
    return Results.BadRequest("El email es requerido.");

cliente.DNI = dto.DNI;
cliente.Nombre = dto.Nombre;
cliente.Apellido = dto.Apellido;
cliente.Email = dto.Email;
cliente.Telefono = dto.Telefono;

repo.Update(cliente);

return Results.Ok(cliente);


})
.WithTags("Cliente");


#endregion

#region ORDENES
app.MapPost("/api/ordenes", (
    OrdenCreateDTO dto,
    IOrdenRepository repo,
    ITarifaRepository tarifaRepo) =>
{
    if (dto.idFuncion.Count != dto.idTarifa.Count ||
        dto.idTarifa.Count != dto.Cantidades.Count)
        return Results.BadRequest("Las listas idFunciones, idTarifas y Cantidades deben tener la misma longitud.");

    var orden = new Orden
    {
        idCliente = dto.IdCliente,
        Fecha = DateTime.Now,
        Estado = "Creada",
        Detalles = new List<DetalleOrden>()
    };

    for (int i = 0; i < dto.idTarifa.Count; i++)
    {
        var tarifa = tarifaRepo.GetById(dto.idTarifa[i]);
        if (tarifa is null)
            return Results.BadRequest($"La tarifa {dto.idTarifa[i]} no existe");

        if (tarifa.Stock < dto.Cantidades[i])
            return Results.BadRequest($"No hay stock suficiente para la tarifa {dto.idTarifa[i]}");

        tarifaRepo.RestarStock(dto.idTarifa[i], dto.Cantidades[i]);

        var detalle = new DetalleOrden
        {
            IdTarifa = dto.idTarifa[i],
            Cantidad = dto.Cantidades[i],
            PrecioUnitario = tarifa.Precio,
            IdEvento = tarifa.idEvento,
            IdFuncion = tarifa.idFuncion
        };

        orden.Detalles.Add(detalle);
    }

    orden.Total = orden.Detalles.Sum(d => d.PrecioUnitario * d.Cantidad);

    repo.Add(orden);

    var salida = new OrdenDTO
    {
        IdOrden = orden.idOrden,
        IdCliente = orden.idCliente,
        Fecha = orden.Fecha,
        Total = orden.Total,
        Detalles = orden.Detalles.Select(d => new DetalleOrdenDTO
        {
            IdDetalleOrden = d.IdDetalleOrden,
            idEvento = d.IdEvento,
            idTarifa = d.IdTarifa,
            Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario
        }).ToList()
    };

    return Results.Created($"/api/ordenes/{orden.idOrden}", salida);
})
.WithTags("Orden");



app.MapGet("/api/ordenes", (IOrdenRepository repo) =>
{
    var ordenes = repo.GetAll();

   var salida = ordenes.Select(o => new OrdenDTO
{
    IdOrden = o.idOrden,
    IdCliente = o.idCliente,
    Fecha = o.Fecha,
    Total = o.Total,
    Estado = o.Estado, 

    Detalles = o.Detalles.Select(d => new DetalleOrdenDTO
    {
        IdDetalleOrden = d.IdDetalleOrden,
        idEvento = d.IdEvento,
        idTarifa = d.IdTarifa,
        Cantidad = d.Cantidad,
        PrecioUnitario = d.PrecioUnitario
    }).ToList()
    });

    return Results.Ok(salida);
})
.WithTags("Orden");


app.MapGet("/api/ordenes/{idOrden}", (int idOrden, IOrdenRepository repo) =>
{
    var o = repo.GetById(idOrden);
    if (o is null)
        return Results.NotFound();

 var dto = new OrdenDTO
{
    IdOrden = o.idOrden,
    IdCliente = o.idCliente,
    Fecha = o.Fecha,
    Total = o.Total,
    Estado = o.Estado,  

    Detalles = o.Detalles.Select(d => new DetalleOrdenDTO
    {
        IdDetalleOrden = d.IdDetalleOrden,
        idEvento = d.IdEvento,
        idTarifa = d.IdTarifa,
        Cantidad = d.Cantidad,
        PrecioUnitario = d.PrecioUnitario
    }).ToList()
    };

    return Results.Ok(dto);
})
.WithTags("Orden");


app.MapPost("/api/ordenes/{idOrden}/pagar", (int idOrden, IOrdenRepository repo) =>
{
    var ok = repo.Pagar(idOrden);
    return ok ? Results.Ok("Orden pagada.") : Results.BadRequest("No se pudo pagar la orden.");
})
.WithTags("Orden");

app.MapPost("/api/ordenes/{idOrden}/cancelar", (int idOrden, IOrdenRepository repo) =>
{
    bool ok = repo.Cancelar(idOrden);
    return ok
        ? Results.Ok("Orden cancelada.")
        : Results.BadRequest("La orden no se puede cancelar.");
})
.WithTags("Orden");


#endregion 

#region ENTRADAS
app.MapGet("/api/entradas", (IEntradaRepository repo) =>
{
    var entradas = repo.GetAll();

    var entradasDTO = entradas.Select(e => new EntradaDTO
    {
        idEntrada = e.IdEntrada,
        Precio = (int)e.Precio,
        Numero = e.Numero,
        Usada = e.Usada,
        Anulada = e.Anulada,
        QR = e.QR
    });

    return Results.Ok(entradasDTO);
})
.WithTags("Entradas");

app.MapGet("/api/entradas/{idEntrada}", (int idEntrada, IEntradaRepository repo) =>
{
    var e = repo.GetById(idEntrada);
    if (e is null)
        return Results.NotFound("Entrada no encontrada");

    var dto = new EntradaDTO
    {
        idEntrada = e.IdEntrada,
        Precio = (int)e.Precio,
        Numero = e.Numero,
        Usada = e.Usada,
        Anulada = e.Anulada,
        QR = e.QR
    };

    return Results.Ok(dto);
})
.WithTags("Entradas");

app.MapPost("/api/entradas", (EntradaCreateDTO dto, IEntradaRepository repo) =>
{
    if (dto.Precio < 0)
        return Results.BadRequest("El precio no puede ser negativo.");

    var entrada = new Entrada
    {
        Precio = dto.Precio,
        Numero = dto.Numero,
        Usada = dto.Usada,
        Anulada = dto.Anulada,
        QR = dto.QR
    };

    repo.Add(entrada);

    var salida = new EntradaDTO
    {
        idEntrada = entrada.IdEntrada,
        Precio = (int)entrada.Precio,
        Numero = entrada.Numero,
        Usada = entrada.Usada,
        Anulada = entrada.Anulada,
        QR = entrada.QR
    };

    return Results.Created($"/api/entradas/{entrada.IdEntrada}", salida);
})
.WithTags("Entradas");

#endregion

#region EVENTOS
app.MapPost("/eventos", (EventoCreateDTO dto, IEventoRepository repo) =>
{
   var evento = new Evento
   {
        Nombre = dto.Nombre,
        Fecha = dto.Fecha,
        Tipo = dto.Tipo,
        Descripcion = dto.Descripcion,
        Activo = true,
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
        Tipo = e.Tipo,
        Descripcion = e.Descripcion,
        Activo = e.Activo,
        idLocal = e.idLocal
    }));
}).WithTags("Eventos");


app.MapGet("/eventos/{eventoId}", (int idEvento, IEventoRepository repo) =>
{
    var e = repo.GetById(idEvento);
    return e is null ? Results.NotFound() : Results.Ok(e);
}).WithTags("Eventos");

app.MapPut("/eventos/{eventoId}", (int idEvento, EventoUpdateDTO dto, IEventoRepository repo) =>
{
    var ok = repo.Update(idEvento, dto);
    return ok ? Results.NoContent() : Results.NotFound();
}).WithTags("Eventos");

app.MapPost("/eventos/{eventoId}/publicar", (int idEvento, IEventoRepository repo) =>
{
    try
    {
        repo.Publicar(idEvento);
        return Results.Ok(new { mensaje = "Evento publicado correctamente." });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithTags("Eventos");


app.MapPost("/eventos/{eventoId}/cancelar", (int idEvento, IEventoRepository repo) =>
{
    var ok = repo.Cancelar(idEvento);

    if (!ok)
        return Results.BadRequest(new { error = "El evento ya está cancelado o no existe." });

    return Results.Ok(new { mensaje = "Evento cancelado." });
}).WithTags("Eventos");



#endregion

#region FUNCIONES


app.MapPost("/api/funciones", (FuncionCreateDTO dto, IFuncionRepository repo) =>
{
    var f = new Funcion
    {
        idEvento = dto.idEvento,
        FechaHora = dto.Fecha,
        idLocal = dto.idLocal
    };
    repo.Add(f);
    return Results.Created($"/api/funciones/{f.idFuncion}", f);
}).WithTags("Funcion");
app.MapGet("/api/funciones", (IFuncionRepository repo) =>
{
    var funciones = repo.GetAll();
    return Results.Ok(funciones.Select(f => new FuncionDTO
    {
        idFuncion = f.idFuncion,
        idEvento = f.idEvento,
        Fecha = f.FechaHora,
        idLocal = f.idLocal
    }));
}).WithTags("Funcion");

app.MapGet("/funciones/{funcionId}", (int IdFuncion, IFuncionRepository repo) =>
{
    var f = repo.GetById(IdFuncion);
    return f is null ? Results.NotFound() : Results.Ok(f);
}).WithTags("Funcion");

app.MapPut("/funciones/{idFuncion}", (int idFuncion, FuncionUpdateDTO dto, IFuncionRepository repo) =>
{
    bool ok = repo.Update(idFuncion, dto);

    if (!ok)
        return Results.NotFound(new { mensaje = "Función no encontrada." });

    var funcionActualizada = repo.GetById(idFuncion);

    return Results.Ok(funcionActualizada);
}).WithTags("Funcion");


app.MapPost("/funciones/{funcionId}/cancelar", (int funcionId, IFuncionRepository repo) =>
{
    var funcion = repo.GetById(funcionId);
    if (funcion is null)
        return Results.NotFound(new { mensaje = "La función no existe." });

    if (funcion.Activo == 0)
        return Results.BadRequest(new { mensaje = "La función ya está cancelada." });

    bool ok = repo.Cancelar(funcionId);
    if (!ok)
        return Results.BadRequest(new { mensaje = "No se pudo cancelar la función." });

    return Results.Ok(new { mensaje = "Función cancelada correctamente." });
}).WithTags("Funcion");

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
    local.idLocal = id; // <-- IMPORTANTE

    return Results.Created($"/api/locales/{id}", local);
})
.WithTags("Local");

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
})
.WithTags("Local");


app.MapGet("/api/locales/{localId}", (int localId, ILocalRepository repo) =>
{
    var l = repo.GetById(localId); // <-- NOMBRE CONSISTENTE
    return l is null ? Results.NotFound() : Results.Ok(l);
})
.WithTags("Local");

app.MapPut("/api/locales/{localId}", (int localId, Local local, ILocalRepository repo) =>
{
    var existente = repo.GetById(localId);
    if (existente == null)
        return Results.NotFound("Local no encontrado.");

    local.idLocal = localId;

    repo.Update(local);

    return Results.Ok(local);
})
.WithTags("Local");

app.MapDelete("/api/locales/{localId}", (int localId, ILocalRepository repo) =>
{
    var ok = repo.Delete(localId);
    return ok ? Results.NoContent() : Results.BadRequest("No se puede eliminar (tiene funciones vigentes)");
})
.WithTags("Local");

#endregion

#region SECTOR

app.MapPost("/locales/{localId}/sectores", (int idLocal, SectorCreateDTO dto, ISectorRepository repo) =>
{
    var sector = new Sector
    {
        Nombre = dto.Nombre,
        idLocal = idLocal,
        Capacidad = dto.Capacidad,
        Precio = dto.Precio
    };

    repo.Add(idLocal, sector);

    return Results.Created($"/sectores/{sector.idSector}", sector);
}).WithTags("Sector");


app.MapGet("/locales/{localId}/sectores", (int idLocal, ISectorRepository repo) =>
{
    var sectores = repo.GetByLocal(idLocal);

    return Results.Ok(sectores.Select(s => new SectorDTO
    {
        idSector = s.idSector,
        Nombre = s.Nombre,
        idLocal = s.idLocal,
        Capacidad = s.Capacidad,
        Precio = s.Precio
    }));
}).WithTags("Sector");

app.MapPut("/sectores/{sectorId}", (int idSector, SectorUpdateDTO dto, ISectorRepository repo) =>
{
    bool actualizado = repo.Update(idSector, dto);

    if (!actualizado)
        return Results.NotFound("Sector no encontrado");

    var sector = repo.GetById(idSector);
    return Results.Ok(sector);
}).WithTags("Sector");

app.MapDelete("/sectores/{sectorId}", (int idSector, ISectorRepository repo) =>
{
    bool borrado = repo.Delete(idSector);

    return borrado
        ? Results.NoContent()
        : Results.BadRequest("Sector con tarifas/funciones asociadas");
}).WithTags("Sector");

#endregion

#region TARIFAS
app.MapPost("/api/tarifas", (TarifaCreateDTO dto, ITarifaRepository repo) =>
{
    var t = new Tarifa
    {
        Precio = dto.Precio,
        Descripcion = dto.Nombre,
        idFuncion = dto.idFuncion,
        idSector = dto.idSector,
        idEvento = dto.idEvento,
        Stock = dto.Stock,
        Activo = true
    };

    repo.Add(t);
    return Results.Created($"/api/tarifas/{t.idTarifa}", t);
})
.WithTags("Tarifas");

app.MapGet("/api/funcion/{idFuncion}/tarifas", (int idFuncion, ITarifaRepository repo) =>
{
    var tarifas = repo.GetByFuncionId(idFuncion);
    return Results.Ok(tarifas.Select(t => new TarifaDTO
    {
        idTarifa = t.idTarifa,
        Precio = t.Precio,
        idFuncion = t.idFuncion,
        Stock = t.Stock 
    }));
}).WithTags("Tarifas");


app.MapPut("/tarifa/{idTarifa}", (int idTarifa, TarifaUpdateDTO dto, ITarifaRepository repo) =>
{
    var ok = repo.Update(idTarifa, dto);
    return ok ? Results.NoContent() : Results.NotFound();
}).WithTags("Tarifas");

app.MapGet("/tarifa/{idTarifa}", (int idTarifa, ITarifaRepository repo) =>
{
    var t = repo.GetById(idTarifa);
    return t is null ? Results.NotFound() : Results.Ok(t);
}).WithTags("Tarifas");
#endregion

#region USUARIO
app.MapPost("/auth/register", (UsuarioRegisterDTO dto, AuthService auth) =>
{
    var resultado = auth.Register(dto);
    return Results.Ok(resultado);
}).WithTags("Usuario");

app.MapPost("/auth/login", (UsuarioLoginDTO dto, AuthService auth) =>
{
    var resultado = auth.Login(dto);
    return Results.Ok(resultado);
})
.WithTags("Usuario");

app.MapPost("/auth/refresh", (RefreshDTO dto, AuthService auth) =>
{
    var resultado = auth.Refresh(dto);
    return Results.Ok(resultado);
}).WithTags("Usuario");

app.MapPost("/auth/logout", (RefreshDTO dto, AuthService auth) =>
{
    var resultado = auth.Logout(dto);
    return Results.Ok(resultado);
}).WithTags("Usuario");

app.MapGet("/auth/me", (ClaimsPrincipal user) =>
{
    if (!user.Identity!.IsAuthenticated)
        return Results.Unauthorized();

    return Results.Ok(new
    {
        success = true,
        message = "Estás autenticado",
        usuario = user.Identity.Name
    });
})
.RequireAuthorization()
.WithTags("Usuario");

app.MapGet("/auth/roles", () =>
{
    var roles = new[] { "Administrador", "Cliente", "Empleado" };
    return Results.Ok(roles);
})
.RequireAuthorization()
.WithTags("Usuario");


app.MapPost("/usuarios/{id}/roles", (int IdUsuario, string rol, AuthService auth) =>
{
    var resultado = auth.AsignarRol(IdUsuario, rol);
    return Results.Ok(resultado);
}).WithTags("Usuario");
#endregion

#region QR
app.MapGet("/entradas/{idEntrada}/qr",
(int idEntrada, IQrService qrService, IEntradaRepository repo, IConfiguration config) =>
{
    var entrada = repo.GetById(idEntrada);
    if (entrada == null)
        return Results.NotFound("Entrada no existe");

    string qrContent = $"{entrada.IdEntrada}|{entrada.idFuncion}|{config["Qr:Key"]}";
    var qrBytes = qrService.GenerarQr(qrContent);

    return Results.File(qrBytes, "image/png");
})
.WithTags("QR");

app.MapPost("/qr/validar",
(string qrContent, IQrService qrService) =>
{
    var resultado = qrService.ValidarQr(qrContent);

    return Results.Ok(resultado);
})
.WithTags("QR");


#endregion

app.Run();


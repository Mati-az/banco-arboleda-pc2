using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BancoArboleda.Data;

var builder = WebApplication.CreateBuilder(args);

// ── Servicios ────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddSingleton<DataStore>(); // una sola instancia en memoria

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "banco_arboleda_secret_key_2026_seguro";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        opt.Events = new JwtBearerEvents
        {
            OnChallenge = ctx =>
            {
                ctx.HandleResponse();
                ctx.Response.StatusCode = 401;
                ctx.Response.ContentType = "application/json";
                return ctx.Response.WriteAsync("{\"success\":false,\"message\":\"Acceso no autorizado. Inicie sesión.\"}");
            }
        };
    });

builder.Services.AddAuthorization();

// ── Puerto ───────────────────────────────────────────────────────────────────
builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

// ── Middleware ────────────────────────────────────────────────────────────────
app.UseDefaultFiles();       // sirve wwwroot/index.html automáticamente
app.UseStaticFiles();        // sirve CSS, JS, etc. desde wwwroot/

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Cualquier ruta no-API devuelve index.html (SPA fallback)
app.MapFallbackToFile("index.html");

Console.WriteLine("=== Banco Arboleda ===");
Console.WriteLine("Servidor: http://localhost:5000");
Console.WriteLine("Usuario de prueba: cliente01 / clave123");
Console.WriteLine("Clave online:      online456");
Console.WriteLine("======================");

app.Run();

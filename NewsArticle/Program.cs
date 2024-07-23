using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NewsArticle.Models;
using NewsArticle.Servicios;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Registrar el servicio antes de construir la aplicación
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();
builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddTransient<IRepositorioPalabrasClave, RepositorioPalabrasClave>();
builder.Services.AddTransient<IRepositorioPais, RepositorioPais>();
builder.Services.AddTransient<IRepositorioEconomiaIlicita, RepositorioEconomiaIlicita>();
builder.Services.AddTransient<IRepositorioSectorEconomico, RepositorioSectorEconomico>();
builder.Services.AddTransient<IRepositorioTransporte, RepositorioTransporte>();
builder.Services.AddTransient<IRepositorioTipoViolencia, RepositorioTipoViolencia>();
builder.Services.AddTransient<IRepositorioIncautacion, RepositorioIncautacion>();
builder.Services.AddTransient<IRepositorioVehiculo, RepositorioVehiculo>();
builder.Services.AddTransient<IRepositorioMatasArbustos, RepositorioMatasArbustos>();
builder.Services.AddTransient<IRepositorioDroga, RepositorioDroga>();
builder.Services.AddTransient<IRepositorioNotaPeriodistica, RepositorioNotaPeriodistica>();
builder.Services.AddTransient<IUserStore<Usuario>, UsuarioStore>();
builder.Services.AddTransient<SignInManager<Usuario>>();

// Configurar Identity
builder.Services.AddIdentityCore<Usuario>(opciones =>
{
    opciones.Password.RequireDigit = false;
    opciones.Password.RequireLowercase = false;
    opciones.Password.RequireUppercase = false;
    opciones.Password.RequireNonAlphanumeric = false;
}).AddErrorDescriber<MensajeDeErrorIdentity>();

// Configurar autenticación y cookies
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme, opciones =>
{
    opciones.LoginPath = "/usuarios/login";
});

// Registrar GeoLocationService
builder.Services.AddSingleton<GeoLocationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Asegúrate de tener esto antes de UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=NoticiaPeriodistica}/{action=Index}/{id?}");

app.Run();

using CapaDatos;
using CapaNegocio.Servicios;
//AUTENTICACION METHODS -----------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
//AUTENTICACION METHODS -----------------------------------------------------------------------------------------

var builder = WebApplication.CreateBuilder(args);

// Configurar cadena de conexión
Conexion.SetCadenaConexion(
	builder.Configuration.GetConnectionString("cadenaSQL")!
);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 🔵 Registrar PayPalService (AQUÍ)
builder.Services.AddScoped<PayPalService>();

//AUTENTICACION METHODS -----------------------------------------------------------------------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
	{
		options.LoginPath = "/Acceso/Index";
		options.AccessDeniedPath = "/Acceso/Index";
		options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
	});

builder.Services.AddAuthorization();
//AUTENTICACION METHODS -----------------------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tienda}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

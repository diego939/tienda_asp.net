using CapaDatos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
//AUTENTICACION METHODS -----------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Authentication.Cookies;
//AUTENTICACION METHODS -----------------------------------------------------------------------------------------

var builder = WebApplication.CreateBuilder(args);

// Configurar cadena de conexión
Conexion.SetCadenaConexion(
	builder.Configuration.GetConnectionString("cadenaSQL")!
);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

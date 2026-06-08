using Microsoft.EntityFrameworkCore;
using PractiseDN.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles(); 

//app.UseStaticFiles() enables the Static File Middleware in ASP.NET Core.
//It allows files stored in the wwwroot folder, such as CSS, JavaScript, images, and fonts, to be served directly to the client.
//Without it, requests for these files return 404 errors

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
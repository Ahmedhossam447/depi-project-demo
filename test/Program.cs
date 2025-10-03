using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Helpers;

namespace test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- 1. Add services to the container ---

            builder.Services.AddDbContext<DepiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("connection")));
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddScoped<test.Services.PhotoServices>();
            builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyCookieAuth";
        // 2. Set the login path
        options.LoginPath = "/Account/Login";
    });


            // Use AddControllersWithViews for MVC applications that use Views.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();


            var app = builder.Build();

            // --- 2. Configure the HTTP request pipeline (Order is very important here) ---

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // This enables serving static files like CSS and JavaScript from the wwwroot folder.
            app.UseStaticFiles();

            // This sets up routing.
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // This is where you define your application's routes or "endpoints".
            // It should come at the end.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
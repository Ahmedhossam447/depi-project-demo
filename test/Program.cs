using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Helpers;
using test.Interfaces;
using test.Repository;
using test.Services;


namespace test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- 1. Add services to the container ---

            builder.Services.AddDbContext<DepiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("connection")));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
            option.SignIn.RequireConfirmedEmail = true)
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<DepiContext>();
                
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddScoped<test.Services.PhotoServices>();
            builder.Services.AddScoped<IAnimal, AnimalRepository>();
            builder.Services.AddScoped<IAccounts, AccountRepository>();
            builder.Services.AddScoped<IRequests, RequestRepository>();
            builder.Services.AddScoped<IEmailSender,EmailSenderServcies>();
            builder.Services.AddScoped<IShelter, ShelterRepository>();
            builder.Services.Configure<SendGridOptions>(
    builder.Configuration.GetSection("SendGrid")
);
            builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyCookieAuth";
        // 2. Set the login path
        options.LoginPath = "/Account/Login";
    });
            var googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
            builder.Services.AddAuthentication().AddGoogle(Services =>
            {
                Services.ClientId = googleAuthNSection["ClientId"] ;
                Services.ClientSecret = googleAuthNSection["ClientSecret"];
                Services.CallbackPath = "/signin-google";
            });

            // Use AddControllersWithViews for MVC applications that use Views.
            builder.Services.AddControllersWithViews();


            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                // Pass the IServiceProvider to your seeder
                await RoleServices.SeedRolesAsync(scope.ServiceProvider);
            }

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

            app.Run();

        }
    }
}
using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services;
using HelloJobBackEnd.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace HelloJobBackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddScoped<LayoutService>();
            builder.Services.AddScoped<IVacansService, VacansService>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<IBusinessTitleService, BusinessTitleService>();
            builder.Services.AddScoped<ILikedService, LikedService>();
            builder.Services.AddScoped<ICvPageService, CvPageService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<UserService>();


            builder.Services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "866366361059-m9c9icfu0v1c8cth4prdedkfp2osa9cp.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });

            builder.Services.AddAuthentication()
            .AddFacebook(options =>
             {
                 options.AppId = "203567782610976";
                 options.AppSecret = "9537ea0fa0ab56e49e29f4a9385dce52";
             });
            builder.Services.AddDbContext<HelloJobDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });


            builder.Services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.SignIn.RequireConfirmedEmail = true;
                opt.Password.RequiredUniqueChars = 1;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 5;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = false;


                opt.User.RequireUniqueEmail = false;

                opt.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnm_-1234567890.QWERTYUIOPASDFGHJKLZXCVBNM:)( ";

                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<HelloJobDbContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=login}/{search?}"
                );
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{search?}");
            });


            app.Run();

        }
    }
}
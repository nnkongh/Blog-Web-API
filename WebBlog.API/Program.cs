using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebBlog.API.DatabaseConnection;
using WebBlog.API.Interface;
using WebBlog.API.Models;
using WebBlog.API.Models.Cloudinary;
using WebBlog.API.Repo;
using WebBlog.API.Repository;
using WebBlog.API.Services;

namespace WebBlog.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
            });
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    ValidateAudience = true,

                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidateIssuer = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),

                    ValidateLifetime = true

                };
            });
            builder.Services.AddIdentityCore<AppUser>(x =>
            {
                x.Password.RequireLowercase = true;
                x.Password.RequireUppercase = true;
                x.Password.RequiredLength = 8;
            })

                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(2);

            });
            builder.Services.AddAuthorization();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();

            var email = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            builder.Services.AddSingleton(email);

            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();

                    });
            });

            var app = builder.Build();

            using (var scoped = app.Services.CreateScope())
            {
                var services = scoped.ServiceProvider;
                await RoleService.CreateRoles(services);
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowReactApp");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}

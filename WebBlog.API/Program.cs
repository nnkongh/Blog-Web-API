using Microsoft.EntityFrameworkCore;
using WebBlog.API.DatabaseConnection;
using WebBlog.API.Interface;
using WebBlog.API.Repo;

namespace WebBlog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<BlogDatabase>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
            });

            builder.Services.AddScoped<IBlogRepository, BlogRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

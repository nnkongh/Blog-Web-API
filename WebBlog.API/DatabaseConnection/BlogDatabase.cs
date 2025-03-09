using Microsoft.EntityFrameworkCore;
using WebBlog.API.Models;

namespace WebBlog.API.DatabaseConnection
{
    public class BlogDatabase : DbContext
    {
       public BlogDatabase(DbContextOptions<BlogDatabase> options) : base(options)
        {
        }
        public DbSet<Blog> Blogs { get; set; }
    }
}

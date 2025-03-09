using Microsoft.EntityFrameworkCore;
using WebBlog.API.DatabaseConnection;
using WebBlog.API.Interface;
using WebBlog.API.Models;

namespace WebBlog.API.Repo
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDatabase _db;  

        public BlogRepository(BlogDatabase db) {
            _db = db;
        }
        public async Task<Blog> CreateAsync(Blog blog)
        {
            await _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();
            return blog;
        }

        public async Task<Blog?> DeleteAsync(int id)
        {
            var item = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return null;
            _db.Blogs.Remove(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Blog>> GetAsync()
        {
            var items = await _db.Blogs.ToListAsync();
            return items;
        }

        public async Task<Blog?> GetByIdAsync(int id)
        {
            var item = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return null;
            return item;
        }

        public async Task<Blog?> UpdateAsync(int id, Blog blog)
        {
            var item = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return null;
            item.Title = blog.Title;
            item.Content = blog.Content;
            item.Tags = blog.Tags;
            await _db.SaveChangesAsync();
            return item;
        }
    }
}

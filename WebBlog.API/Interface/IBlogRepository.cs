using WebBlog.API.Models;

namespace WebBlog.API.Interface
{
    public interface IBlogRepository
    {
        Task<IEnumerable<Blog>> GetAsync();
        Task<Blog?> GetByIdAsync(int id);
        Task<Blog> CreateAsync(Blog blog);
        Task<Blog?> DeleteAsync(int id);
        Task<Blog?> UpdateAsync(int id, Blog blog);
        Task<IQueryable<Blog>> GetBlogAsQuery();
    }
}
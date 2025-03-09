using Microsoft.AspNetCore.Mvc;
using WebBlog.API.Interface;
using WebBlog.API.Models;

namespace WebBlog.API.Controllers
{
    [ApiController]
    [Route("api/blog")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _repo;

        public BlogController(IBlogRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await _repo.GetAsync();
        }
        [HttpGet("{id}")]
        public async Task<Blog?> GetBlog(int id)
        {
            var blog = await _repo.GetByIdAsync(id);
            if (blog == null)
            {
                return null;
            }
            return blog;
        }
        [HttpPost]
        public async Task<Blog> CreateBlog(Blog blog)
        {
            return await _repo.CreateAsync(blog);
        }
        [HttpPut("{id}")]
        public async Task<Blog?> UpdateBlog(int id, Blog blog)
        {
            var item = await _repo.UpdateAsync(id, blog);
            if (item == null)
            {
                return null;
            }
            return item;
        }
        [HttpDelete("{id}")]
        public async Task<Blog?> DeleteBlog(int id)
        {
            var item = await _repo.DeleteAsync(id);
            if (item == null)
            {
                return null;
            }
            return item;
        }
    }
}

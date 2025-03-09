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
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            var items = await _repo.GetAsync();
            return Ok(items);
        }
        [HttpGet("{id:int}")] // Route constraint
        public async Task<ActionResult<Blog?>> GetBlog(int id)
        {
            var blog = await _repo.GetByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }
        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlog(Blog blog)
        {
            var item = await _repo.CreateAsync(blog);
            return CreatedAtAction(nameof(GetBlog), new { id = item.Id }, item);
        }
        [HttpPut("{id:int}")]
        public async Task<Blog?> UpdateBlog(int id, Blog blog)
        {
            var item = await _repo.UpdateAsync(id, blog);
            if (item == null)
            {
                return null;
            }
            return item;
        }
        [HttpDelete("{id:int}")] // Route constraint    ??Custom Route Regex??
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

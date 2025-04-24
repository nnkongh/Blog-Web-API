using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBlog.API.Interface;
using WebBlog.API.Models;
using WebBlog.API.Models.Pagination;
using WebBlog.API.ViewModel;

namespace WebBlog.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    [Route("api/blog")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _repo;
        private readonly IPhotoService _photo;

        public BlogController(IBlogRepository repo, IPhotoService photo)
        {
            _repo = repo;
            _photo = photo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs(Pagination<Blog> blog)
        {
            var blogs = await _repo.GetBlogAsQuery();
            var items = await Pagination<Blog>.CreateAsync(blogs,blog.PageNumber,blog.Size);
            return Ok(items);
        }
        [HttpGet("{id:int}")] // Route constraint
        // Custom Route Regex
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
        [Route("createBlog")]
        public async Task<ActionResult<Blog>> CreateBlog([FromForm]BlogViewModel blog)
        {
            string imageUrl = string.Empty;
            if (blog.Image != null)
            {
                var photo = await _photo.AddPhotoAsync(blog.Image);
                imageUrl = photo.Url.ToString();
            }
            var Blog = new Blog
            {
                Title = blog.Title,
                Content = blog.Content,
                Tags = blog.Tags,
                Image = imageUrl,
            };
            
            var item = await _repo.CreateAsync(Blog);
            return CreatedAtAction(nameof(GetBlog), new { id = item.Id }, item);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Blog?>> UpdateBlog(int id, Blog blog)
        {
            var item = await _repo.UpdateAsync(id, blog);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        [HttpDelete("{id:int}")] // Route constraint    ??Custom Route Regex??
        public async Task<ActionResult<Blog?>> DeleteBlog(int id)
        {
            var item = await _repo.DeleteAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebBlog.API.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public string Tags { get; set; } = string.Empty;
        public string? Iamge { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

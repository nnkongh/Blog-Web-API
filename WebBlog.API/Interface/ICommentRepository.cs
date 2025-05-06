using WebBlog.API.Models;

namespace WebBlog.API.Interface
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment cmt);
        Task<Comment?> UpdateAsync(Comment cmt, int id);
        Task<Comment?> DeleteAsync(int id);
     }
}

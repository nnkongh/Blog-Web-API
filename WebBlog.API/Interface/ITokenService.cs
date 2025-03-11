using WebBlog.API.Models;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Interface
{
    public interface ITokenService
    {
        public Task<string> CreateToken(AppUser model);
    }
}

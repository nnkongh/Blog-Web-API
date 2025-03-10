namespace WebBlog.API.Interface
{
    public interface ITokenService
    {
        public string CreateToken(string email);
    }
}

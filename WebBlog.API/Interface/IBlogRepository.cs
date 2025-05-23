﻿using WebBlog.API.Helper;
using WebBlog.API.Models;

namespace WebBlog.API.Interface
{
    public interface IBlogRepository
    {
        Task<Blog?> GetByIdAsync(int id);
        Task<Blog> CreateAsync(Blog blog);
        Task<Blog?> DeleteAsync(int id);
        Task<Blog?> UpdateAsync(int id, Blog blog);
        Task<IEnumerable<Blog>> GetBlogs(QueryObject querry);
        Task<bool> IfExists(int id);

    }
}
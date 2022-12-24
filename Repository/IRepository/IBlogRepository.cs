using DotNet_API_Example.Models;
using System.Linq.Expressions;

namespace DotNet_API_Example.Repository.IRepository
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Task<Blog> UpdateAsync(Blog entity);
    }
}

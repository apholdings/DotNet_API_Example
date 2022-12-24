using DotNet_API_Example.Models;
using System.Linq.Expressions;

namespace DotNet_API_Example.Repository.IRepository
{
    public interface IBlogNumberRepository : IRepository<BlogNumber>
    {
        Task<BlogNumber> UpdateAsync(BlogNumber entity);
    }
}

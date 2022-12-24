using DotNet_API_Example.Data;
using DotNet_API_Example.Models;
using DotNet_API_Example.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotNet_API_Example.Repository
{
    public class BlogNumberRepository : Repository<BlogNumber>, IBlogNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public BlogNumberRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public async Task<BlogNumber> UpdateAsync(BlogNumber entity)
        {
            entity.UpdatedDate= DateTime.Now;
            _db.BlogNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}

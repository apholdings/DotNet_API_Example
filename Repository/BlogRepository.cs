using DotNet_API_Example.Data;
using DotNet_API_Example.Models;
using DotNet_API_Example.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotNet_API_Example.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly ApplicationDbContext _db;
        public BlogRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public async Task<Blog> UpdateAsync(Blog entity)
        {
            entity.UpdatedDate= DateTime.Now;
            _db.Blogs.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}

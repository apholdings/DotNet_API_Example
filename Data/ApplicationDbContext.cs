using DotNet_API_Example.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNet_API_Example.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            //AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }


        public DbSet<Blog> Blogs { get; set; } 
        public DbSet<BlogNumber> BlogNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Blog>().HasData(
                new Blog()
                {
                    Id = 1,
                    Title = "Sexo con anto",
                    Description = "La vez que tuve sex con a",
                    Content = "Tuve sexo con antonella en el 2023",
                    Thumbnail = "https://boomslag.s3.us-east-2.amazonaws.com/lightbulb.jpg",
                    Rate = 200,
                    CreatedDate= DateTime.UtcNow
                },

                new Blog()
                {
                    Id = 2,
                    Title = "Titties",
                    Description = "Las tetas de antonella, YUMMY",
                    Content = "Antos titties",
                    Thumbnail = "https://boomslag.s3.us-east-2.amazonaws.com/lightbulb.jpg",
                    Rate = 200,
                    CreatedDate= DateTime.UtcNow
                }
                );
        }
    }
}

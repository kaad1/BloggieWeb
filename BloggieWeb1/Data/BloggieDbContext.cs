using BloggieWeb1.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BloggieWeb1.Data
{
    public class BloggieDbContext : DbContext
    {
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options)
        {

        }
        
       public DbSet<Tag> Tags { get; set; }
       public  DbSet<BlogPost> BlogPosts { get; set; }
    }
}

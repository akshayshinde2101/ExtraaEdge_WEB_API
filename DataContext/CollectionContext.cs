using ExtraaEdge_WEB_API.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExtraaEdge_WEB_API.DataContext
{
    public class CollectionContext : DbContext
    {
        public CollectionContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Sale> sales { get; set; }
        public DbSet<Mobile> mobiles { get; set; }
        public DbSet<Brand> brands { get; set; }

        public DbSet<Customer> customers { get; set; }
        public DbSet<LoginModel> login{get;set;}
      
    }
}

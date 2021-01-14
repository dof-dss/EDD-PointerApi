using Microsoft.EntityFrameworkCore;
using PointerApi.Models;

namespace PointerApi.Data
{
    public class PointerContext : DbContext
    {
        public PointerContext(DbContextOptions<PointerContext> options) : base(options)
        {
        }

        public DbSet<PointerModel> Pointer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PointerModel>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
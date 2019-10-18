using Microsoft.EntityFrameworkCore;

namespace FinancialGains
{
    public class FinancialDataContext : DbContext
    {
        public DbSet<Budget> Budget { get; set; }
        public DbSet<BudgetCategory> BudgetCategory { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=onsager.database.windows.net;Database=TestDatabase;User Id=keo;Password=[mypassword];");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.TimeStamp).IsRequired();
                entity.Property(e => e.Description).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.CategoryFk).IsRequired();
                entity.HasOne(e => e.Category).WithOne().HasForeignKey<Category>(c => c.Id);
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Amount).IsRequired();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<BudgetCategory>(entity =>
            {
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.BudgetFk).IsRequired();
                entity.Property(e => e.CategoryFk).IsRequired();
            });
        }
    }
}

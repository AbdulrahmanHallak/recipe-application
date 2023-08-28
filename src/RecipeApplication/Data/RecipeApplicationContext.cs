using Microsoft.EntityFrameworkCore;
using RecipeApplication.Models;

namespace RecipeApplication.Data
{
    public class RecipeApplicationContext : DbContext
    {
        public RecipeApplicationContext (DbContextOptions<RecipeApplicationContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>(s =>
            {
                s.Property(x => x.Name).HasMaxLength(60);
                s.Property(x => x.TimeToCook).HasColumnType("time");
                s.Property(x => x.Method).HasMaxLength(300);
                s.ToTable("Recipe");
            });

            modelBuilder.Entity<Ingredient>(s =>
            {
                s.Property(x => x.Name).HasMaxLength(60);
                s.Property(x => x.Quantity).HasColumnType("decimal(6, 2)");
                s.Property(x => x.Unit).HasMaxLength(60);
                s.ToTable("Ingredient");
            });
        }
        public DbSet<RecipeApplication.Models.Ingredient> Ingredient { get; set; } = default!;
        public DbSet<RecipeApplication.Models.Recipe> Recipe { get; set; } = default!;

    }
}

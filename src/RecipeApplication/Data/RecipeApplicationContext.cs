using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeApplication.Models;

namespace RecipeApplication.Data
{
    public class RecipeApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public RecipeApplicationContext (DbContextOptions<RecipeApplicationContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Recipe>(s =>
            {
                s.Property(x => x.Name).HasMaxLength(60);
                s.Property(x => x.TimeToCook).HasColumnType("time");
                s.Property(x => x.Method).HasMaxLength(300);
                s.ToTable("Recipe");
                s.HasQueryFilter(s => !s.IsDeleted);
            });

            modelBuilder.Entity<Ingredient>(s =>
            {
                s.Property(x => x.Name).HasMaxLength(60);
                s.Property(x => x.Quantity).HasColumnType("decimal(6, 2)");
                s.Property(x => x.Unit).HasMaxLength(60);
                s.ToTable("Ingredient");
            });
        }
        public DbSet<Ingredient> Ingredient { get; set; } = default!;
        public DbSet<Recipe> Recipe { get; set; } = default!;

    }
}

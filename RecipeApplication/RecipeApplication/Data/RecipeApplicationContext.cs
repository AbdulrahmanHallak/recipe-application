using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                s.Property(x => x.Name).HasMaxLength(20);
                s.Property(x => x.TimeToCook).HasColumnType("time");
                s.Property(x => x.Method).HasMaxLength(150);
                s.ToTable("Recipes");
            });

            modelBuilder.Entity<Ingredient>(s =>
            {
                s.Property(x => x.Name).HasMaxLength(20);
                s.Property(x => x.Quantity).HasColumnType("decimal(3, 2)");
                s.ToTable("Ingredients");
            });
        }
        public DbSet<RecipeApplication.Models.Ingredient> Ingredient { get; set; } = default!;
        public DbSet<RecipeApplication.Models.Recipe> Recipe { get; set; } = default!;
    }
}

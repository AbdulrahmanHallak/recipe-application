using Microsoft.EntityFrameworkCore;

namespace RecipeApplication.Data;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new RecipeApplicationContext(serviceProvider.GetRequiredService<DbContextOptions<RecipeApplicationContext>>());
        if (context == null || context.Recipe == null)
            throw new ArgumentNullException("Null RazorPagesMovieContext");
        if (context.Recipe.Any())
            return;
        context.Recipe.AddRange(
            new Recipe
            {
                Name = "Spaghetti Bolognese",
                TimeToCook = TimeSpan.FromMinutes(30),
                Method = "1. Boil water and cook spaghetti. 2. Brown the minced meat and onions. 3. Add tomatoes, tomato paste, and seasonings. Simmer for 20 minutes. 4. Serve the Bolognese sauce over cooked spaghetti.",
                LastModified = DateTimeOffset.UtcNow,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Name = "Spaghetti", Quantity = 250, Unit = "grams" },
                    new Ingredient { Name = "Minced Meat", Quantity = 500, Unit = "grams" },
                    new Ingredient { Name = "Onion", Quantity = 1, Unit = "piece" },
                    new Ingredient { Name = "Tomatoes", Quantity = 4, Unit = "pieces" },
                    new Ingredient { Name = "Tomato Paste", Quantity = 2, Unit = "tablespoons" },
                    new Ingredient { Name = "Salt", Quantity = 1, Unit = "teaspoon" },
                    new Ingredient { Name = "Pepper", Quantity = 0.5m, Unit = "teaspoon" },
                    new Ingredient { Name = "Oregano", Quantity = 1, Unit = "teaspoon" }
                }
            });
        context.SaveChanges();
    }
}

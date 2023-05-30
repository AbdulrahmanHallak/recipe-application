using Microsoft.EntityFrameworkCore;
using RecipeApplication.Models.ViewModels;

namespace RecipeApplication;

public class RecipeService
{
    private readonly Data.RecipeApplicationContext _context;
    public RecipeService(Data.RecipeApplicationContext context)
    {
        _context = context;
    }
    // Get recipes to view in index.
    public async Task<List<RecipeSummaryVM>> GetRecipes()
    {
        return await _context.Recipe
            .Where(s => !s.IsDeleted)
            .Select(s => new RecipeSummaryVM()
            {
                Id = s.Id,
                Name = s.Name,
                TimeToCook = $"{s.TimeToCook.Hours}hrs and {s.TimeToCook.Minutes}m",
                NumberOfIngredients = s.Ingredients.Count(),

            }).ToListAsync();

    }
    public async Task<RecipeDetailsVM> GetRecipeDetails(int id)
    {
        var s = await _context.Recipe
            .Where(s => !s.IsDeleted)
            .Select(s => new RecipeDetailsVM()
            {
                Id = s.Id,
                Name = s.Name,
                Method = s.Method,
                TimeToCook = $"{s.TimeToCook.Hours}hrs and {s.TimeToCook.Minutes}minutes",
                Ingredients = s.Ingredients
            }).SingleOrDefaultAsync();
        return s!;
    }

}

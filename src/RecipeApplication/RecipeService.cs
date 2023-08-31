using Microsoft.EntityFrameworkCore;
using RecipeApplication.Models;

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
        var recipeSummary = await (
            from recipe in _context.Recipe
            select new RecipeSummaryVM()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                TimeToCook = $"{recipe.TimeToCook.Hours}hrs and {recipe.TimeToCook.Minutes}m",
                NumberOfIngredients = recipe.Ingredients.Count(),
            }
        ).ToListAsync();
        return recipeSummary;
    }

    public async Task<RecipeDetailsVM> GetRecipeDetails(int id)
    {
        var recipeDetails = await (
            from recipe in _context.Recipe
            where recipe.Id == id
            select new RecipeDetailsVM()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Method = recipe.Method,
                TimeToCook = $"{recipe.TimeToCook.Hours}hrs and {recipe.TimeToCook.Minutes}minutes",
                LastModified = recipe.LastModified,
                Ingredients = recipe.Ingredients
            }
        ).SingleOrDefaultAsync();

        return recipeDetails
            ?? throw new InvalidOperationException(
                "There is no entity that correspond to the provided ID"
            );
    }

    public async Task<EditRecipeVM> GetRecipeForUpdate(int id)
    {
        var recipeForUpdate = await (
            from recipe in _context.Recipe
            where recipe.Id == id
            select new EditRecipeVM()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Method = recipe.Method,
                TimeToCook = recipe.TimeToCook,
                Ingredients = new List<EditIngredientsVM>(
                    from ingredient in recipe.Ingredients
                    select new EditIngredientsVM()
                    {
                        Name = ingredient.Name,
                        Quantity = ingredient.Quantity,
                        Unit = ingredient.Unit,
                    }
                )
            }
        ).SingleOrDefaultAsync();

        return recipeForUpdate
            ?? throw new InvalidOperationException("There is no entity with this ID");
        ;
    }

    public async Task<int> CreateRecipe(EditRecipeVM recipeVM)
    {
        var recipe = new Recipe()
        {
            Name = recipeVM.Name,
            TimeToCook = recipeVM.TimeToCook,
            Method = recipeVM.Method,
            LastModified = DateTimeOffset.UtcNow,
            Ingredients = new List<Ingredient>(
                from ingredient in recipeVM.Ingredients
                select new Ingredient()
                {
                    Name = ingredient.Name,
                    Quantity = ingredient.Quantity,
                    Unit = ingredient.Unit,
                }
            )
        };
        _context.Add(recipe);
        await _context.SaveChangesAsync();
        return recipe.Id;
    }

    public async Task<int> UpdateRecipe(EditRecipeVM recipeVM)
    {
        var recipe = await (
            from recipee in _context.Recipe
            where recipee.IsDeleted == false && recipee.Id == recipeVM.Id
            select recipee
        )
            .Include("Ingredients")
            .SingleOrDefaultAsync();

        if (recipe is null || recipe.IsDeleted)
            throw new InvalidOperationException("There is no entity with this id");

        recipe.Name = recipeVM.Name;
        recipe.TimeToCook = recipeVM.TimeToCook;
        recipe.Method = recipeVM.Method;
        recipe.LastModified = DateTimeOffset.UtcNow;
        recipe.Ingredients = (
            from ingredient in recipeVM.Ingredients
            select new Ingredient()
            {
                Name = ingredient.Name,
                Quantity = ingredient.Quantity,
                Unit = ingredient.Unit,
            }
        ).ToList();

        await _context.SaveChangesAsync();
        return recipe.Id;
    }

    /// <summary>
    /// Marks an existing recipe as deleted
    /// </summary>
    /// <param name="recipeId"></param>
    public async Task DeleteRecipe(int recipeId)
    {
        var recipe =
            await _context.Recipe.FindAsync(recipeId)
            ?? throw new Exception("Unable to find recipe");
        recipe.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
}

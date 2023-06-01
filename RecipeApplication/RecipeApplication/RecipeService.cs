using Microsoft.EntityFrameworkCore;
using RecipeApplication.Models;
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
    public async Task<RecipeDetailsVM>? GetRecipeDetails(int id)
    {
        var s = await _context.Recipe
            .Where(s => !s.IsDeleted)
            .Where(s => s.Id == id)
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
    public async Task<EditRecipeVM> GetRecipeForUpdate(int id)
    {
        var recipe = await _context.Recipe
            .Where(s => !s.IsDeleted)
            .Where(s => s.Id == id)
            .Include(s => s.Ingredients)
            .SingleOrDefaultAsync();
        var ingredientsVM = new List<EditIngredientsVM>();
        foreach (var item in recipe.Ingredients)
        {
            ingredientsVM.Add(new EditIngredientsVM()
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                Unit = item.Unit,
            });
        }
        var recipeVM = new EditRecipeVM()
        {
            Name = recipe!.Name,
            Method = recipe.Method,
            TimeToCook = recipe.TimeToCook,
            Ingredients = ingredientsVM
        };
        return recipeVM;
    }
    public async Task<int> CreateRecipe(EditRecipeVM recipeVM)
    {
        //var recipe = MapToEntity(id, recipeVM);
        //to cast the ingredient collection in EditIngredientVM
        ICollection<Ingredient> ingredients = new List<Ingredient>();
        foreach (var item in recipeVM.Ingredients)
        {
            var ingredient = new Ingredient()
            {
                Name = item.Name,
                Quantity = item.Quantity,
                Unit = item.Unit,
            };
            ingredients.Add(ingredient);
        }
        var recipe = new Recipe()
        {
            Name = recipeVM.Name,
            Method = recipeVM.Method,
            TimeToCook = recipeVM.TimeToCook,
            Ingredients = ingredients
        };
        _context.Recipe.Add(recipe);
        await _context.SaveChangesAsync();
        return recipe.Id;
    }
    public async Task<int> UpdateRecipe(int id,EditRecipeVM recipeVM)
    {
        var recipe = MapToEntity(id,recipeVM);
        _context.Attach(recipe).State = EntityState.Modified;
        foreach (var item in recipe.Ingredients)
        {
            _context.Attach(item).State = EntityState.Modified;
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
        return recipe.Id;
    }
    /// <summary>
    /// Marks an existing recipe as deleted
    /// </summary>
    /// <param name="recipeId"></param>
    public async Task DeleteRecipe(int recipeId)
    {
        var recipe = await _context.Recipe.FindAsync(recipeId) ?? throw new Exception("Unable to find recipe");
        recipe.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    // Cast the EditeRecipeVM to a Recipe entity.
    private static Recipe MapToEntity(int id,EditRecipeVM recipeVM)
    {
        //to cast the ingredient collection in EditIngredientVM
        ICollection<Ingredient> ingredients = new List<Ingredient>();
        foreach (var item in recipeVM.Ingredients)
        {
            var ingredient = new Ingredient()
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                Unit = item.Unit,
            };
            ingredients.Add(ingredient);
        }
        var recipe = new Recipe()
        {
            Id =id,
            Name = recipeVM.Name,
            Method = recipeVM.Method,
            TimeToCook = recipeVM.TimeToCook,
            Ingredients = ingredients
        };
        return recipe;
    }
}


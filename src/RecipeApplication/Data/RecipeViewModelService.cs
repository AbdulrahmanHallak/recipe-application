using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using OneOf.Types;
using RecipeApplication.Interfaces;
using RecipeApplication.Models;

namespace RecipeApplication.Data;

public class RecipeViewModelService : IRecipeViewModelService
{
    private readonly RecipeApplicationContext _context;
    private readonly ILogger<RecipeApplicationContext> _logger;

    public RecipeViewModelService(RecipeApplicationContext context, ILogger<RecipeApplicationContext> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OneOf<List<RecipeSummaryVM>, None>> GetSummariesAsync()
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

        if (recipeSummary.IsNullOrEmpty())
            return new None();

        return recipeSummary;
    }

    public async Task<OneOf<RecipeDetailsVM, None>> GetDetailAsync(int id)
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

        if (recipeDetails is null)
        {
            _logger.LogWarning("A request to retrive a recipe with {RecipeId} which does not exist", id);
            return new None();
        }

        return recipeDetails;
    }

    public async Task<OneOf<EditRecipeVM, None>> GetForUpdateAsync(int id)
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

        if (recipeForUpdate is null)
        {
            _logger.LogWarning("A request to retrive a recipe with {RecipeId} which does not exist", id);
            return new None();
        }

        return recipeForUpdate;
    }

    public async Task<int> CreateAsync(EditRecipeVM recipeVM, string createdById)
    {
        var recipe = new Recipe()
        {
            Name = recipeVM.Name,
            TimeToCook = recipeVM.TimeToCook,
            Method = recipeVM.Method,
            LastModified = DateTimeOffset.UtcNow,
            CreatedById = createdById,
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

    public async Task<OneOf<int, None>> UpdateAsync(EditRecipeVM recipeVM)
    {
        var recipe = await (
            from recipee in _context.Recipe
            where recipee.IsDeleted == false && recipee.Id == recipeVM.Id
            select recipee
        )
            .Include(x => x.Ingredients)
            .SingleOrDefaultAsync();

        if (recipe is null)
            return new None();

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

    public async Task DeleteAsync(int id)
    {
        var recipe = await _context.Recipe.FindAsync(id);
        if (recipe is null)
        {
            _logger.LogWarning("A request to delete a recipe with {RecipeId} which does not exist", id);
          return;
        }


        recipe.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<Recipe> GetForAuthorizationAsync(int id)
    {
        var recipe = await _context.Recipe.Where(x => x.Id == id).SingleOrDefaultAsync();
        return recipe!;
    }

    public async Task<OneOf<List<UserRecipesVM>, None>> GetForUserAsync(string userId, int numOfRecipes)
    {
        var recipes = await
            (from r in _context.Recipe
             where r.CreatedById == userId
             select new UserRecipesVM()
             {
                 Id = r.Id,
                 Name = r.Name
             }).Take(numOfRecipes).ToListAsync();

        if (recipes.IsNullOrEmpty())
            return new None();

        return recipes;
    }
}

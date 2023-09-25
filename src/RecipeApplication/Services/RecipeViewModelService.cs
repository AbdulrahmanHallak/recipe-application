using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using OneOf.Types;
using RecipeApplication.Data;
using RecipeApplication.Interfaces;
using RecipeApplication.Models;

namespace RecipeApplication;

public class RecipeViewModelService : IRecipeViewModelService
{
    private readonly RecipeApplicationContext _context;

    public RecipeViewModelService(Data.RecipeApplicationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a summary of each recipe to view on the index page.
    /// </summary>
    /// <returns>
    /// A list of <see cref="RecipeSummaryVM"/> containing recipe summaries if the database is not empty;
    /// otherwise, returns <see cref="None"/>.
    /// </returns>
    public async Task<OneOf<List<RecipeSummaryVM>, None>> GetRecipesSummaryAsync()
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
    /// <summary>
    /// Retrieves detailed information about a recipe by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve.</param>
    /// <returns>
    /// A <see cref="RecipeDetailsVM"/> object with the details of the recipe if found;
    /// otherwise, <see cref="None"/>.
    /// </returns>
    public async Task<OneOf<RecipeDetailsVM, None>> GetRecipeDetailsAsync(int id)
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
            return new None();

        return recipeDetails;
    }
    /// <summary>
    /// Retrieves a recipe for updating based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve for updating.</param>
    /// <returns>
    /// An
    /// <see cref="EditRecipeVM"/> object, which is used by the user to update the recipe if found;
    /// otherwise, <see cref="None"/>.
    /// </returns>
    public async Task<OneOf<EditRecipeVM, None>> GetRecipeForUpdateAsync(int id)
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
            return new None();

        return recipeForUpdate;
    }
    /// <summary>
    /// Creates a new recipe in the database based on the values provided in the <paramref name="recipeVM"/>.
    /// </summary>
    /// <param name="recipeVM">A <see cref="EditRecipeVM"/> object containing the data for the new recipe.</param>
    /// <param name="createdById">A GUID representing the user who created the recipe.</param>
    /// <returns>
    /// An <see cref="int"/> representing the unique Id of the newly created recipe.
    /// </returns>
    public async Task<int> CreateRecipeAsync(EditRecipeVM recipeVM, string createdById)
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
    /// <summary>
    /// Updates a recipe in the database if it exists based on the values provided in the <paramref name="recipeVM"/>.
    /// </summary>
    /// <param name="recipeVM">A <see cref="EditRecipeVM"/> object containing the updated data for the recipe.</param>
    /// <returns>
    /// An <see cref="int"/> representing the ID of the updated recipe.
    /// A <see cref="None"/> if the recipe does not exist.
    /// </returns>
    public async Task<OneOf<int, None>> UpdateRecipeAsync(EditRecipeVM recipeVM)
    {
        var recipe = await (
            from recipee in _context.Recipe
            where recipee.IsDeleted == false && recipee.Id == recipeVM.Id
            select recipee
        )
            .Include("Ingredients")
            .SingleOrDefaultAsync();

        if (recipe is null)
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
    /// Marks a recipe as deleted if it exists.
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteRecipeAsync(int id)
    {
        var recipe = await _context.Recipe.FindAsync(id);
        if (recipe is null) return;

        recipe.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
    public async Task<Recipe> GetRecipeAsync(int id)
    {
        var recipe = await _context.Recipe.Where(x => x.Id == id).SingleOrDefaultAsync();
        return recipe ?? throw new InvalidOperationException();
    }
}

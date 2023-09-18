using Microsoft.EntityFrameworkCore;
using RecipeApplication.Data;
using RecipeApplication.Models;

namespace RecipeApplication;

public class RecipeService
{
    private readonly RecipeApplicationContext _context;

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
    /// <summary>
    /// Retrieves detailed information about a recipe by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve.</param>
    /// <returns>
    /// A <see cref="RecipeDetailsVM"/> object with the details of the recipe if found.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no recipe entity corresponds to the provided ID.
    /// </exception>
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
    /// <summary>
    /// Retrieves a recipe for updating based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve for updating.</param>
    /// <returns>
    /// An
    /// <see cref="EditRecipeVM"/> object, which is used by the user to update the recipe.
    /// If no recipe with the provided ID is found, the method throws an <see cref="InvalidOperationException"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no recipe entity corresponds to the provided ID.
    /// </exception>
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
    /// <summary>
    /// Creates a new recipe in the database based on the values provided in the <paramref name="recipeVM"/>.
    /// </summary>
    /// <param name="recipeVM">A <see cref="EditRecipeVM"/> object containing the data for the new recipe.</param>
    /// <returns>
    /// An integer representing the unique ID of the newly created recipe.
    /// </returns>
    public async Task<int> CreateRecipe(EditRecipeVM recipeVM, string createdById)
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
    /// An integer representing the ID of the updated recipe.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no recipe entity corresponds to the ID provided in the <paramref name="recipeVM"/>.
    /// </exception>
    public async Task<int> UpdateRecipe(EditRecipeVM recipeVM)
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
    /// <param name="recipeId"></param>
    public async Task DeleteRecipe(int recipeId)
    {
        var recipe = await _context.Recipe.FindAsync(recipeId);
        if (recipe is null) return;

        recipe.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
}

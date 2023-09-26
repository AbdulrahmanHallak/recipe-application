using OneOf;
using OneOf.Types;
using RecipeApplication.Data;
using RecipeApplication.Models;

namespace RecipeApplication.Interfaces;
public interface IRecipeViewModelService
{
    /// <summary>
    /// Retrieves a summary of each recipe to view on the index page.
    /// </summary>
    /// <returns>
    /// A list of <see cref="RecipeSummaryVM"/> containing recipe summaries if the database is not empty;
    /// otherwise, returns <see cref="None"/>.
    /// </returns>
    public Task<OneOf<List<RecipeSummaryVM>, None>> GetRecipesSummaryAsync();

    /// <summary>
    /// Retrieves detailed information about a recipe by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve.</param>
    /// <returns>
    /// A <see cref="RecipeDetailsVM"/> object with the details of the recipe if found;
    /// otherwise, <see cref="None"/>.
    /// </returns>
    public Task<OneOf<RecipeDetailsVM, None>> GetRecipeDetailsAsync(int id);

    /// <summary>
    /// Retrieves a recipe for updating based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve for updating.</param>
    /// <returns>
    /// An
    /// <see cref="EditRecipeVM"/> object, which is used by the user to update the recipe if found;
    /// otherwise, <see cref="None"/>.
    /// </returns>
    public Task<OneOf<EditRecipeVM, None>> GetRecipeForUpdateAsync(int id);

    /// <summary>
    /// Creates a new recipe in the database based on the values provided in the <paramref name="recipeVM"/>.
    /// </summary>
    /// <param name="recipeVM">A <see cref="EditRecipeVM"/> object containing the data for the new recipe.</param>
    /// <param name="createdById">A GUID representing the user who created the recipe.</param>
    /// <returns>
    /// An <see cref="int"/> representing the unique Id of the newly created recipe.
    /// </returns>
    public Task<int> CreateRecipeAsync(EditRecipeVM recipe, string createdById);

    /// <summary>
    /// Updates a recipe in the database if it exists based on the values provided in the <paramref name="recipeVM"/>.
    /// </summary>
    /// <param name="recipeVM">A <see cref="EditRecipeVM"/> object containing the updated data for the recipe.</param>
    /// <returns>
    /// An <see cref="int"/> representing the ID of the updated recipe.
    /// A <see cref="None"/> if the recipe does not exist.
    /// </returns>
    public Task<OneOf<int, None>> UpdateRecipeAsync(EditRecipeVM recipe);

    /// <summary>
    /// Marks a recipe as deleted if it exists.
    /// </summary>
    /// <param name="id"></param>
    public Task DeleteRecipeAsync(int id);
    public Task<Recipe> GetRecipeForAuthorizationAsync(int id);
}

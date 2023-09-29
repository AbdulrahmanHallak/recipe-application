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
    /// a <see cref="OneOf{List{RecipeSummaryVM}, None}"/> where:
    ///   - The first type parameter, <see cref="List{RecipeSummaryVM}"/>, represents a list of recipe summaries if any are available.
    ///   - The second type parameter, <see cref="None"/>, if the database is empty.
    /// </returns>
    public Task<OneOf<List<RecipeSummaryVM>, None>> GetRecipesSummaryAsync();

    /// <summary>
    /// Retrieves detailed information about a recipe by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve.</param>
    /// <returns>
    /// A <see cref="OneOf{T0, T1}"/> where:
    ///   - The first type parameter, <see cref="RecipeDetailsVM"/>, an object with the details of the recipe if found.
    ///   - The second type parameter, <see cref="None"/>, if the recipe is not found.
    /// </returns>
    public Task<OneOf<RecipeDetailsVM, None>> GetRecipeDetailsAsync(int id);

    /// <summary>
    /// Retrieves a recipe for updating based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve for updating.</param>
    /// <returns>
    /// A <see cref="OneOf{EditRecipeVM, None}"/> where:
    ///   - The first type parameter, <see cref="EditRecipeVM"/>, represents the recipe to be edited if found in the database.
    ///   - The second type parameter, <see cref="None"/>, represents a case where the recipe with the specified ID is not found in the database.
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
    /// A <see cref="OneOf{int, None}"/> where:
    /// An <see cref="int"/> representing the ID of the updated recipe.
    /// A <see cref="None"/> if the recipe does not exist.
    /// </returns>
    public Task<OneOf<int, None>> UpdateRecipeAsync(EditRecipeVM recipe);

    /// <summary>
    /// Marks a recipe as deleted if it exists.
    /// </summary>
    /// <param name="id"></param>
    public Task DeleteRecipeAsync(int id);
    /// <summary>
    /// Retrieves a <see cref="Recipe"/> object based on the <paramref name="id"/> provided.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// A <see cref="Recipe"/> object with the provided id.
    /// </returns>
    /// <remarks>
    /// This method is ONLY intended to be used for resource-based authorization.
    /// </remarks>
    public Task<Recipe> GetRecipeForAuthorizationAsync(int id);

    /// <summary>
    /// Retrieves a list of <see cref="UserRecipeVM"/> objects created by the specified user, limited to a specified number of recipes.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose recipes are to be retrieved.</param>
    /// <param name="numOfRecipes">The maximum number of recipes to include in the result.</param>
    /// <returns>
    /// A <see cref="OneOf{List{UserRecipeVM}, None}"/> where:
    /// <see cref="List{UserRecipeVM}"/>, represents a list of user-created recipes if any are found.
    /// <see cref="None"/>, represents a case where the user has not created any recipes.
    /// </returns>
    public Task<OneOf<List<UserRecipesVM>, None>> GetRecipesForUserAsync(string userId, int numOfRecipes);
}

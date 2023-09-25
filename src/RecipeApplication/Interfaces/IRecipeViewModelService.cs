using OneOf;
using OneOf.Types;
using RecipeApplication.Models;

namespace RecipeApplication.Interfaces;
public interface IRecipeViewModelService
{
    public Task<OneOf<List<RecipeSummaryVM>, None>> GetRecipesSummaryAsync();
    public Task<OneOf<RecipeDetailsVM, None>> GetRecipeDetailsAsync(int id);
    public Task<OneOf<EditRecipeVM, None>> GetRecipeForUpdateAsync(int id);
    public Task<int> CreateRecipeAsync(EditRecipeVM recipe, string createdById);
    public Task<OneOf<int, None>> UpdateRecipeAsync(EditRecipeVM recipe);
    public Task DeleteRecipeAsync(int id);
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeApplication.Data;
using RecipeApplication.Interfaces;

namespace RecipeApplication.ViewComponents;
public class UserRecipesViewComponent : ViewComponent
{
    private readonly IRecipeViewModelService _recipeService;
    private readonly UserManager<ApplicationUser> _userManager;
    public UserRecipesViewComponent(IRecipeViewModelService recipeService, UserManager<ApplicationUser> userManager)
    {
        _recipeService = recipeService;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(int numOfRecipes)
    {
        if (!User.Identity!.IsAuthenticated)
            return View("Unauthenticated");

        var userId = _userManager.GetUserId(HttpContext.User);
        var result = await _recipeService.GetRecipesForUserAsync(userId!, numOfRecipes);

        return result.Match<IViewComponentResult>
        (
            (recipes) => View(recipes),
            _ => Content("You do not have any recipes")
        );
    }
}

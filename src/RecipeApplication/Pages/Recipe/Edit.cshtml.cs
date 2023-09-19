using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Models;

namespace RecipeApplication.Pages.Recipe;
[Authorize]
public class EditModel : PageModel
{
    [BindProperty]
    public EditRecipeVM Recipe { get; set; } = default!;

    private readonly RecipeService _service;
    private readonly IAuthorizationService _authService;
    public EditModel(RecipeService service, IAuthorizationService authService)
    {
        _service = service;
        _authService = authService;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var recipe = await _service.GetRecipeAsync(id);
        var authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
        if (!authResult.Succeeded)
        {
            return new ForbidResult();
        }
        Recipe = await _service.GetRecipeForUpdate(id); // The exception is handled by ExceptionHandler middleware.
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (ModelState.IsValid)
            {
                var authResult = await _authService.AuthorizeAsync(User, Recipe, "CanManageRecipe");
                if (!authResult.Succeeded)
                {
                    return new ForbidResult();
                }
                await _service.UpdateRecipe(Recipe);
                return RedirectToPage("View", new { Recipe.Id });
            }
        }
        catch (Exception)
        {
            // TODO: Log error
            // Add a model-level error by using an empty string key
            ModelState.AddModelError(
                string.Empty,
                "An error occured saving the recipe"
                );
        }

        //If we got to here, something went wrong
        return Page();
    }
}

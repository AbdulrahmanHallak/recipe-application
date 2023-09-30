using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Interfaces;
using RecipeApplication.Models;

namespace RecipeApplication.Pages.Recipe;
[Authorize]
public class EditModel : PageModel
{
    [BindProperty]
    public EditRecipeVM Recipe { get; set; } = default!;

    private readonly IRecipeViewModelService _service;
    private readonly IAuthorizationService _authService;
    public EditModel(IRecipeViewModelService service, IAuthorizationService authService)
    {
        _service = service;
        _authService = authService;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var recipe = await _service.GetForAuthorizationAsync(id);
        var authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
        if (!authResult.Succeeded)
        {
            return new ForbidResult();
        }
        var result = await _service.GetForUpdateAsync(id);

        return result.Match
        (
            (recipe) => { Recipe = recipe; return Page(); },
            _ => Page()
        );
    }
    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (ModelState.IsValid)
            {
                var recipe = await _service.GetForAuthorizationAsync(Recipe.Id);
                var authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
                if (!authResult.Succeeded)
                {
                    return new ForbidResult();
                }
                var result = await _service.UpdateAsync(Recipe);
                return result.Match<IActionResult>
                (
                    id => RedirectToPage("View", new { id }),
                    _ => Page()
                );
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

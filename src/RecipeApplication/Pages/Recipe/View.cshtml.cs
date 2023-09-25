using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Interfaces;
using RecipeApplication.Models;

namespace RecipeApplication.Pages.Recipe;

public class ViewModel : PageModel
{
    public RecipeDetailsVM? Recipe { get; set; }
    public bool CanManageRecipe { get; set; } // This is only used to hide UI elements

    private readonly IRecipeViewModelService _service;
    private readonly IAuthorizationService _authService;

    public ViewModel(IRecipeViewModelService service, IAuthorizationService authService)
    {
        _service = service;
        _authService = authService;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var recipe = await _service.GetRecipeForAuthorizationAsync(id);
        var authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
        CanManageRecipe = authResult.Succeeded;

        var result = await _service.GetRecipeDetailsAsync(id);
        return result.Match<IActionResult>
        (
            recipe => { Recipe = recipe; return Page(); },
            _ => RedirectToPage("/Error")
        );
    }
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var recipe = await _service.GetRecipeForAuthorizationAsync(id);
        var authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
        if (!authResult.Succeeded)
        {
            return new ForbidResult();
        }
        await _service.DeleteRecipeAsync(id);
        return RedirectToPage("/Index");
    }
}

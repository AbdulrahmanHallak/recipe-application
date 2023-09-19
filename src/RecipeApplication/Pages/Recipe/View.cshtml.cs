using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Models;

namespace RecipeApplication.Pages.Recipe;

public class ViewModel : PageModel
{
    public RecipeDetailsVM? Recipe { get; set; }

    public bool CanManageRecipe { get; set; } // This is only used to hide UI elements
    private readonly RecipeService _service;
    private readonly IAuthorizationService _authService;

    public ViewModel(RecipeService service, IAuthorizationService authService)
    {
        _service = service;
        _authService = authService;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        Recipe = await _service.GetRecipeDetails(id)!; // The exception is handled by the ExceptionHandler middleware.

        var recipe = _service.GetRecipeAsync(id);
        var authResult = await _authService.AuthorizeAsync(User, recipe , "CanManageRecipe");
        CanManageRecipe = authResult.Succeeded;

        return Page();
    }
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _service.DeleteRecipe(id);
        return RedirectToPage("/Index");
    }
}

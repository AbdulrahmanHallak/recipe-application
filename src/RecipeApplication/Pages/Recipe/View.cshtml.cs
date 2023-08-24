using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Models.ViewModels;

namespace RecipeApplication.Pages.Recipe;

public class ViewModel : PageModel
{
    public RecipeDetailsVM? Recipe { get; set; }
    private readonly RecipeService _service;
    public ViewModel(RecipeService service)
    {
        _service = service;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        Recipe = await _service.GetRecipeDetails(id)!;
        if (Recipe == null) return NotFound();

        return Page();
    }
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _service.DeleteRecipe(id);
        return RedirectToPage("/Index");
    }
}

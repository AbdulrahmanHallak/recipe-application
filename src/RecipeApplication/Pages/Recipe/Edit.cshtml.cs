using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Models;

namespace RecipeApplication.Pages.Recipe;

public class EditModel : PageModel
{
    [BindProperty]
    public EditRecipeVM Recipe { get; set; } = default!;

    private readonly RecipeService _service;
    public EditModel(RecipeService service)
    {
        _service = service;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        Recipe = await _service.GetRecipeForUpdate(id);
        if (Recipe == null) return NotFound();
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateRecipe(Recipe);
                return RedirectToPage("View", new {Recipe.Id});
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Models;

namespace RecipeApplication.Pages.Recipe;

public class CreateModel : PageModel
{
    [BindProperty]
    public EditRecipeVM Recipe { get; set; } = default!;

    private readonly RecipeService _service;
    public CreateModel(RecipeService service)
    {
        _service = service;
    }
    public void OnGet()
    {
        Recipe = new EditRecipeVM();
    }
    public async Task<IActionResult> OnPost()
    {
        try
        {
            if (ModelState.IsValid)
            {
                var id = await _service.CreateRecipe(Recipe);
                return RedirectToPage("View", new { Id = id });
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

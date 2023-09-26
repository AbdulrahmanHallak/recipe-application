using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Data;
using RecipeApplication.Interfaces;
using RecipeApplication.Models;

namespace RecipeApplication.Pages.Recipe;

[Authorize]
public class CreateModel : PageModel
{
    [BindProperty]
    public EditRecipeVM Recipe { get; set; } = default!;

    private readonly IRecipeViewModelService _service;
    private readonly UserManager<ApplicationUser> _userManager;
    public CreateModel(IRecipeViewModelService service, UserManager<ApplicationUser> userManager)
    {
        _service = service;
        _userManager = userManager;
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
                var user = await _userManager.GetUserAsync(User);
                if (user is null)
                    return Page();

                var id = await _service.CreateRecipeAsync(Recipe, user.Id);
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

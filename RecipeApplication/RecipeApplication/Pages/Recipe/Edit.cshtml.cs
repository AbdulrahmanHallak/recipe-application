using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Models.ViewModels;

namespace RecipeApplication.Pages.Recipe;

public class EditModel : PageModel
{
    public EditRecipeVM? Recipe { get; set; }
    private readonly RecipeService _service;
    public EditModel(RecipeService service)
    {
        _service = service;
    }
    public void OnGet(int id)
    {
    }
}

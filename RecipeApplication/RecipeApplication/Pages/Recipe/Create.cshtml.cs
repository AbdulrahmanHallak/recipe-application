using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Models.ViewModels;

namespace RecipeApplication.Pages.Recipe;

public class CreateModel : PageModel
{
    [BindProperty]
    public RecipeDetailsVM? Recipe { get; set; }
    public void OnGet()
    {
    }
}

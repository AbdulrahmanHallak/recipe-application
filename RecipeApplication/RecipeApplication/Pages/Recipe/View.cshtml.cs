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
    public void OnGet(int id)
    {
    }
}

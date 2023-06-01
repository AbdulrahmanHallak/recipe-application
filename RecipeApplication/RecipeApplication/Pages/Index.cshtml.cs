using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Models.ViewModels;

namespace RecipeApplication.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly RecipeService _service;
    public IEnumerable<RecipeSummaryVM> Recipe { get; set; } = default!;

    public IndexModel(ILogger<IndexModel> logger , RecipeService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> OnGet()
    {
        Recipe = await _service.GetRecipes();
        return Page();
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Interfaces;
using RecipeApplication.Models;

namespace RecipeApplication.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IRecipeViewModelService _service;
    public List<RecipeSummaryVM>? Recipes { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IRecipeViewModelService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> OnGet()
    {
        var result = await _service.GetSummariesAsync();
        return result.Match<IActionResult>
        (
            list => { Recipes = list; return Page(); },
            _ => Page()
        );
    }
}

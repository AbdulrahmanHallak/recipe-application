using Microsoft.AspNetCore.Mvc;
using RecipeApplication.Filters;
using RecipeApplication.Interfaces;
using RecipeApplication.Models;

namespace RecipeApplication.Controllers;

[Route("api/[controller]")]
[ApiController, ApiEnabled]
public class RecipeApiController : ControllerBase
{
    private IRecipeViewModelService _service;
    public RecipeApiController(IRecipeViewModelService service)
    {
        _service = service;
    }
    // GET: api/RecipeApi/123
    [HttpGet("{id}")]
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var result = await _service.GetDetailAsync(id);
        return result.Match<IActionResult>
        (
            recipe => Ok(recipe),
            _ => NotFound()
        );
    }
    // PUT: api/RecipeApiController/123
    [HttpPut("{id}")]
    public async Task<IActionResult> OnPutAsynck(int id, EditRecipeVM recipe)
    {
        var result = await _service.UpdateAsync(recipe);
        return result.Match<IActionResult>
        (
            id => Ok(id),
            _ => NotFound()
        );
    }
}

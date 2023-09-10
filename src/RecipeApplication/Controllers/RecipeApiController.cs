using Microsoft.AspNetCore.Mvc;
using RecipeApplication.Filters;
using RecipeApplication.Models;

namespace RecipeApplication.Controllers;

[Route("api/[controller]")]
[ApiController, ApiEnabled, RecipeNotFound]
public class RecipeApiController : ControllerBase
{
    private RecipeService _service;
    public RecipeApiController(RecipeService service)
    {
        _service = service;
    }
    // GET: api/RecipeApi/123
    [HttpGet("{id}")]
    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var recipe = await _service.GetRecipeDetails(id);
            Response.GetTypedHeaders().LastModified = recipe.LastModified;
            return Ok(recipe);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
    // PUT: api/RecipeApiController/123
    [HttpPut("{id}")]
    public async Task<IActionResult> OnPutAsynck(int id, EditRecipeVM recipe)
    {
        try
        {
            var recipeId = await _service.UpdateRecipe(recipe);
            return Ok();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}

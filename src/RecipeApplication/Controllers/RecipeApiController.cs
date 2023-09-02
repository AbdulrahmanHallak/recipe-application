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
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var recipe = await _service.GetRecipeDetails(id);
        Response.GetTypedHeaders().LastModified = recipe.LastModified;
        return Ok(recipe);
    }
    [HttpPost("{id}")]
    public async Task<IActionResult> Update(int id, EditRecipeVM recipe)
    {
        var recipeId = await _service.UpdateRecipe(recipe);
        return Ok();
    }
}

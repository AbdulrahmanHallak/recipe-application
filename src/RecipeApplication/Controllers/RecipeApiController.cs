using Microsoft.AspNetCore.Mvc;
using RecipeApplication.Models;

namespace RecipeApplication.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RecipeApiController : ControllerBase
{
    private readonly bool isApiEnabled; // To check if the API is enabled. The value will be loaded from Configuration.
    private RecipeService _service;
    public RecipeApiController(RecipeService service, IConfiguration configuration)
    {
        _service = service;
        isApiEnabled = configuration.GetValue<bool>("IsApiEnabled", false);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        if (!isApiEnabled) return BadRequest();
        try
        {
            var recipe = await _service.GetRecipeDetails(id);
            Response.GetTypedHeaders().LastModified = recipe.LastModified;
            return Ok(recipe);
        }
        catch (Exception ex)
        {
            return GetErrorResponse(ex);
        }
    }
    [HttpPost("{id}")]
    public async Task<IActionResult> Update(int id, EditRecipeVM recipe)
    {
        if (!isApiEnabled) return BadRequest();
        try
        {
            var recipeId = await _service.UpdateRecipe(recipe);
            return Ok();
        }
        catch (Exception ex)
        {
            return GetErrorResponse(ex);
        }
    }

    private IActionResult GetErrorResponse(Exception ex)
    {
        var error = new ProblemDetails()
        {
            Title = "An error occured",
            Detail = ex.Message,
            Type = "https://httpsstatuses.com/500",
            Status = 500
        };
        return new ObjectResult(error)
        {
            StatusCode = 500
        };
    }

}

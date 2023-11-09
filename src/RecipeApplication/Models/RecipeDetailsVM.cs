using RecipeApplication.Data;

namespace RecipeApplication.Models;

public class RecipeDetailsVM
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Method { get; set; } = default!;
    public string TimeToCook { get; set; } = default!;
    public DateTimeOffset LastModified { get; set; }
    public IEnumerable<Ingredient> Ingredients { get; set; } = default!;
}

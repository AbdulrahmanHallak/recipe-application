namespace RecipeApplication.Models;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeSpan TimeToCook { get; set; }
    public bool IsDeleted { get; set; }
    public string Method { get; set; } = string.Empty;
    public DateTimeOffset LastModified { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; } = default!;
}

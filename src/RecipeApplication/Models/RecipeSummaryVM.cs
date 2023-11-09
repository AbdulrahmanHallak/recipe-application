namespace RecipeApplication.Models;

public class RecipeSummaryVM
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    // Made the TimeToCook a string for the index view.
    public string TimeToCook { get; set; } = default!;
    public int NumberOfIngredients { get; set; }
}

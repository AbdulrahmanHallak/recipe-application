using System.ComponentModel.DataAnnotations;

namespace RecipeApplication.Models;

public class EditIngredientsVM
{
    // Had to include the ingredient bc each recipe has multiple ingredient and there is no way to track their id without including it in the model.
    public int Id { get; set; }
    [Required, StringLength(50)]
    public string Name { get; set; } = default!;


    [Required, Range(0, 10_000)]
    public decimal Quantity { get; set; }


    [Required, StringLength(50)]
    public string Unit { get; set; } = default!;
}

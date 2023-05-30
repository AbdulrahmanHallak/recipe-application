using System.ComponentModel.DataAnnotations;

namespace RecipeApplication.Models.ViewModels;

public class EditRecipeVM
{
    [Required, StringLength(20)]
    public string Name { get; set; } = string.Empty;


    [Required, DataType(DataType.Duration)]
    public TimeSpan TimeToCook { get; set; }


    [Required, StringLength(150)]
    public string Method { get; set; } = string.Empty;

    public ICollection<EditIngredientsVM> Ingredients { get; set; } = default!;
}

using System.ComponentModel.DataAnnotations;

namespace RecipeApplication.Models.ViewModels;

public class EditRecipeVM
{
    public int Id { get; set; }
    
    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;


    [Required, DataType(DataType.Duration)]
    public TimeSpan TimeToCook { get; set; }


    [Required, StringLength(300)]
    public string Method { get; set; } = string.Empty;

    public List<EditIngredientsVM> Ingredients { get; set; } = new List<EditIngredientsVM>();
}

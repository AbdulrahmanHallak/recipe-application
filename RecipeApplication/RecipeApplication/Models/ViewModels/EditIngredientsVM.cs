using System.ComponentModel.DataAnnotations;

namespace RecipeApplication.Models.ViewModels;

public class EditIngredientsVM
{
    [Required, StringLength(15)]
    public string Name { get; set; } = string.Empty;


    [Required, Range(1 , 100)]
    public decimal Quantity { get; set; }


    [Required, StringLength(15)]
    public string Unit { get; set; } = string.Empty;
}

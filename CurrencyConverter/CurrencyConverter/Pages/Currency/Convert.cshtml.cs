using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CurrencyConverter.Pages.Currency
{
    public class ConvertModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public SelectListItem[] CurrencyCodes { get; } =
        {
            new SelectListItem{Text="GBP", Value = "GBP"},
            new SelectListItem{Text="USD", Value = "USD"},
            new SelectListItem{Text="CAD", Value = "CAD"},
            new SelectListItem{Text="EUR", Value = "EUR"},
        };

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if(Input.CurrencyFrom == Input.CurrencyTo)
            {
                ModelState.AddModelError(string.Empty,"Cannot convert currency to itself");
            }

            if(!ModelState.IsValid)
                return Page();

            return RedirectToPage("Success");
        }


        public class InputModel
        {
            [Required, DisplayName("Currency from"), StringLength(3, MinimumLength = 3), CurrencyCode("GBP", "USD", "CAD", "EUR")]
            public string CurrencyFrom { get; set; } = default!;



            [Required, DisplayName("Currency to"), StringLength(3, MinimumLength = 3), CurrencyCode("GBP", "USD", "CAD", "EUR")]
            public string CurrencyTo { get; set; } = default!;


            [Required, Range(1, 1000)]
            public decimal Quantity { get; set; } = default!;
        }
    }
}

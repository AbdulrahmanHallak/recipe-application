using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace RecipeApplication.Pages;
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public string? ExceptionMessage { get; set; }

    public ErrorModel()
    {
    }

    public void OnGet()
    {
        var exceptionHandlerPath = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPath?.Error is InvalidOperationException)
        {
            ExceptionMessage = "Recipe not found";
        }
        else
        {
            ExceptionMessage = "An error occured. Please try again. If the issue persisted please contact us";
        }
    }
}


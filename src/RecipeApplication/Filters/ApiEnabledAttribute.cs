using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RecipeApplication.Filters;
public class ApiEnabledAttribute : Attribute, IResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        bool isEnabled = false;
        var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
        if (configuration is not null)
            isEnabled = configuration.GetValue<bool>("IsApiEnabled", false);

        if (isEnabled is false)
            context.Result = new BadRequestResult();
    }
}

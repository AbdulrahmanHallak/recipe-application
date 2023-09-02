using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RecipeApplication.Filters;
public class RecipeNotFoundAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is InvalidOperationException)
            context.Result = new NotFoundResult();
    }
}

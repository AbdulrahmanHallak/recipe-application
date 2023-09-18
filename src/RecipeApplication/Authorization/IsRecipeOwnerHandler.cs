using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RecipeApplication.Data;

namespace RecipeApplication.Authorization;
public class IsRecipeOwnerHandler : AuthorizationHandler<IsRecipeOwnerRequirement, Recipe>
{
    public IsRecipeOwnerHandler(){ }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsRecipeOwnerRequirement requirement, Recipe resource)
    {
        var userId = context.User.FindFirstValue("UserId");
        if (userId is null)
        {
            return;
        }
        if( resource.CreatedById == userId)
        {
            context.Succeed(requirement);
        }
    }
}

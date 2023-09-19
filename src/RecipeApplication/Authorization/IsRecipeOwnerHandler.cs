using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RecipeApplication.Data;

namespace RecipeApplication.Authorization;
public class IsRecipeOwnerHandler : AuthorizationHandler<IsRecipeOwnerRequirement, Recipe>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IsRecipeOwnerHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsRecipeOwnerRequirement requirement, Recipe resource)
    {
        var user = await _userManager.GetUserAsync(context.User);
        if (user is null)
        {
            return;
        }
        if (resource.CreatedById == user.Id)
        {
            context.Succeed(requirement);
        }
    }
}

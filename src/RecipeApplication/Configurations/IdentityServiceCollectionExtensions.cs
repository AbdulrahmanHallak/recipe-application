using Microsoft.AspNetCore.Identity;
using RecipeApplication.Configurations;
using RecipeApplication.Data;

namespace RecipeApplication;
public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<CustomEmailConfirmationTokenProvider<ApplicationUser>>(); // Register the custom service into container.
        services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Tokens.ProviderMap.Add // Add custom DataProtectorTokenProvider to override tokens lifespan.
            (
                "CustomEmailConfirmation",
                new TokenProviderDescriptor(typeof(CustomEmailConfirmationTokenProvider<ApplicationUser>))
            );
            options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
        })
        .AddEntityFrameworkStores<RecipeApplicationContext>();

        services.AddAuthentication().AddGoogle(options =>
        {
            options.ClientId = configuration["Authentication__Google__ClientId"] ?? throw new InvalidOperationException("ClientID for auth is null");
            options.ClientSecret = configuration["Authentication__Google__ClientSecret"] ?? throw new InvalidOperationException("ClientSecret is null for google auth");
        });
        return services;
    }
}
using Microsoft.AspNetCore.Identity.UI.Services;
using RecipeApplication.Services;
namespace RecipeApplication.Configurations;
public static class ConfigureEmailServices
{
    public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configurations)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddFluentEmail("abdulrahman.m.hallak.293@gmail.com")
                .AddMailGunSender("sandboxe4ca7a54fa5a4729871cd7e5ccd0f4d1.mailgun.org", configurations["MailgunKey"]);
        return services;
    }
}

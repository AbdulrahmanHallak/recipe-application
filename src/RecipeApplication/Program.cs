using Microsoft.EntityFrameworkCore;
using RecipeApplication.Data;
using RecipeApplication.Configurations;
using RecipeApplication.Authorization;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using RecipeApplication.Interfaces;
namespace RecipeApplication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure serilog.
        var serilogConfig = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom
            .Configuration(serilogConfig)
            .CreateLogger();

        builder.Host.UseSerilog();

        // Add services to the container.

        builder.Services.AddDbContext<RecipeApplicationContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("RecipeApplicationContext")
            ?? throw new InvalidOperationException("Connection string 'RecipeApplication' not found.")));

        builder.Services.AddAuthenticationServices(builder.Configuration); // Add ASP.NET Core Identity and Google external login.

        builder.Services.AddAuthorization(configure =>
        {
            configure.AddPolicy("CanManageRecipe", policyBuilder =>
            {
                policyBuilder.AddRequirements(new IsRecipeOwnerRequirement());
            });
        });

        builder.Services.AddScoped<IAuthorizationHandler, IsRecipeOwnerHandler>();

        builder.Services.AddScoped<IRecipeViewModelService, RecipeViewModelService>();

        builder.Services.AddEmailServices(builder.Configuration); // Add FluentEmail and Mailgun services.

        builder.Services.AddRazorPages();
        builder.Services.AddControllers();

        var app = builder.Build();


        // Seed the db if it is empty.
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            SeedData.Initialize(services);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();

        app.Run();
    }
}

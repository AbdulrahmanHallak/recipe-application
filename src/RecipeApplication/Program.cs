using Microsoft.EntityFrameworkCore;
using RecipeApplication.Data;
namespace RecipeApplication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddDbContext<RecipeApplicationContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("RecipeApplicationContext") ?? throw new InvalidOperationException("Connection string 'RecipeApplication' not found.")));

        builder.Services.AddRazorPages();
        builder.Services.AddControllers();
        builder.Services.AddScoped(typeof(RecipeService));

        var app = builder.Build();

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

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();

        app.Run();
    }
}

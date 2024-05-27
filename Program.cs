// Goal is to take MS doco example and walk through it to better understand ASP.NET DI scopes\lifetimes
// We want to see the different scopes\lifetimes in action(via console logs and/or webpage outputs)

namespace TestDI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();

        builder.Services.AddTransient<IOperationTransient, Operation>();
        builder.Services.AddScoped<IOperationScoped, Operation>();
        builder.Services.AddSingleton<IOperationSingleton, OperationSingleton>();

        /*
         * If we did the below - how does it know which one to hand out to us when we request it?
         * I think it just hands back the first one it registered?
        builder.Services.AddTransient<IOperation, Operation>();
        builder.Services.AddScoped<IOperation, Operation>();
        builder.Services.AddSingleton<IOperation, Operation>();
        */

        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // Our custom middleware
        app.UseMyMiddleware();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();

    }
}
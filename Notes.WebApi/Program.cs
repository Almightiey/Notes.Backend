using Microsoft.EntityFrameworkCore;
using Notes.Application;
using Notes.Application.Common.Mapping;
using Notes.Application.Interfaces;
using Notes.Persistence;
using System.Reflection;

public class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        using (var scope = builder.Services.BuildServiceProvider().CreateScope()) // invoke method of Db initialization
        {
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var context = serviceProvider.GetRequiredService<NotesDbContext>(); // for accessing dependencies
                DbInitializer.Init(context); // initialize database
            }
            catch (Exception e) { }
        }
        // Add services to the container.
        // builder.Services.AddRazorPages();
        builder.Services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
        });

        builder.Services.AddScoped<NotesDbContext>();
        builder.Services.AddPersistence(builder.Configuration);

        builder.Services.AddApplication();

        
        var app = builder.Build();

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
        app.UseCors("AllowAll");
        //app.UseAuthorization();

        //app.MapRazorPages();

        app.Run();
    }
}

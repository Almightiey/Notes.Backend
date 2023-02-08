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

        builder.Services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
        });

        builder.Services.AddApplication();

        builder.Services.AddPersistence(builder.Configuration);
        builder.Services.AddControllers();

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

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");

            app.UseHsts();
        }
        
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseCors("AllowAll"); // change this
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }
}

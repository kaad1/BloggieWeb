using BloggieWeb1.Data;
using BloggieWeb1.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BloggieWeb1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidCastException("Default Connection not found");
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IBlogPostRespository, BlogPostRepository>(); 
            builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();
            builder.Services.AddDbContext<BloggieDbContext>( options =>  options.UseSqlServer(connectionString) );
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

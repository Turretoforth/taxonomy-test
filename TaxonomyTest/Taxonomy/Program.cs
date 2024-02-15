
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Taxonomy.Business;
using Taxonomy.Business.Interface;
using Taxonomy.DbModels;

namespace Taxonomy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<TaxonomyDbContext>(o => o.UseSqlite("Data Source=taxonomy.db"));
            builder.Services.AddScoped<ITaxonomyBusiness, TaxonomyBusiness>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Ensure the database is created and apply all migrations at startup
            // Should not be used in a production scenario, it's here for convenience and time
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TaxonomyDbContext>();
                db.Database.Migrate();
            }

            app.Run();
        }
    }
}

using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;


namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using( var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if(!context.Platforms.Any())
            {
                Console.WriteLine("---> Seeding Data...");

                context.Platforms.AddRange(
                    new Platform() {Name="Dot Net", Publisher="Microsoft", Cost="Free"},
                     new Platform() {Name="Jango", Publisher="Python Corp", Cost="Free"},
                      new Platform() {Name="Rails", Publisher="Rails Corp", Cost="Free"},
                       new Platform() {Name="Spring", Publisher="Java", Cost="Free"},
                        new Platform() {Name="AWS", Publisher="Amazon", Cost="Yearly Costs, XXXXX"}
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---> We already have data");
            }
        }
    }
}
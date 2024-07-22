using Blazor_ApiStatusCheck.Connection;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Blazor_ApiStatusCheck.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<ApiHealthCheck> HealthChecks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            
            modelbuilder.Entity<ApiHealthCheck>().HasData(
                new ApiHealthCheck
                {
                    Api__Id="1",
                    Name = "Api_1",
                    Url = "www.api1.com",
                    CreatedAt = DateTime.Now,
                    LastChecked = DateTime.Now,
                    Message="healthy",
                    Status="healthy"
                },
                new ApiHealthCheck
                {
                    Api__Id = "2",
                    Name = "Api_2",
                    Url = "www.api2.com",
                    CreatedAt = DateTime.Now,
                    LastChecked = DateTime.Now,
                    Message = "healthy",
                    Status = "healthy"

                },
                new ApiHealthCheck
                {
                    Api__Id = "3",
                    Name = "Api_3",
                    Url = "www.api3.com",
                    CreatedAt = DateTime.Now,
                    LastChecked = DateTime.Now,
                    Message = "unhealthy",
                    Status = "unhealthy"
                }
                ); 
        }        
    }
   
}

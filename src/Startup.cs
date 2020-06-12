using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CustomerApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: Add Swagger
            //TODO: Add logging
            //TODO: Add logging middleware - https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-3.1
            //TODO: Add correlation id for tracking requests https://github.com/ekmsystems/serilog-enrichers-correlation-id/blob/master/README.md
            //TODO: Add exception handling middleware? Probably overkill for such a simple API
            services.AddDbContext<CustomerContext>(opt => opt.UseInMemoryDatabase("CustomerDb"));
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

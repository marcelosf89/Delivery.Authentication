using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Builder;

#if DEBUG
using Swashbuckle.AspNetCore.Swagger;
#endif

namespace Delivery.Authentication.Api.Configs.Swagger
{
    public static  class SwaggerConfigurationExtension
    {
        public static void AddSwagger(this IServiceCollection services, Version version)
        {
#if DEBUG
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Auth API", Version = $"{version.Major}.{version.Minor}.{version.Build}" });
            });
#endif
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
#if DEBUG
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "{uitemplate}/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery API");
            });

            app.UseReDoc(c =>
            {
                c.RoutePrefix = "api-docs";
                c.SpecUrl = "v1/swagger.json";
            });
#endif
        }
    }
}

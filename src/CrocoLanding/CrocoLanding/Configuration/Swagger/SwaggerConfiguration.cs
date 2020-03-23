using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace CrocoLanding.Configuration.Swagger
{
    public class SwaggerConfiguration
    {
        public static void ConfigureSwagger(IServiceCollection services, List<string> xmlDocs)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Title"
                });
                //c.SchemaFilter<EnumSchemaFilter>();
                c.EnableAnnotations();
                xmlDocs.ForEach(x => c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, x)));
            });
        }
    }
}
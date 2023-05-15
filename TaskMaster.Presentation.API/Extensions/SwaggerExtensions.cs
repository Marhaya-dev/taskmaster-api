using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace TaskMaster.Presentation.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TASK MASTER API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT contendo claim com user_id.\r\n\r\n" +
                    "Digite 'Bearer' [espaço] e em seguida seu token na caixa de texto abaixo.\r\n\r\n" +
                    "Exemplo: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },

                            UnresolvedReference = true
                        },

                        new List<string>()
                    }
                });
            });

            return services;
        }
    }
}

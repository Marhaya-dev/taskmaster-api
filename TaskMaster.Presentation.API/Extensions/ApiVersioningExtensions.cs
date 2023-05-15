using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace TaskMaster.Presentation.API.Extensions
{
    public static class ApiVersioningExtensions
    {
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddControllersWithViews(o =>
            {
                o.UseGeneralRoutePrefix("api/v{version:apiVersion}");
            });

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
            });

            return services;
        }
    }
}

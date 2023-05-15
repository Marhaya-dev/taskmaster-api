using FluentMigrator.Runner;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Buffers.Text;
using System.Linq;
using TaskMaster.Domain.ViewModels.v1;
using TaskMaster.Infra.IOC;
using TaskMaster.Presentation.API.Extensions;
using TaskMaster.Presentation.API.Middlewares;

namespace TaskMasterAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/json" }
                );
            });

            services.AddRateLimiting(Configuration);
            services.AddVersioning();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<ListMeta>();

            services.AddControllers();
            services.AddAuth();
            services.AddHttpContextAccessor();
            services.AddSwagger();
            services.ResolveDependencies(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }

            app.UseRateLimiting();

            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Master API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}

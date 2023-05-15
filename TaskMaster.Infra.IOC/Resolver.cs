using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TaskMaster.Application.Interfaces.Services;
using TaskMaster.Application.Services;
using TaskMaster.Domain.Mappings;
using TaskMaster.Domain.Settings;
using TaskMaster.Domain.Utils;
using TaskMaster.Infra.Interfaces;
using TaskMaster.Infra.Interfaces.Repositories;
using TaskMaster.Infra.Migrations;
using TaskMaster.Infra.Repositories;

namespace TaskMaster.Infra.IOC
{
    public static class Resolver
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            DbConnectionSettings dbConnection = JsonUtils.ReadDbConnection();
            var connectionString = @$"Data Source={dbConnection.Server}; Initial Catalog={dbConnection.Database}; User ID={dbConnection.User}; Password={dbConnection.Password};";
            var connectionFactory = new SqlConnectionFactory(connectionString);

            services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(MigrationsAssembly).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddSingleton<IDbConnectionFactory>(connectionFactory);

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITaskStatusRepository, TaskStatusRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITaskStatusService, TaskStatusService>();
            services.AddTransient<ITaskService, TaskService>();

            return services;
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(ps =>
            {
                ps.AddProfile(new UserMappingProfile());
                ps.AddProfile(new TaskStatusMappingProfile());
                ps.AddProfile(new TaskMappingProfile());
            });

            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<DbConnectionSettings>(configuration.GetSection("DbConnectionSettings"));

            return services;
        }

        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMappings();
            services.AddSettings(configuration);
            services.AddRepositories(configuration);
            services.AddServices();

            return services;
        }
    }
}

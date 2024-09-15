using Administration.Application.Interfaces;
using Administration.Infra.Data.Context;
using Administration.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Administration.Application.Interfaces.Authentication;
using Administration.Application.Services.Authentication;
using Administration.Application.Services.Users;
using Administration.Application.Services.Projects;
using Administration.Application.Interfaces.Projects;
using Administration.Infra.Data.Repositories.Interfaces;
using Administration.Application.Interfaces.UserInterface;
using Administration.Application.Services;

namespace Administration.Infra.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IDbContext, ProjectDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserService, UserService>();
            //Register services
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProjectTaskService, ProjectTaskService>();
            services.AddScoped<IUploadService, LocalStorageUploadService>();

        }

    }
}

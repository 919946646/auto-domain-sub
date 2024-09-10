using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.CRM.AppServices
{
    public static class AddModulesInjector
    {
        /// <summary>
        /// 注意：需要注册到Program.cs文件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddERPModulesConfig(this IServiceCollection services, ConfigurationManager configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //注入MediatR(不用单独注入领域事件了)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Domain.ApplicationAssemblyHook).Assembly));

            //覆盖MES服务 Project_statsService
            //services.AddScoped<IProject_statsService, Project_stats.Project_statsService>();
        }
    }
}
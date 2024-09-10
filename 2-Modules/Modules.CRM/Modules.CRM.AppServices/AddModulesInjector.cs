using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.CRM.AppServices
{
    public static class AddModulesInjector
    {
        /// <summary>
        /// ע�⣺��Ҫע�ᵽProgram.cs�ļ�
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddERPModulesConfig(this IServiceCollection services, ConfigurationManager configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //ע��MediatR(���õ���ע�������¼���)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Domain.ApplicationAssemblyHook).Assembly));

            //����MES���� Project_statsService
            //services.AddScoped<IProject_statsService, Project_stats.Project_statsService>();
        }
    }
}
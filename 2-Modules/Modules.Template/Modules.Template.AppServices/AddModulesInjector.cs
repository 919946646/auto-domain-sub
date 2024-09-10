using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Template.AppServices.CodeTemplate;

namespace Modules.Template.AppServices
{
    public static class AddModulesInjector
    {
        /// <summary>
        /// ע�⣺��Ҫע�ᵽProgram.cs�ļ�
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddTemplateModulesConfig(this IServiceCollection services, ConfigurationManager configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //ע��MediatR(���õ���ע�������¼���)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Domain.ApplicationAssemblyHook).Assembly));

            services.AddScoped<Domain.CodeTemplate.ICodeTemplateRepository, Repository.CodeTemplate.CodeTemplateRepository>();
            services.AddScoped<ICodeTemplateService, CodeTemplateService>();
            services.AddScoped<ICodeTemplatePageService, CodeTemplatePageService>();
            //tree
            services.AddScoped<ICodeTemplateTreeService, CodeTemplateTreeService>();
            services.AddScoped<Domain.CodeTemplate.ICodeTemplateTreeRepository, Repository.CodeTemplate.CodeTemplateTreeRepository>();
        }
    }
}
using Blazor.Server;
using Blazor.Server.Components;
using Blazor.Server.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using Modules.CodeGenerator.AppServices;
using Modules.Core.AppServices;
using Modules.Core.Blazor.SignalR;
using Modules.Core.Shared;
using Modules.Core.Shared.AppConfig;
using Modules.CRM.AppServices;
using Modules.MES.AppServices;
using Modules.Tasks.AppServices;
using Modules.Template.AppServices;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.DownstreamRouteFinder.Finder;
using Ocelot.Middleware;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
/* ȫ�ֱ������� */
GlobalConfig.SiteId = SiteEnum.ZXZG;

// Add services to the container.
builder.Services.AddRazorComponents(options =>
    options.DetailedErrors = builder.Environment.IsDevelopment())
    .AddInteractiveServerComponents().AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
        options.HandshakeTimeout = TimeSpan.FromSeconds(30);
    }); 

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

ConfigurationManager configurationManager = new();
configurationManager.SetBasePath(System.IO.Path.Combine(System.AppContext.BaseDirectory, "Config"))
    .AddJsonFile("ocelot.json", true, reloadOnChange: true)
    .AddJsonFile("ocelot.{hostingContext.HostingEnvironment.EnvironmentName.ToLower()}.json", true, true)
    .AddEnvironmentVariables();
builder.Services.AddOcelot(configurationManager);

//���ڽ��  <Routes @rendermode="InteractiveServer" /> ģʽ�����ʹ��@attribute [Authorize] ����ҳ��ˢ��404�Ҳ�������
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
#region ����ע��
//��ʾ��ϸ����
builder.Services.Configure<CircuitOptions>(options => { options.DetailedErrors = true; });

//����signaIR��Ϣ�����ƣ�Ĭ��32K����JS���ý�ͼ��ʱ�򣬴�ͼƬ���ܴ��ݣ�Ҳ��ͨ���ָ�ͼƬ��δ��ݣ�
builder.Services.Configure<HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024 * 10; // 10MB or use null ��nullΪ�����ƴ�С
});

builder.Services.AddLocalization(); //��̬��������

//ע�������ģ�AOP������Ի�ȡIOC����
builder.Services.AddHttpContextAccessor();

//builder.AddErrorLogConfigPostgre(); ��¼������־
#endregion
#region ģ��ע��(����ע��Ҫ����ǰ�棬����ĻḲ��ǰ��ķ���)
builder.Services.AddCoreModulesConfig(builder.Configuration); //����ģ��
builder.Services.AddTaskModulesConfig(builder.Configuration); //����ģ��
builder.Services.AddMesModulesConfig(builder.Configuration); //����MESģ��
builder.Services.AddCodeGeneratorModulesConfig(builder.Configuration); //��������ģ��
builder.Services.AddTemplateModulesConfig(builder.Configuration); //ģ��ģ��

builder.Services.AddERPModulesConfig(builder.Configuration); //CRMģ��


#endregion
//���������еķ���(����ģ��)
builder.Services.AddHostedService<QuartzHostedService>();
//ʹ��AntDesign UI
builder.Services.AddAntDesign();

//ʹ��SingaIR
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
       new[] { "application/octet-stream" });
});
builder.Services.AddScoped<HubClient>(); //ע��SingalR����

var app = builder.Build();
app.UseResponseCompression();
// Ҫ���������Ϊ����ʹ����BuildServiceProvider
HostService.RegisterHostServices(builder.Services);

//AntDesign�л���ǰ�̵߳�����
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
//����������ʱ��
//app.UseRequestLocalization("zh-CN");
//ȫ�ָ�ʽ������,
CultureInfo culinfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
DateTimeFormatInfo dateInfo = (DateTimeFormatInfo)Thread.CurrentThread.CurrentCulture.DateTimeFormat.Clone();
dateInfo.ShortDatePattern = "yyyy-MM-dd";
dateInfo.LongDatePattern = "yyyy-MM-dd";
dateInfo.ShortTimePattern = "HH:mm";
dateInfo.LongTimePattern = "HH:mm:ss";
culinfo.DateTimeFormat = dateInfo;
Thread.CurrentThread.CurrentCulture = culinfo;

var supportedCultures = new List<CultureInfo> { };
var ar3 = new CultureInfo("en-US");
supportedCultures.Add(ar3);

var options = new RequestLocalizationOptions
{
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
     {
         new CookieRequestCultureProvider()
     }
};
app.UseRequestLocalization(options);

//���þ�̬Ŀ¼�������û�����http://url/Imageʱ��ʵ�ʷ��ʵ��� StaticFile/Image��
string DirPath = Path.Combine(AppContext.BaseDirectory, "Upload");
//string DirPath = Path.Combine(AppContext.BaseDirectory, "Upload", "Object", "images");
if (!Directory.Exists(DirPath)) Directory.CreateDirectory(DirPath);//����·��
app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(DirPath),   //ʵ��Ŀ¼��ַ
    RequestPath = new Microsoft.AspNetCore.Http.PathString("/Upload"),  //�û����ʵ�ַ
    EnableDirectoryBrowsing = false                                     //����Ŀ¼���
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

//app.UseRouting();
/*
 * ���� app.UseRouting �󣬽� Antiforgery �м����ӵ�������ܵ���
 * ����ж� app.UseRouting �� app.UseEndpoints �ĵ��ã���� app.UseAntiforgery �ĵ��ñ����������֮�䡣 
 * �� app.UseAntiforgery �ĵ��ñ����ڶ� app.UseAuthentication �� app.UseAuthorization �ĵ��ú󷢳���
*/

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(_PageRouteData.AdditionalPages); //ע��������ע������ҳ��

app.UseStatusCodePagesWithRedirects("/404");

/* Ocelotͨ��Ҫ�������м���ܵ�����ˣ��ᵼ��blazorҳ�浼��ʧЧ����������ҳ����תҲ�ᱻ��������ת�� */
app.MapWhen(context =>
{
    var internalConfigurationResponse = context.RequestServices.GetRequiredService<IInternalConfigurationRepository>().Get();
    if (internalConfigurationResponse.IsError || internalConfigurationResponse.Data.Routes.Count == 0)
    {
        return false;
    }

    var internalConfiguration = internalConfigurationResponse.Data;
    var downstreamRouteFinder = context.RequestServices.GetRequiredService<IDownstreamRouteProviderFactory>().Get(internalConfiguration);
    var upstreamHeaders = context.Request.Headers
                .ToDictionary(h => h.Key, h => string.Join(';', h.Value));
    var response = downstreamRouteFinder.Get(context.Request.Path, context.Request.QueryString.ToString(),
        context.Request.Method, internalConfiguration, context.Request.Host.ToString(), upstreamHeaders);
    return !response.IsError && !string.IsNullOrEmpty(response.Data?.Route?.DownstreamRoute?.FirstOrDefault()?.DownstreamScheme);
}, appBuilder => appBuilder.UseOcelot().Wait());

if (!app.Environment.IsDevelopment())
{
    try
    {
        System.Diagnostics.Process.Start("explorer.exe", "http://localhost:5000/");
    }
    catch (Exception)
    {

    }
}
app.MapHub<ChatHub>(Modules.Core.Blazor.SignalR.HubClient.HUBURL); //���SignalR

app.Run();

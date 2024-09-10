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
/* 全局变量配置 */
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

//用于解决  <Routes @rendermode="InteractiveServer" /> 模式下如果使用@attribute [Authorize] 导致页面刷新404找不到问题
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
#region 依赖注入
//显示详细错误
builder.Services.Configure<CircuitOptions>(options => { options.DetailedErrors = true; });

//配置signaIR消息的限制，默认32K，在JS调用截图在时候，大图片不能传递（也可通过分割图片多次传递）
builder.Services.Configure<HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024 * 10; // 10MB or use null ，null为不限制大小
});

builder.Services.AddLocalization(); //静态设置区域

//注册上下文：AOP里面可以获取IOC对象
builder.Services.AddHttpContextAccessor();

//builder.AddErrorLogConfigPostgre(); 记录错误日志
#endregion
#region 模块注入(核心注入要放在前面，后面的会覆盖前面的方法)
builder.Services.AddCoreModulesConfig(builder.Configuration); //核心模块
builder.Services.AddTaskModulesConfig(builder.Configuration); //任务模块
builder.Services.AddMesModulesConfig(builder.Configuration); //核心MES模块
builder.Services.AddCodeGeneratorModulesConfig(builder.Configuration); //代码生成模块
builder.Services.AddTemplateModulesConfig(builder.Configuration); //模板模块

builder.Services.AddERPModulesConfig(builder.Configuration); //CRM模块


#endregion
//开启就运行的服务(任务模块)
builder.Services.AddHostedService<QuartzHostedService>();
//使用AntDesign UI
builder.Services.AddAntDesign();

//使用SingaIR
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
       new[] { "application/octet-stream" });
});
builder.Services.AddScoped<HubClient>(); //注入SingalR服务

var app = builder.Build();
app.UseResponseCompression();
// 要放在最后，因为里面使用了BuildServiceProvider
HostService.RegisterHostServices(builder.Services);

//AntDesign切换当前线程的语言
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
//调整服务区时区
//app.UseRequestLocalization("zh-CN");
//全局格式化日期,
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

//设置静态目录，这样用户访问http://url/Image时，实际访问的是 StaticFile/Image。
string DirPath = Path.Combine(AppContext.BaseDirectory, "Upload");
//string DirPath = Path.Combine(AppContext.BaseDirectory, "Upload", "Object", "images");
if (!Directory.Exists(DirPath)) Directory.CreateDirectory(DirPath);//创建路径
app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(DirPath),   //实际目录地址
    RequestPath = new Microsoft.AspNetCore.Http.PathString("/Upload"),  //用户访问地址
    EnableDirectoryBrowsing = false                                     //开启目录浏览
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

//app.UseRouting();
/*
 * 调用 app.UseRouting 后，将 Antiforgery 中间件添加到请求处理管道。
 * 如果有对 app.UseRouting 和 app.UseEndpoints 的调用，则对 app.UseAntiforgery 的调用必须介于两者之间。 
 * 对 app.UseAntiforgery 的调用必须在对 app.UseAuthentication 和 app.UseAuthorization 的调用后发出。
*/

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(_PageRouteData.AdditionalPages); //注意在这里注册其他页面

app.UseStatusCodePagesWithRedirects("/404");

/* Ocelot通常要求阻塞中间件管道，因此，会导致blazor页面导航失效，即正常的页面跳转也会被当成请求转发 */
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
app.MapHub<ChatHub>(Modules.Core.Blazor.SignalR.HubClient.HUBURL); //添加SignalR

app.Run();

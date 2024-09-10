using Blazor_TestApp.Components;
using Blazored.LocalStorage;
using Modules.Core.AppServices;
using Modules.CRM.AppServices;
using Modules.MES.AppServices;
using Modules.Tasks.AppServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped(
                sp => new HttpClient { BaseAddress = new Uri(AppContext.BaseDirectory) });
#region 模块注入
builder.Services.AddCoreModulesConfig(builder.Configuration); //核心模块
builder.Services.AddTaskModulesConfig(builder.Configuration); //任务模块
builder.Services.AddMesModulesConfig(builder.Configuration); //核心MES模块
builder.Services.AddERPModulesConfig(builder.Configuration); //CRM模块


#endregion
//使用AntDesign UI
builder.Services.AddAntDesign();
//LocalStorage
builder.Services.AddBlazoredLocalStorage();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

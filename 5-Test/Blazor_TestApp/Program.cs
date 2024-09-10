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
#region ģ��ע��
builder.Services.AddCoreModulesConfig(builder.Configuration); //����ģ��
builder.Services.AddTaskModulesConfig(builder.Configuration); //����ģ��
builder.Services.AddMesModulesConfig(builder.Configuration); //����MESģ��
builder.Services.AddERPModulesConfig(builder.Configuration); //CRMģ��


#endregion
//ʹ��AntDesign UI
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

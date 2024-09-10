using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Blazor.Server.Components
{
    /// <summary>
    /// 用于解决  <Routes @rendermode="InteractiveServer" /> 模式下
    /// 如果使用@attribute [Authorize]  导致页面刷新404找不到问题
    /// https://github.com/dotnet/aspnetcore/issues/52317
    /// 使用方法：services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
    /// </summary>
    public class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            return next(context);
        }
    }
}

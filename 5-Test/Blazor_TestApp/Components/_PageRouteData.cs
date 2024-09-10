namespace Blazor_TestApp.Components
{
    public static class _PageRouteData
    {
        //附加Page的路由，使用AdditionalAssemblies 添加路由信息
        //public static System.Reflection.Assembly[] Pages = new[]
        //{
        //    typeof(Pages.Index).Assembly
        //}
        //.Concat(Modules.Core.Blazor._PageRoute.Pages) //合并模块
        //.Concat(Modules.Tasks.Blazor._PageRoute.Pages) //合并模块
        //.Concat(Modules.CodeGenerator.Blazor._PageRoute.Pages) //合并模块
        //.Concat(Modules.Template.Blazor._PageRoute.Pages) //合并模块
        //.Concat(Modules.CRM.Blazor._PageRoute.Pages) //合并模块
        //.Concat(Modules.ZXJD.Blazor._PageRoute.Pages) //合并模块
        //.ToArray();

        public static System.Reflection.Assembly[] AdditionalPages =
        [
            typeof(Modules.Core.Blazor._Imports).Assembly,
            // typeof(Modules.Mobile.Blazor._Imports).Assembly,

        ];
    }
}

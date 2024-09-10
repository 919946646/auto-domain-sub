using Microsoft.AspNetCore.Components;
using Modules.Core.AppServices.Base_sys_menu;
using Modules.Core.AppServices.Base_user_favorite;
using Modules.Core.Domain.DbEntity;
using Modules.Core.Shared.Base_sys_menu;
using Modules.MES.AppServices.Project;
using Yitter.IdGenerator;

namespace Blazor.Server.Pages.Workbench
{
    partial class Workbench
    {
        [Inject] private IBase_user_favoriteService _FavService { get; set; }
        [Inject] public IProjectService ProjectService { get; set; }
        List<Base_sys_menuVM> menus;
        List<Base_sys_menuVM> SysFavMenus = new List<Base_sys_menuVM>(); //系统推荐的4个菜单
        protected override async Task OnInitializedAsync()
        {
            InitSysFavMenus();
            await base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //组件呈现之后触发，通常用来做JavaScript的互操作，需要注意的是要加上对firstRender参数的判断
            if (firstRender)
            {
                var list = MenuTreeUtils.MenuTreeToList(_userService.MenuModel.UserTreeMenus);
                foreach (var item in list)
                {
                    //全局替换空图标
                    if (string.IsNullOrWhiteSpace(item.Icon)) item.Icon = "link";
                }
                var VmList = _mapper.Map<List<Base_sys_menuVM>>(list);
                menus = VmList.Where(s => s.Menu_type == "页面" && !string.IsNullOrWhiteSpace(s.Url)).ToList();

                //查找用户收藏的菜单
                if (!string.IsNullOrWhiteSpace(_userService.CurrentUser.Authname))
                {

                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void InitSysFavMenus()
        {
            //初始化用户收藏的菜单,默认添加4个
            this.SysFavMenus = new List<Base_sys_menuVM>{
                new Base_sys_menuVM() { Id = YitIdHelper.NextId().ToString(), Url = "", Name = "", Menu_target = "_blank", Icon = "link" },
                new Base_sys_menuVM() { Id = YitIdHelper.NextId().ToString(), Url = "", Name = "", Menu_target = "_blank", Icon = "link" },
                new Base_sys_menuVM() { Id = YitIdHelper.NextId().ToString(), Url = "", Name = "", Menu_target = "_blank", Icon = "link" },
                new Base_sys_menuVM() { Id = YitIdHelper.NextId().ToString(), Url = "", Name = "", Menu_target = "_blank", Icon = "link" }
                };
        }

        //添加菜单到收藏夹
        private async Task AddFavorite(Base_sys_menuVM menu)
        {
            var ID = YitIdHelper.NextId().ToString();
            var fav = new Base_user_favoriteEntity
            {
                id = ID,
                name = menu.Name,
                icon = menu.Icon,
                url = menu.Url,
                types = menu.Menu_target,
                module_id = "menu",
                key = "id",
                key_value = ID,
                userid = _userService.CurrentUser.Authname,
                createtime = DateTime.Now,
                createuid = _userService.CurrentUser.Authname,
            };
            var first = await _FavService.QueryFirstAsync(s => s.module_id == "menu" && s.url == menu.Url && s.userid == _userService.CurrentUser.Authname);
            if (first == null)
            {
                var ret = await _FavService.InsertAsync(fav);
                if (ret > 0) _ = _message.Success("成功添加到收藏夹。");
                else _ = _message.Error("添加到收藏夹失败。");
            }
            else
            {
                _ = _message.Info("已存在收藏内容。");
            }
        }
    }
}
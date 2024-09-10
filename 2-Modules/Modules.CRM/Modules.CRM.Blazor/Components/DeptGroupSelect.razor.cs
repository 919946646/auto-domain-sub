using AntDesign;
using Microsoft.AspNetCore.Components;

namespace Modules.CRM.Blazor.Components
{
    partial class DeptGroupSelect : AntInputComponentBase<string>
    {
        [Parameter] public EventCallback<string> OnSelectItemChanged { get; set; }
        private async Task OnSelectedItemChangedHandlerAsync(string value)
        {
            this.CurrentValue = value;
            //给父组件传递事件
            if (OnSelectItemChanged.HasDelegate)
            {
                await OnSelectItemChanged.InvokeAsync(value);
            }
        }
    }
}

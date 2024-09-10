﻿using AntDesign;
using Force.DeepCloner;
using Microsoft.AspNetCore.Components;
using Modules.Template.AppServices.CodeTemplate;
using Modules.Template.Shared.CodeTemplate;

namespace Modules.Template.Blazor.Pages.CodeTemplate.Components
{
    partial class CodeTemplateAdd : ComponentBase
    {
        //注入服务
        [Inject] private ICodeTemplatePageService _service { get; set; }
        [Inject] private IMessageService _message { get; set; }
        //组件之间传值
        [Parameter] public CodeTemplateVM model { get; set; }
        [Parameter] public bool IsAdd { get; set; }
        [Parameter] public EventCallback<CodeTemplateVM> OnValueCallback { get; set; }

        private bool btnLoading = false;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        private async Task SubmitForm()
        {
            btnLoading = true;
            if (IsAdd)
            {
                var ret = await _service.AddRowDataAsync(model.DeepClone());
                if (ret != null)
                {
                    model = ret;
                    _ = _message.Success("保存成功");
                }
                else
                {
                    _ = _message.Error("保存失败");
                }
            }
            else
            {
                var ret = await _service.UpdateRowDataAsync(model);
                _ = ret ? _message.Success("保存成功") : _message.Error("保存失败");
            }

            //给父组件传递参数
            await ReturnAsync(model);//返回
            btnLoading = false;
        }
        private async Task ReturnAsync(CodeTemplateVM ret)
        {
            //给父组件传递参数
            if (OnValueCallback.HasDelegate)
            {
                await OnValueCallback.InvokeAsync(ret);
            }
        }
    }
}
/**
 * C# 中使用方式
 * await JSRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(fileBytes));
 * 
 * 对于JS中的windows方法，可以直接调用
 * alert()、atob()、btoa()、blur()、close()、focus()、open()、print()、setInterval()、setTimeout()、stop()等等
 * JsRuntime.InvokeAsync<object>("open", href, href);
 */
//新窗口打开
window.NavigateTo = (url, target) => {
    const link = document.createElement('a');
    link.href = url;
    link.target = target; // '_blank';
    document.body.appendChild(link);
    link.click();
    link.remove();
}

//Blazor Animation组件，需要引用animate.min.css
var AnimatedComponent = AnimatedComponent || {};
AnimatedComponent.animationend = function (element, dotNet) {
    element.addEventListener('animationend', function (e) {
        if (e.target === this) dotNet.invokeMethodAsync("OnAnimationEnd")
    });
};
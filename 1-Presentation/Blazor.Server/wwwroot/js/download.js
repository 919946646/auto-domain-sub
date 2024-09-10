/**
 * C# 中使用方式
 * await JSRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(fileBytes));
 * 
 * 对于JS中的windows方法，可以直接调用
 * alert()、atob()、btoa()、blur()、close()、focus()、open()、print()、setInterval()、setTimeout()、stop()等等
 * JsRuntime.InvokeAsync<object>("open", href, href);
 */

window.saveAsFile = function (fileName, byteBase64) {
    var link = this.document.createElement('a');
    link.download = fileName;
    link.href = "data:application/octet-stream;base64," + byteBase64;
    this.document.body.appendChild(link);
    link.click();
    this.document.body.removeChild(link);
    //alert("Downloaded");
}
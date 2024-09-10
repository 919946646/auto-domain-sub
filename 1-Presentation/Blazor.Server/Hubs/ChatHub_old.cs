using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Blazor.Server.Hubs
{
    /*
     * https://learn.microsoft.com/zh-cn/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-8.0&tabs=visual-studio
     * 需要在Layout页面执行StartAsync
     * await hubConnection.StartAsync();
     * 
     * 使用IHubContext<ChatHub> _ChatHub进行注入
     */
    [Authorize]
    public class ChatHub_old : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public async Task InitializeUserList()
        {
            var list = _connections.GetAllUsers();

            await Clients.All.SendAsync("ReceiveInitializeUserList", list);
        }
        public async Task SendMessage(string userID, string message)
        {
            string name = Context.User.Identity.Name;//获取用户的名称

            if (string.IsNullOrEmpty(userID)) // If All selected
            {
                var users = _connections.GetAllUsers();

                foreach (var user in users)
                {
                    foreach (var connectionId in _connections.GetConnections(user))
                    {
                        await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.User.Identity.Name ?? "匿名", message);
                    }
                }
            }
            else
            {
                foreach (var connectionId in _connections.GetConnections(userID))
                {
                    await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage",
                                           Context.User.Identity.Name ?? "匿名", message);
                }
            }

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            string name = Context.User.Identity.Name;
            _connections.Remove(name, Context.ConnectionId);

            var list = _connections.GetAllUsers();

            await Clients.All.SendAsync("ReceiveInitializeUserList", list);

            await Clients.All.SendAsync("MessageBoard",
                        $"{Context.User.Identity.Name}  离线");

        }


        public override async Task OnConnectedAsync()
        {

            string name = Context.User.Identity.Name;

            _connections.Add(name, Context.ConnectionId);

            await Clients.All.SendAsync("MessageBoard", $"{Context.User.Identity.Name}  上线");

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveUserName", Context.User.Identity.Name);

            await Task.CompletedTask;
        }
    }
}

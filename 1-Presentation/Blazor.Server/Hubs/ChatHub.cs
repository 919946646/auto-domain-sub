using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Modules.Core.Blazor.SignalR;

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
    public class ChatHub : Hub
    {
        //一个用户ID，有多个ConnectionId
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        /// <summary>
        /// 给用户发消息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string SenderUser, string ReceiveUser, string message)
        {
            var UserConnectionIds = _connections.GetConnections(ReceiveUser);
            if (UserConnectionIds != null && UserConnectionIds.Count() > 0)
                await Clients.Clients(UserConnectionIds).SendAsync(HubMessages.RECEIVE, SenderUser, message);
        }

        //发送广播消息
        public async Task SendBroadcast(string SenderUser, string message)
        {
            await Clients.All.SendAsync(HubMessages.RECEIVE, SenderUser, message);
        }

        //取得在线用户
        public async Task OnlineUserList()
        {
            var onlineUserList = _connections.GetAllUsers();
            await Clients.All.SendAsync(HubMessages.ONLINE_USERLIST, onlineUserList);
        }

        //强制用户退出（用户在其他地方登录，管理员踢人)
        public async Task KickUserLogout(string username)
        {
            var UserConnectionIds = _connections.GetConnections(username);
            if (UserConnectionIds != null && UserConnectionIds.Count() > 0)
                await Clients.Clients(UserConnectionIds).SendAsync(HubMessages.KICKUSERLOGOUT, username);
        }
        /// <summary>
        /// 用户上线
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task Register(string username)
        {
            // 通知其他(此用户第一次打开页面)
            var conns = _connections.GetConnections(username);
            if (conns == null || (conns != null && conns.Count() == 0))
            {
                await Clients.All.SendAsync(HubMessages.USER_LOGIN, username, true); //true上线
            }
            //将当前用户添加到在线列表
            _connections.Add(username, Context.ConnectionId);
        }

        public override Task OnConnectedAsync()
        {
            //var name = Context.User.Identity.Name; 不使用jwt取得不了登录信息
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Log disconnection
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception e)
        {
            // 通知用户退出(如果用户没有其他连接)
            KeyValuePair<string, HashSet<string>>? user = _connections.GetUserByConnectionId(Context.ConnectionId);
            if (user != null)
            {
                var conns = user.Value.Value;
                if (conns != null && conns.Count == 1)
                {
                    await Clients.All.SendAsync(HubMessages.USER_LOGIN, user.Value.Key, false); //false 下线
                }
            }
            //移除这个连接
            _connections.Remove(Context.ConnectionId);

            await base.OnDisconnectedAsync(e);
        }
    }
}

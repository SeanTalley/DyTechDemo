using Microsoft.AspNetCore.SignalR;

namespace DemoAPI.Hubs;
public class ClientHub : Hub
{
    private static int ClientCount = 0;
    private static Dictionary<string,UserInfo> ClientList = new Dictionary<string, UserInfo>();
    public async override Task OnConnectedAsync()
    {
        int clientId = ++ClientCount;
        Console.WriteLine($"Client connected: {Context.ConnectionId} - assigned username User {clientId}");
        ClientList.Add(Context.ConnectionId, new UserInfo { UserName = $"User {clientId}" });
        await Clients.Client(Context.ConnectionId).SendAsync("UserName", $"User {clientId}");
        // Send the list of users to the new client
        await Clients.Client(Context.ConnectionId).SendAsync("UserList", ClientList.Values.Where(x => x.UserName != $"User {clientId}"));
        // Send the new user to all other clients
        await Clients.AllExcept(Context.ConnectionId).SendAsync("UserJoined", ClientList[Context.ConnectionId]);
        await base.OnConnectedAsync();
    }
    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        if(ClientList[Context.ConnectionId] is not null) {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("UserLeft", ClientList[Context.ConnectionId]);
            ClientList.Remove(Context.ConnectionId);
        }
        await base.OnDisconnectedAsync(exception);
    }
    public async Task MouseMove(int x, int y)
    {
        ClientList[Context.ConnectionId].CursorX = x;
        ClientList[Context.ConnectionId].CursorY = y;
        await Clients.AllExcept(Context.ConnectionId).SendAsync("MouseMove", ClientList[Context.ConnectionId]);
    }
    public async Task StartEditing(ClientInfo clientInfo) {
        await Clients.AllExcept(Context.ConnectionId).SendAsync("StartEditing", clientInfo, ClientList[Context.ConnectionId]);
    }
    public async Task StopEditing(ClientInfo clientInfo) {
        await Clients.AllExcept(Context.ConnectionId).SendAsync("StopEditing", clientInfo, ClientList[Context.ConnectionId]);
    }
    public async Task UpdateClient(ClientInfo clientInfo) {
        await Clients.AllExcept(Context.ConnectionId).SendAsync("UpdateClient", clientInfo, ClientList[Context.ConnectionId]);
    }
    public async Task DeleteClient(ClientInfo clientInfo) {
        await Clients.AllExcept(Context.ConnectionId).SendAsync("DeleteClient", clientInfo, ClientList[Context.ConnectionId]);
    }
    public async Task CreateClient(ClientInfo clientInfo) {
        await Clients.AllExcept(Context.ConnectionId).SendAsync("CreateClient", clientInfo, ClientList[Context.ConnectionId]);
    }
}
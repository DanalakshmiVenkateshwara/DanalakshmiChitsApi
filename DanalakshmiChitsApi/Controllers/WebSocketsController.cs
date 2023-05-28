using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
namespace DanalakshmiChitsApi.Controllers
{

[Route("api/[controller]")]
public class WebSocketController : ControllerBase
{
    private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

    [HttpGet]
     
    public async Task Connect()
    {
        var context = ControllerContext.HttpContext;
        if (context.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            string connectionId = Guid.NewGuid().ToString();
            _sockets.TryAdd(connectionId, webSocket);

            await SendConnectedMessage(connectionId);

            await ReceiveMessages(webSocket, connectionId);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }

    private async Task ReceiveMessages(WebSocket webSocket, string connectionId)
    {
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                await SendMessageToOthers(connectionId, message);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                _sockets.TryRemove(connectionId, out WebSocket removedSocket);
                await SendDisconnectedMessage(connectionId);
            }
        }
    }

    private async Task SendConnectedMessage(string connectionId)
    {
        string message = $"Client {connectionId} connected";
        await SendMessageToAll(message);
    }

    private async Task SendDisconnectedMessage(string connectionId)
    {
        string message = $"Client {connectionId} disconnected";
        await SendMessageToAll(message);
    }

    private async Task SendMessageToAll(string message)
    {
        foreach (var socket in _sockets)
        {
            if (socket.Value.State == WebSocketState.Open)
            {
                await socket.Value.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    private async Task SendMessageToClient(string connectionId, string message)
    {
        if (_sockets.TryGetValue(connectionId, out WebSocket socket))
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    private async Task SendMessageToOthers(string senderConnectionId, string message)
    {
        foreach (var socket in _sockets)
        {
            if (socket.Key != senderConnectionId && socket.Value.State == WebSocketState.Open)
            {
                await socket.Value.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
}
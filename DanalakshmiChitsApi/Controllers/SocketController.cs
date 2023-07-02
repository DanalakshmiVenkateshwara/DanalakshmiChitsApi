using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DanalakshmiChitsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebSocketController : ControllerBase
    {
        private static WebSocketConnectionManager _connectionManager = new WebSocketConnectionManager();

        public WebSocketController(WebSocketConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        [HttpGet]
        public async Task<IActionResult> Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var connectionId = _connectionManager.AddSocket(webSocket);

                await HandleWebSocketConnection(webSocket, connectionId);
            }
            else
            {
                return BadRequest();
            }

            return Ok();
        }

        private async Task HandleWebSocketConnection(WebSocket webSocket, string connectionId)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            // Handshake request
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (message == "{\"action\":\"connect\"}")
                {
                    // Send handshake response with connection ID
                    var response = new { action = "connectResponse", connectionId = connectionId };
                    await webSocket.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)),
                        WebSocketMessageType.Text, true, CancellationToken.None);

                    // Continue handling other messages
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
            }

            // Handle other messages (e.g., echo functionality)
            while (!result.CloseStatus.HasValue)
            {
                var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                // Handle the received message

                // Example: Send a signal to a specific client
                await _connectionManager.SendMessageToClient(connectionId, "Hello from the server!");

                // Example: Send a signal to all clients
                await _connectionManager.SendMessageToAllClients("New user connected!");

                // Echo the received message back to the client
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            // Clean up the connection
            _connectionManager.RemoveSocket(connectionId);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }

    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public string AddSocket(WebSocket socket)
        {
            string connectionId = Guid.NewGuid().ToString();
            _sockets.TryAdd(connectionId, socket);
            return connectionId;
        }

        public void RemoveSocket(string connectionId)
        {
            _sockets.TryRemove(connectionId, out _);
        }

        public async Task SendMessageToClient(string connectionId, string message)
        {
            if (_sockets.TryGetValue(connectionId, out var socket))
            {
                await socket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task SendMessageToAllClients(string message)
        {
            foreach (var socket in _sockets.Values)
            {
                await socket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}

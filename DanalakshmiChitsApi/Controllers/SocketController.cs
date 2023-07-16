using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace DanalakshmiChitsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebSocketController : ControllerBase
    {
        private static WebSocketConnectionManager _connectionManager = new WebSocketConnectionManager();
        
        [HttpGet]
        public async Task<IActionResult> Connect(string connectionId, string userDetails,DateTime socketCloseTime)
        {
            //_connectionManager.LoadDataFromStorage();
            //TriggerActionAtTime(socketCloseTime);
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var isNewConnection = (string.IsNullOrEmpty(connectionId) || connectionId == "undefined");
                //var userDetailsObject = JsonSerializer.Deserialize<UserDetails>(userDetails);
                //Console.WriteLine("User Details: " + userDetailsObject.Username);
              
                    connectionId = _connectionManager.AddSocket(webSocket, connectionId);

                    // Store the connection ID in local storage
                    // Note: You may need to modify this based on your React app's storage mechanism
                    // For example, if you're using React-Redux, you can dispatch an action to store the connection ID in the Redux store
                    // Here, we're using browser's localStorage for simplicity
                    //localStorage.setItem('connectionId', connectionId);
                   
                
                _connectionManager.AddConnectedClient(connectionId, userDetails);
                var response = new { Action = "connectResponse", connectionId = connectionId };
                await webSocket.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)),
                    WebSocketMessageType.Text, true, CancellationToken.None);

                // Send the list of connected clients to the new client
                var connectedClients = _connectionManager.GetConnectedClients();
                //await SendToClient(webSocket, "connectedClients", connectedClients);

               // if (isNewConnection)
              //  {
                    // Add the new client to the list of connected clients
                    //_connectionManager.AddConnectedClient(connectionId);
                   // var connectedClients = _connectionManager.GetConnectedClients();
                    // Broadcast the updated list of connected clients to all clients
                    await BroadcastToAll("connectedClients", connectedClients);
                var biddingDetails = _connectionManager.GetBiddings();
                await BroadcastToAll("biddingResponse", biddingDetails);
                //}
                //var connectedClients = _connectionManager.GetConnectedClients();
                // await SendToClient(webSocket, "connectedClients", connectedClients);

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

            while (!result.CloseStatus.HasValue)
            {
                // Handle WebSocket messages based on the received action
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var jsonMessage = JsonSerializer.Deserialize<WebSocketMessage>(message);

                switch (jsonMessage.Action)
                {
                    case "sendMessage":
                        // Process the received message (e.g., chat functionality)
                        var sender = _connectionManager.GetConnectedClient(connectionId);
                        var data = new { sender.User, message = jsonMessage.Data };
                        await BroadcastToAll("receiveMessage", data);
                        break;
                    case "connected":
                        var response = new { Action = "connectResponse", connectionId = connectionId };
                        await webSocket.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                        //var connectedClients = _connectionManager.GetConnectedClients();
                        //await SendToClient(webSocket, "connectedClients", connectedClients);
                        //// Continue handling other messages
                        //result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        break;
                    case "bidding": 
                        _connectionManager.AddBidding(connectionId, JsonSerializer.Serialize(jsonMessage.Data));
                        var biddingDetails = _connectionManager.GetBiddings();
                        //biddingDetails.ConnectionId = connectionId;
                        await BroadcastToAll("biddingResponse", biddingDetails);
                        break;
                    // Add more cases to handle other actions
                    default:
                        // Invalid action, ignore the message
                        break;
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            // Clean up the connection
            _connectionManager.RemoveSocket(connectionId);
            _connectionManager.RemoveConnectedClient(connectionId);

            // Broadcast the updated list of connected clients to all clients
            var connectedClients = _connectionManager.GetConnectedClients();
            await BroadcastToAll("connectedClients", connectedClients);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private async Task SendToClient(WebSocket webSocket, string action, object data)
        {
            var message = new WebSocketMessage { Action = action, Data = data };
            var jsonMessage = JsonSerializer.Serialize(message);
            await webSocket.SendAsync(Encoding.UTF8.GetBytes(jsonMessage), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task BroadcastToAll(string action, object data)
        {
            var message = new WebSocketMessage { Action = action, Data = data };
            var jsonMessage = JsonSerializer.Serialize(message);
            var buffer = Encoding.UTF8.GetBytes(jsonMessage);

            var sendTasks = new List<Task>();
            foreach (var socket in _connectionManager.GetAllSockets())
            {
                sendTasks.Add(socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None));
            }
            await Task.WhenAll(sendTasks);
        }

        [HttpPost("triggerAction")]
        public  async Task TriggerAction()
        {
            var auctionStatus = new { message = "Auction closed"};
            await BroadcastToAll("auctionResponse", auctionStatus);
            _connectionManager.RemoveAll();
        }

        //[HttpPost("trigger")]
        //public IActionResult TriggerActionAtTime(DateTime triggerTime)
        //{
        //    // Calculate the time until the desired trigger time
        //    TimeSpan timeUntilTrigger = triggerTime - DateTime.Now;

        //    // If the desired trigger time has already passed, return an error response
        //    if (timeUntilTrigger.TotalMilliseconds < 0)
        //    {
        //        return BadRequest("The trigger time has already passed.");
        //    }

        //    // Create a timer with the time until the desired trigger time
        //    System.Timers.Timer timer = new System.Timers.Timer(timeUntilTrigger.TotalMilliseconds);
        //    timer.Elapsed += async (sender, e) =>
        //    {
        //        // Stop the timer after the action is triggered
        //        timer.Stop();

        //        // Perform the desired action
        //        await TriggerAction();
        //    };
        //    timer.Start();

        //    return Ok("Action triggered at the specified time.");
        //}
    }

    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        private ConcurrentDictionary<string, ClientInfo> _connectedClients = new ConcurrentDictionary<string, ClientInfo>();
        private ConcurrentDictionary<string, BiddingDetails> _bidding = new ConcurrentDictionary<string, BiddingDetails>();

        public string AddSocket(WebSocket socket,string connectionId)
        {
            if ((string.IsNullOrEmpty(connectionId) || connectionId == "undefined"))
                
            {
              connectionId = Guid.NewGuid().ToString();
            }
            _sockets.TryAdd(connectionId, socket);
            return connectionId;
        }

        public void RemoveSocket(string connectionId)
        {
            _sockets.TryRemove(connectionId, out _);
        }

        public IEnumerable<WebSocket> GetAllSockets()
        {
            return _sockets.Values;
        }

        public void AddConnectedClient(string connectionId,string userDetails)
        {
            // Retrieve additional user details from the React app (e.g., username)
            // and store them along with the connection ID
            // Modify this logic based on how you retrieve user details from the React app

            var user = JsonSerializer.Deserialize<UserDetails>(userDetails);
            //Console.WriteLine("User Details: " + userDetailsObject.Username);

             var clientInfo = new ClientInfo { ConnectionId = connectionId, User = new UserDetails { Username = user.Username, Email = user.Email }, CreatedDate = DateTime.UtcNow };

            _connectedClients.TryAdd(connectionId, clientInfo);
        }

        public void RemoveConnectedClient(string connectionId)
        {
            _connectedClients.TryRemove(connectionId, out _);
        }

        public System.Collections.Generic.IEnumerable<ClientInfo> GetConnectedClients()
        {
            return _connectedClients.Values;
        }

        public ClientInfo GetConnectedClient(string connectionId)
        {
            _connectedClients.TryGetValue(connectionId, out var clientInfo);
            return clientInfo;
        }

        public void AddBidding(string connectionId, string Data )
        {
            var bid = JsonSerializer.Deserialize<BiddingDetails>(Data);
            var bids = new BiddingDetails { name = bid?.name, amount = bid?.amount, ConnectionId = connectionId,CreatedDate = DateTime.UtcNow };
            _bidding.TryAdd(Guid.NewGuid().ToString(), bids);
        }

        public IEnumerable<BiddingDetails> GetBiddings()
        {
           return _bidding.Values;
        }

        public void RemoveAll()
        {
            _sockets.Clear();
            _connectedClients.Clear();
            _bidding.Clear();
        }

      



       
        

       



    }

    public class ClientInfo
    {
        public string ConnectionId { get; set; }
        public UserDetails User { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class WebSocketMessage
    {
        public string Action { get; set; }
        public object Data { get; set; }
    }
    public class UserDetails
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
        public class BiddingDetails
    {
        public string name { get; set; }
        public string ConnectionId { get; set; }
        public string amount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

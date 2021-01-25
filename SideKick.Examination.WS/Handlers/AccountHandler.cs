using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SideKick.Examination.WS.Commands;
using SideKick.Examination.WS.Constants;
using SideKick.Examination.WS.Helper;
using SideKick.Examination.WS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Handlers
{
    public class AccountHandler : WebSocketHandler
    {
        private IAccountService _accountService;

        public AccountHandler(IAccountService accountService, ConnectionManager webSocketConnectionManager) :
            base(webSocketConnectionManager)
        {
            this._accountService = accountService;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var socketId = WebSocketConnectionManager.GetId(socket);
            await SendMessageAsync(socket,$"now connected to sidekick exam app");
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            var pload = Encoding.UTF8.GetString(buffer, 0, result.Count);

            var jsonSettings = new JsonSerializerSettings { Error = HandleDeserializationError, TypeNameHandling = TypeNameHandling.All };
            var jdata = JsonConvert.DeserializeObject<JData>(pload, jsonSettings);
           
            if (!string.IsNullOrWhiteSpace(jdata?.JsonData))
            {
                var com = JsonConvert.DeserializeObject<BaseCommand>(jdata.JsonData, jsonSettings);
                dynamic resMsg = null;
                switch (com.Command)
                {
                    case Cons.LoginSalt:
                        resMsg = await this._accountService.LoginSaltAsync(JsonConvert.DeserializeObject<LoginSaltCommand>(jdata.JsonData, jsonSettings));
                        break;
                    case Cons.Login:
                        resMsg = await this._accountService.LoginAsyncAsync(JsonConvert.DeserializeObject<LoginCommand>(jdata.JsonData, jsonSettings));
                        break;
                    case Cons.CheckEmail:
                        resMsg = await this._accountService.CheckEmailAsync(JsonConvert.DeserializeObject<CheckEmailCommand>(jdata.JsonData, jsonSettings));
                        break;
                    case Cons.CheckUsername:
                        resMsg = await this._accountService.CheckUserNameAsync(JsonConvert.DeserializeObject<CheckUsernameCommand>(jdata.JsonData, jsonSettings));
                        break;
                    case Cons.EmailVerification:
                        resMsg = await this._accountService.EmailVerificationAsync(JsonConvert.DeserializeObject<EmailVerificationCommand>(jdata.JsonData, jsonSettings));
                        break;
                    case Cons.Register:
                        resMsg = await this._accountService.RegisterAsync(JsonConvert.DeserializeObject<RegisterCommand>(jdata.JsonData, jsonSettings));
                        break;
                    default:
                        break;
                }

                await SendMessageAsync(socketId, JsonConvert.SerializeObject(new { JsonData = JsonConvert.SerializeObject(resMsg) }) );
            }
        }

        public void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }


    }
}

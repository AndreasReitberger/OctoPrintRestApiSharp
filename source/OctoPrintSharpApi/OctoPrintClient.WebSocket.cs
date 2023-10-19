using AndreasReitberger.API.OctoPrint.Enum;
using AndreasReitberger.API.OctoPrint.Models;
using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {

        #region WebSocket

        protected void Client_WebSocketMessageReceived(object sender, WebsocketEventArgs e)
        {
            try
            {
                if (e.Message == null)
                    return;
                if (e.Message.StartsWith("{\"connected\":"))
                {
                    OctoPrintWebSocketConnectionResponse response = JsonConvert.DeserializeObject<OctoPrintWebSocketConnectionResponse>(e.Message);
                }
                /*
                OnWebSocketMessageReceived(new WebsocketEventArgs()
                {
                    CallbackId = PingCounter,
                    Message = e.Message,
                    SessionId = SessionId,
                });
				*/
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = e.Message,
                    Message = jecx.Message,
                });
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        #endregion

    }
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {
        public class OctoPrintConnectionBuilder
        {
            #region Instance
            readonly OctoPrintClient _client = new();
            #endregion

            #region Methods

            public OctoPrintClient Build()
            {
                _client.Target = Print3dServer.Core.Enums.Print3dServerTarget.OctoPrint;
                return _client;
            }

            public OctoPrintConnectionBuilder WithServerAddress(string serverAddress, string version = "")
            {
                _client.ApiTargetPath = serverAddress;
                _client.ApiVersion = version;
                return this;
            }

            public OctoPrintConnectionBuilder WithApiKey(string apiKey)
            {
                _client.ApiKey = apiKey;
                return this;
            }

            public OctoPrintConnectionBuilder WithName(string name)
            {
                _client.ServerName = name;
                return this;
            }


            public OctoPrintConnectionBuilder WithPingInterval(bool enablePing, int interval = 5)
            {
                _client.EnablePing = enablePing;
                _client.PingInterval = interval;
                return this;
            }

            public OctoPrintConnectionBuilder WithTimeout(int timeout = 100)
            {
                _client.DefaultTimeout = timeout;
                return this;
            }
            public OctoPrintConnectionBuilder WithWebSocket(string websocketTarget, string pingCommand = "", int pingInterval = 5, bool enablePing = true)
            {
                _client.WebSocketTargetUri = websocketTarget;
                _client.PingCommand = pingCommand;
                _client.PingInterval = pingInterval;
                _client.EnablePing = enablePing;
                return this;
            }
            #endregion
        }
    }
}

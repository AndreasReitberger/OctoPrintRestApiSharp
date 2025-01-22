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

            public OctoPrintConnectionBuilder WithServerAddress(string serverAddress, int port = 3344, bool https = false)
            {
                _client.IsSecure = https;
                _client.ServerAddress = serverAddress;
                _client.Port = port;
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

            #endregion
        }
    }
}

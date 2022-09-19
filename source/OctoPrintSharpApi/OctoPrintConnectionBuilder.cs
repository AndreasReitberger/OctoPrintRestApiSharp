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
                _client.API = apiKey;
                return this;
            }

            #endregion
        }
    }
}

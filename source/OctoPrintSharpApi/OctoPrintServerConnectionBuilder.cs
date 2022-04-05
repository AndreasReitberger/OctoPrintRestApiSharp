namespace AndreasReitberger
{
    public partial class OctoPrintServer
    {
        public class OctoPrintServerConnectionBuilder
        {
            #region Instance
            readonly OctoPrintServer _client = new();
            #endregion

            #region Methods

            public OctoPrintServer Build()
            {
                return _client;
            }

            public OctoPrintServerConnectionBuilder WithServerAddress(string serverAddress, int port = 3344, bool https = false)
            {
                _client.IsSecure = https;
                _client.ServerAddress = serverAddress;
                _client.Port = port;
                return this;
            }

            public OctoPrintServerConnectionBuilder WithApiKey(string apiKey)
            {
                _client.API = apiKey;
                return this;
            }

            #endregion
        }
    }
}

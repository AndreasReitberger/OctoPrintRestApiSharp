using AndreasReitberger.API.OctoPrint.Enum;
using AndreasReitberger.API.OctoPrint.Interfaces;
using AndreasReitberger.API.OctoPrint.Models;
using AndreasReitberger.Core.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebSocket4Net;
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;


namespace AndreasReitberger.API.OctoPrint
{
    //http://docs.octoprint.org/en/master/api/
    [ObservableObject]
    public partial class OctoPrintClient : IPrintServerClient
    {
        #region Variables
        RestClient restClient;
        HttpClient httpClient;
        int _retries = 0;
        #endregion

        #region Id
        [JsonProperty(nameof(Id))]
        Guid _id = Guid.Empty;
        [JsonIgnore]
        public Guid Id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Instance
        static OctoPrintClient _instance = null;
        static readonly object Lock = new();
        public static OctoPrintClient Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                        _instance = new OctoPrintClient();
                }
                return _instance;
            }

            set
            {
                if (_instance == value) return;
                lock (Lock)
                {
                    _instance = value;
                }
            }

        }

        [ObservableProperty]
        bool _isActive = false;


        bool _updateInstance = false;
        public bool UpdateInstance
        {
            get => _updateInstance;
            set
            {
                if (_updateInstance == value)
                    return;
                _updateInstance = value;
                // Update the instance to the latest settings
                if (_updateInstance)
                    InitInstance(this.ServerAddress, this.ApiKey, this.Port, this.IsSecure);

                OnPropertyChanged();
            }
        }
        [ObservableProperty]

        bool _isInitialized = false;

        #endregion

        #region RefreshTimer
        // For now, this fails XML / JSON serialization

        [ObservableProperty]
        [property: JsonIgnore]
        [property: XmlIgnore]
        Timer _timer;

        [JsonProperty(nameof(RefreshInterval))]
        int _refreshInterval = 3;
        [JsonIgnore]
        public int RefreshInterval
        {
            get => _refreshInterval;
            set
            {
                if (_refreshInterval == value) return;
                _refreshInterval = value;
                if (IsListening)
                {
                    StopListening();
                    StartListening();
                }
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        bool _isListening = false;
        [JsonIgnore, XmlIgnore]
        public bool IsListening
        {
            get => _isListening;
            set
            {
                if (_isListening == value) return;
                _isListening = value;
                OnListeningChanged(new OctoPrintEventListeningChangedEventArgs()
                {
                    SessonId = SessionId,
                    IsListening = value,
                    IsListeningToWebSocket = IsListeningToWebSocket,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        [ObservableProperty]
        bool _initialDataFetched = false;

        #endregion

        #region Properties

        #region Connection

        [JsonIgnore, XmlIgnore]
        [ObservableProperty]
        string _sessionId = string.Empty;

        //[JsonProperty(nameof(ServerName))]
        [ObservableProperty]

        string _serverName = string.Empty;

        //[JsonProperty(nameof(ServerAddress))]
        [ObservableProperty]
        string _serverAddress = string.Empty;

        //[JsonProperty(nameof(IsSecure))]
        [ObservableProperty]
        bool _isSecure = false;

        //[JsonProperty(nameof(ApiKey))]
        [ObservableProperty]
        string _apiKey = string.Empty;

        //[JsonProperty(nameof(Port))]
        [ObservableProperty]
        int _port = 80;

        //[JsonProperty(nameof(DefaultTimeout))]
        [ObservableProperty]
        int _defaultTimeout = 10000;

        //[JsonProperty(nameof(OverrideValidationRules))]
        //[XmlAttribute(nameof(OverrideValidationRules))]
        [ObservableProperty]
        bool _overrideValidationRules = false;

        [JsonProperty(nameof(IsOnline))]
        [XmlAttribute(nameof(IsOnline))]
        bool _isOnline = false;
        [JsonIgnore, XmlIgnore]
        public bool IsOnline
        {
            get => _isOnline;
            set
            {
                if (_isOnline == value) return;
                _isOnline = value;
                // Notify subscribres 
                if (IsOnline)
                {
                    OnServerWentOnline(new OctoPrintEventArgs()
                    {
                        SessonId = SessionId,
                    });
                }
                else
                {
                    OnServerWentOffline(new OctoPrintEventArgs()
                    {
                        SessonId = SessionId,
                    });
                }
                OnPropertyChanged();
            }
        }

        //[JsonProperty(nameof(IsConnecting))]
        //[XmlAttribute(nameof(IsConnecting))]
        [ObservableProperty]

        bool _isConnecting = false;

        //[JsonProperty(nameof(AuthenticationFailed))]
        //[XmlAttribute(nameof(AuthenticationFailed))]
        [ObservableProperty]
        bool _authenticationFailed = false;

        //[JsonProperty(nameof(IsRefreshing))]
        //[XmlAttribute(nameof(IsRefreshing))]
        [ObservableProperty]
        bool _isRefreshing = false;

        //[JsonProperty(nameof(RetriesWhenOffline))]
        //[XmlAttribute(nameof(RetriesWhenOffline))]
        [ObservableProperty]
        int _retriesWhenOffline = 2;
        #endregion

        #region General
        [JsonIgnore, XmlIgnore]
        bool _updateAvailable = false;
        [JsonIgnore, XmlIgnore]
        public bool UpdateAvailable
        {
            get => _updateAvailable;
            private set
            {
                if (_updateAvailable == value) return;
                _updateAvailable = value;
                if (_updateAvailable)
                    // Notify on update available
                    OnServerUpdateAvailable(new OctoPrintEventArgs()
                    {
                        SessonId = this.SessionId,
                    });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        [ObservableProperty]
        object _update;

        #endregion

        #region Proxy
        [JsonProperty(nameof(EnableProxy))]
        [XmlAttribute(nameof(EnableProxy))]
        bool _enableProxy = false;
        [JsonIgnore, XmlIgnore]
        public bool EnableProxy
        {
            get => _enableProxy;
            set
            {
                if (_enableProxy == value) return;
                _enableProxy = value;
                OnPropertyChanged();
                UpdateRestClientInstance();
            }
        }

        [JsonProperty(nameof(ProxyUseDefaultCredentials))]
        [XmlAttribute(nameof(ProxyUseDefaultCredentials))]
        bool _proxyUseDefaultCredentials = true;
        [JsonIgnore, XmlIgnore]
        public bool ProxyUseDefaultCredentials
        {
            get => _proxyUseDefaultCredentials;
            set
            {
                if (_proxyUseDefaultCredentials == value) return;
                _proxyUseDefaultCredentials = value;
                OnPropertyChanged();
                UpdateRestClientInstance();
            }
        }

        //[JsonProperty(nameof(SecureProxyConnection))]
        //[XmlAttribute(nameof(SecureProxyConnection))]
        [ObservableProperty]
        bool _secureProxyConnection = true;

        //[JsonProperty(nameof(ProxyAddress))]
        //[XmlAttribute(nameof(ProxyAddress))]
        [ObservableProperty]
        string _proxyAddress = string.Empty;

        //[JsonProperty(nameof(ProxyPort))]
        //[XmlAttribute(nameof(ProxyPort))]
        [ObservableProperty]
        int _proxyPort = 443;

        //[JsonProperty(nameof(ProxyUser))]
        //[XmlAttribute(nameof(ProxyUser))]
        [ObservableProperty]
        string _proxyUser = string.Empty;


        //[JsonProperty(nameof(ProxyPassword))]
        //[XmlAttribute(nameof(ProxyPassword))]
        [ObservableProperty]
        SecureString _proxyPassword;

        #endregion

        #region DiskSpace
        //[JsonProperty(nameof(FreeDiskSpace))]
        [ObservableProperty]
        long _freeDiskSpace = 0;

        //[JsonProperty(nameof(TotalDiskSpace))]
        [ObservableProperty]
        long _totalDiskSpace = 0;

        #endregion

        #region PrinterStateInformation
        //[JsonProperty(nameof(LastFlowRate))]
        [ObservableProperty]
        double _lastFlowRate = 100;

        //[JsonProperty(nameof(LastFeedRate))]
        [ObservableProperty]
        double _lastFeedRate = 100;

        //[JsonProperty(nameof(CurrentFileLocation))]
        [ObservableProperty]
        OctoPrintFileLocations _currentFileLocation = OctoPrintFileLocations.local;
        #endregion

        #region Printers
        [JsonIgnore, XmlIgnore]
        OctoPrintPrinter _activePrinter;
        [JsonIgnore, XmlIgnore]
        public OctoPrintPrinter ActivePrinter
        {
            get => _activePrinter;
            set
            {
                if (_activePrinter == value) return;
                OnActivePrinterChanged(new OctoPrintActivePrinterChangedEventArgs()
                {
                    SessonId = SessionId,
                    NewPrinter = value,
                    OldPrinter = _activePrinter,
                    Printer = GetActivePrinterSlug(),
                });
                _activePrinter = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        ObservableCollection<OctoPrintPrinter> _printers = new();
        [JsonIgnore, XmlIgnore]
        public ObservableCollection<OctoPrintPrinter> Printers
        {
            get => _printers;
            set
            {
                if (_printers == value) return;
                _printers = value;
                if (_printers != null && _printers.Count > 0)
                {
                    if (ActivePrinter == null)
                        ActivePrinter = _printers[0];
                }
                OnPropertyChanged();
            }
        }
        #endregion

        #region State & Config
        [JsonIgnore, XmlIgnore]
        OctoPrintSettings _config;
        [JsonIgnore, XmlIgnore]
        public OctoPrintSettings Config
        {
            get => _config;
            set
            {
                if (_config == value) return;
                _config = value;
                OnOctoPrintPrinterConfigChanged(new OctoPrintPrinterConfigChangedEventArgs()
                {
                    NewConfiguration = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                    Printer = ActivePrinter != null ? ActivePrinter.Id : ""
                });
                UpdatePrinterConfig(value);
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        OctoPrintConnectionSettings _connectionSettings;
        [JsonIgnore, XmlIgnore]
        public OctoPrintConnectionSettings ConnectionSettings
        {
            get => _connectionSettings;
            set
            {
                if (_connectionSettings == value) return;
                _connectionSettings = value;
                OnOctoPrintConnectionSettingsChanged(new OctoPrintConnectionSettingsChangedEventArgs()
                {
                    NewConnectionSettings = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                    Printer = ActivePrinter != null ? ActivePrinter.Id : ""
                });
                UpdateConnectionSettings(value);
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        OctoPrintPrinterState _state;
        [JsonIgnore, XmlIgnore]
        public OctoPrintPrinterState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                OnOctoPrintPrinterStateChanged(new OctoPrintPrinterStateChangedEventArgs()
                {
                    NewPrinterState = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                    Printer = ActivePrinter != null ? ActivePrinter.Id : ""
                });
                UpdatePrinterState(value);
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        OctoPrintJobInfo _activePrintInfo;
        [JsonIgnore, XmlIgnore]
        public OctoPrintJobInfo ActivePrintInfo
        {
            get => _activePrintInfo;
            set
            {
                if (_activePrintInfo == value) return;
                _activePrintInfo = value;
                OnPrintInfoChanged(new OctoPrintActivePrintInfoChangedEventArgs()
                {
                    SessonId = SessionId,
                    NewActivePrintInfo = value,
                    Printer = GetActivePrinterSlug(),
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        [ObservableProperty]

        ObservableCollection<OctoPrintPrinterStateTemperatureInfo> _extruders = new();

        [JsonIgnore, XmlIgnore]
        [ObservableProperty]

        ObservableCollection<OctoPrintPrinterStateTemperatureInfo> _heatedBeds = new();

        [JsonIgnore, XmlIgnore]
        [ObservableProperty]

        ObservableCollection<OctoPrintPrinterStateTemperatureInfo> _heatedChambers = new();
        #endregion

        #region Models
        [JsonIgnore, XmlIgnore]
        ObservableCollection<OctoPrintModel> _models = new();
        [JsonIgnore, XmlIgnore]
        public ObservableCollection<OctoPrintModel> Models
        {
            get => _models;
            set
            {
                if (_models == value) return;
                _models = value;
                OnOctoPrintModelsChanged(new OctoPrintModelsChangedEventArgs()
                {
                    NewModels = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                    Printer = ActivePrinter != null ? ActivePrinter.Id : ""
                });
                OnPropertyChanged();
            }
        }
        #endregion

        #region ReadOnly
        public string FullWebAddress
        {
            //get => string.Format("{0}{1}:{2}", httpProtocol, ServerAddress, Port);
            get => $"{(IsSecure ? "https" : "http")}://{ServerAddress}:{Port}";
        }

        public bool IsReady
        {
            get
            {
                return (
                    !string.IsNullOrEmpty(ServerAddress) && !string.IsNullOrEmpty(ApiKey)) && Port > 0 &&
                    (
                        // Address
                        (Regex.IsMatch(ServerAddress, RegexHelper.IPv4AddressRegex) || Regex.IsMatch(ServerAddress, RegexHelper.IPv6AddressRegex) || Regex.IsMatch(ServerAddress, RegexHelper.Fqdn)) &&
                        // API-Key
                        (Regex.IsMatch(ApiKey, RegexHelper.OctoPrintApiKey))
                    ||
                        // Or validation rules are overriden
                        OverrideValidationRules
                    )
                    ;
            }
        }
        #endregion

        #endregion

        #region WebSocket

        [ObservableProperty]
        [property: JsonIgnore]
        [property: XmlIgnore]
        WebSocket _webSocket;

        [ObservableProperty]
        [property: JsonIgnore]
        [property: XmlIgnore]
        Timer _pingTimer;

        [JsonIgnore, XmlIgnore]
        [ObservableProperty]

        int _pingCounter = 0;

        [JsonIgnore, XmlIgnore]
        [ObservableProperty]

        int _refreshCounter = 0;

        [JsonIgnore, XmlIgnore]
        [ObservableProperty]

        bool _isListeningToWebSocket = false;

        #endregion

        #region Constructor
        public OctoPrintClient()
        {
            Id = Guid.NewGuid();
            UpdateRestClientInstance();
        }
        public OctoPrintClient(string serverAddress, string api, int port = 80, bool isSecure = false)
        {
            Id = Guid.NewGuid();
            InitInstance(serverAddress, api, port, isSecure);
            UpdateRestClientInstance();
        }
        #endregion

        #region Destructor
        ~OctoPrintClient()
        {
            if (WebSocket != null)
            {
                /* SharpWebSocket
                if (WebSocket.ReadyState == WebSocketState.Open)
                    WebSocket.Close();
                WebSocket = null;
                */
            }
        }
        #endregion

        #region Init
        public void InitInstance()
        {
            try
            {
                Instance = this;
                if (Instance != null)
                {
                    Instance.UpdateInstance = false;
                    Instance.IsInitialized = true;
                }
                UpdateInstance = false;
                IsInitialized = true;
            }
            catch (Exception exc)
            {
                //UpdateInstance = true;
                OnError(new UnhandledExceptionEventArgs(exc, false));
                IsInitialized = false;
            }
        }
        public static void UpdateSingleInstance(OctoPrintClient Inst)
        {
            try
            {
                Instance = Inst;
            }
            catch (Exception)
            {
                //OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public void InitInstance(string serverAddress, string api, int port = 3344, bool isSecure = false)
        {
            try
            {
                ServerAddress = serverAddress;
                ApiKey = api;
                Port = port;
                IsSecure = isSecure;

                Instance = this;

                if (Instance != null)
                {
                    Instance.UpdateInstance = false;
                    Instance.IsInitialized = true;
                }
                UpdateInstance = false;
                IsInitialized = true;
            }
            catch (Exception exc)
            {
                //UpdateInstance = true;
                OnError(new UnhandledExceptionEventArgs(exc, false));
                IsInitialized = false;
            }
        }
        #endregion

        #region EventHandlerss

        #region WebSocket

        public event EventHandler<OctoPrintEventArgs> WebSocketConnected;
        protected virtual void OnWebSocketConnected(OctoPrintEventArgs e)
        {
            WebSocketConnected?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintEventArgs> WebSocketDisconnected;
        protected virtual void OnWebSocketDisconnected(OctoPrintEventArgs e)
        {
            WebSocketDisconnected?.Invoke(this, e);
        }

        public event EventHandler<ErrorEventArgs> WebSocketError;
        protected virtual void OnWebSocketError(ErrorEventArgs e)
        {
            WebSocketError?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintEventArgs> WebSocketDataReceived;
        protected virtual void OnWebSocketDataReceived(OctoPrintEventArgs e)
        {
            WebSocketDataReceived?.Invoke(this, e);
        }

        #endregion

        #region ServerConnectionState

        public event EventHandler<OctoPrintEventArgs> ServerWentOffline;
        protected virtual void OnServerWentOffline(OctoPrintEventArgs e)
        {
            ServerWentOffline?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintEventArgs> ServerWentOnline;
        protected virtual void OnServerWentOnline(OctoPrintEventArgs e)
        {
            ServerWentOnline?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintEventArgs> ServerUpdateAvailable;
        protected virtual void OnServerUpdateAvailable(OctoPrintEventArgs e)
        {
            ServerUpdateAvailable?.Invoke(this, e);
        }
        #endregion

        #region Errors

        public event EventHandler Error;
        protected virtual void OnError()
        {
            Error?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnError(ErrorEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        protected virtual void OnError(UnhandledExceptionEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        protected virtual void OnError(OctoPrintJsonConvertEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        public event EventHandler<OctoPrintRestEventArgs> RestApiError;

        protected virtual void OnRestApiError(OctoPrintRestEventArgs e)
        {
            RestApiError?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintRestEventArgs> RestApiAuthenticationError;
        protected virtual void OnRestApiAuthenticationError(OctoPrintRestEventArgs e)
        {
            RestApiAuthenticationError?.Invoke(this, e);
        }
        public event EventHandler<OctoPrintRestEventArgs> RestApiAuthenticationSucceeded;
        protected virtual void OnRestApiAuthenticationSucceeded(OctoPrintRestEventArgs e)
        {
            RestApiAuthenticationSucceeded?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintJsonConvertEventArgs> RestJsonConvertError;
        protected virtual void OnRestJsonConvertError(OctoPrintJsonConvertEventArgs e)
        {
            RestJsonConvertError?.Invoke(this, e);
        }

        #endregion

        #region ServerStateChanges

        public event EventHandler<OctoPrintEventListeningChangedEventArgs> ListeningChanged;
        protected virtual void OnListeningChanged(OctoPrintEventListeningChangedEventArgs e)
        {
            ListeningChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintEventSessionChangedEventArgs> SessionChanged;
        protected virtual void OnSessionChanged(OctoPrintEventSessionChangedEventArgs e)
        {
            SessionChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintMessagesChangedEventArgs> MessagesChanged;
        protected virtual void OnMessagesChanged(OctoPrintMessagesChangedEventArgs e)
        {
            MessagesChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintActivePrintInfosChangedEventArgs> PrintInfosChanged;
        protected virtual void OnPrintInfosChanged(OctoPrintActivePrintInfosChangedEventArgs e)
        {
            PrintInfosChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintActivePrintInfoChangedEventArgs> PrintInfoChanged;
        protected virtual void OnPrintInfoChanged(OctoPrintActivePrintInfoChangedEventArgs e)
        {
            PrintInfoChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintJobsChangedEventArgs> JobsChanged;
        protected virtual void OnJobsChanged(OctoPrintJobsChangedEventArgs e)
        {
            JobsChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintJobFinishedEventArgs> JobFinished;
        protected virtual void OnJobFinished(OctoPrintJobFinishedEventArgs e)
        {
            JobFinished?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintTempDataEventArgs> TempDataReceived;
        protected virtual void OnTempDataReceived(OctoPrintTempDataEventArgs e)
        {
            TempDataReceived?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintPrinterConfigChangedEventArgs> OctoPrintPrinterConfigChanged;
        protected virtual void OnOctoPrintPrinterConfigChanged(OctoPrintPrinterConfigChangedEventArgs e)
        {
            OctoPrintPrinterConfigChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintPrinterStateChangedEventArgs> OctoPrintPrinterStateChanged;
        protected virtual void OnOctoPrintPrinterStateChanged(OctoPrintPrinterStateChangedEventArgs e)
        {
            OctoPrintPrinterStateChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintConnectionSettingsChangedEventArgs> OctoPrintConnectionSettingsChanged;
        protected virtual void OnOctoPrintConnectionSettingsChanged(OctoPrintConnectionSettingsChangedEventArgs e)
        {
            OctoPrintConnectionSettingsChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintModelsChangedEventArgs> OctoPrintModelsChanged;
        protected virtual void OnOctoPrintModelsChanged(OctoPrintModelsChangedEventArgs e)
        {
            OctoPrintModelsChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintModelGroupsChangedEventArgs> OctoPrintModelGroupsChanged;
        protected virtual void OnOctoPrintModelGroupsChanged(OctoPrintModelGroupsChangedEventArgs e)
        {
            OctoPrintModelGroupsChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintActivePrinterChangedEventArgs> ActivePrinterChanged;
        protected virtual void OnActivePrinterChanged(OctoPrintActivePrinterChangedEventArgs e)
        {
            ActivePrinterChanged?.Invoke(this, e);
        }
        #endregion

        #endregion

        #region WebSocket
        public void ConnectWebSocket()
        {
            try
            {
                if (!IsReady) return;
                //if (!IsReady || IsListeningToWebSocket) return;

                DisconnectWebSocket();

                string target = $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/sockjs/websocket?t={ApiKey}";
                WebSocket = new WebSocket(target)
                {
                    EnableAutoSendPing = false
                };

                if (IsSecure)
                {
                    // https://github.com/sta/websocket-sharp/issues/219#issuecomment-453535816
                    var sslProtocolHack = (SslProtocols)(SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
                    //Avoid TlsHandshakeFailure
                    if (WebSocket.Security.EnabledSslProtocols != sslProtocolHack)
                    {
                        WebSocket.Security.EnabledSslProtocols = sslProtocolHack;
                    }
                }

                WebSocket.MessageReceived += WebSocket_MessageReceived;
                //WebSocket.DataReceived += WebSocket_DataReceived;
                WebSocket.Opened += WebSocket_Opened;
                WebSocket.Closed += WebSocket_Closed;
                WebSocket.Error += WebSocket_Error;

#if NETSTANDARD
                WebSocket.OpenAsync();
#else
                WebSocket.Open();
#endif

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public void DisconnectWebSocket()
        {
            try
            {
                if (WebSocket != null)
                {
                    if (WebSocket.State == WebSocketState.Open)
#if NETSTANDARD
                        WebSocket.CloseAsync();
#else
                        WebSocket.Close();
#endif
                    StopPingTimer();

                    WebSocket.MessageReceived -= WebSocket_MessageReceived;
                    //WebSocket.DataReceived -= WebSocket_DataReceived;
                    WebSocket.Opened -= WebSocket_Opened;
                    WebSocket.Closed -= WebSocket_Closed;
                    WebSocket.Error -= WebSocket_Error;

                    WebSocket = null;
                }
                //WebSocket = null;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        private void WebSocket_Error(object sender, ErrorEventArgs e)
        {
            IsListeningToWebSocket = false;
            OnWebSocketError(e);
            OnError(e);
        }

        private void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                if (e.Message == null)
                    return;
                if (e.Message.StartsWith("{\"connected\":"))
                {
                    OctoPrintWebSocketConnectionRespone respone = JsonConvert.DeserializeObject<OctoPrintWebSocketConnectionRespone>(e.Message);
                }

                OnWebSocketDataReceived(new OctoPrintEventArgs()
                {
                    CallbackId = PingCounter,
                    Message = e.Message,
                    //SessonId = this.SessionId,
                });
            }
            catch (JsonException jecx)
            {
                OnError(new OctoPrintJsonConvertEventArgs()
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

        private void WebSocket_Closed(object sender, EventArgs e)
        {
            IsListeningToWebSocket = false;
            StopPingTimer();
            OnWebSocketDisconnected(new OctoPrintEventArgs()
            {
                Message = $"WebSocket connection to {WebSocket} closed. Connection state while closing was '{(IsOnline ? "online" : "offline")}'",
                Printer = GetActivePrinterSlug(),
            });
        }

        private void WebSocket_Opened(object sender, EventArgs e)
        {
            // Send a ping to the server to keep the connection alive (max 5s)
            // Not needed with octoprint?
            // PingTimer = new Timer((action) => PingServer(), null, 0, 2500);

            IsListeningToWebSocket = true;
            OnWebSocketConnected(new OctoPrintEventArgs()
            {
                Message = $"WebSocket connection to {WebSocket} established. Connection state while opening was '{(IsOnline ? "online" : "offline")}'",
                Printer = GetActivePrinterSlug(),
            });
        }

        private void WebSocket_DataReceived(object sender, DataReceivedEventArgs e)
        {

        }
        #endregion

        #region Methods

        #region Private

        #region ValidateResult

        bool GetQueryResult(string result, bool EmptyResultIsValid = false)
        {
            try
            {
                if (string.IsNullOrEmpty(result) && EmptyResultIsValid)
                    return true;
                var actionResult = JsonConvert.DeserializeObject<OctoPrintActionResult>(result);
                if (actionResult != null)
                    return actionResult.Ok;
                else
                    return false;
            }
            catch (JsonException jecx)
            {
                OnError(new OctoPrintJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result,
                    Message = jecx.Message,
                });
                return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        OctoPrintApiRequestRespone ValidateRespone(RestResponse respone, Uri targetUri)
        {
            OctoPrintApiRequestRespone apiRsponeResult = new() { IsOnline = IsOnline };
            try
            {
                if ((
                    respone.StatusCode == HttpStatusCode.OK || respone.StatusCode == HttpStatusCode.NoContent) &&
                    respone.ResponseStatus == ResponseStatus.Completed)
                {
                    apiRsponeResult.IsOnline = true;
                    AuthenticationFailed = false;
                    apiRsponeResult.Result = respone.Content;
                    apiRsponeResult.Succeeded = true;
                    apiRsponeResult.EventArgs = new OctoPrintRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    };
                }
                else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation
                    || respone.StatusCode == HttpStatusCode.Forbidden
                    || respone.StatusCode == HttpStatusCode.Unauthorized
                    )
                {
                    apiRsponeResult.IsOnline = true;
                    apiRsponeResult.HasAuthenticationError = true;
                    apiRsponeResult.EventArgs = new OctoPrintRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    };
                }
                else if (respone.StatusCode == HttpStatusCode.Conflict)
                {
                    apiRsponeResult.IsOnline = true;
                    apiRsponeResult.HasAuthenticationError = false;
                    apiRsponeResult.EventArgs = new OctoPrintRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    };
                }
                else
                {
                    OnRestApiError(new OctoPrintRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    });
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }
        #endregion

        #region ValidateActivePrinter
        string GetActivePrinterSlug()
        {
            try
            {
                if (!this.IsReady || ActivePrinter == null)
                {
                    return string.Empty;
                }
                var currentPrinter = ActivePrinter.Id;
                return currentPrinter;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return string.Empty;
            }
        }
        #endregion

        #region RestApi

        async Task<OctoPrintApiRequestRespone> SendRestApiRequestAsync(
            OctoPrintCommandBase commandBase,
            Method method,
            string command,
            object jsonObject = null,
            CancellationTokenSource cts = default,
            Dictionary<string, string> urlSegments = null,
            string requestTargetUri = ""
            )
        {
            OctoPrintApiRequestRespone apiRsponeResult = new() { IsOnline = IsOnline };
            if (!IsOnline) return apiRsponeResult;

            try
            {
                if (cts == default)
                {
                    cts = new(DefaultTimeout);
                }
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                RestRequest request = new(
                    $"{(string.IsNullOrEmpty(requestTargetUri) ? commandBase.ToString() : requestTargetUri)}/{command}")
                {
                    RequestFormat = DataFormat.Json,
                    Method = method
                };

                request.AddHeader("X-Api-Key", ApiKey);

                if (urlSegments != null)
                {
                    foreach (KeyValuePair<string, string> pair in urlSegments)
                    {
                        request.AddParameter(pair.Key, pair.Value, ParameterType.QueryString);
                    }
                }

                if (jsonObject != null)
                {
                    request.AddJsonBody(jsonObject, "application/json");
                }

                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token).ConfigureAwait(false);
                    apiRsponeResult = ValidateRespone(respone, fullUri);
                    /*
                    if (respone.StatusCode == HttpStatusCode.OK && respone.ResponseStatus == ResponseStatus.Completed)
                    {
                        apiRsponeResult.IsOnline = true;
                        AuthenticationFailed = false;
                        apiRsponeResult.Result = respone.Content;
                        apiRsponeResult.Succeeded = true;
                        apiRsponeResult.EventArgs = new OctoPrintRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation
                        || respone.StatusCode == HttpStatusCode.Forbidden
                        || respone.StatusCode == HttpStatusCode.Unauthorized
                        )
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = true;
                        apiRsponeResult.EventArgs = new OctoPrintRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    else
                    {
                        OnRestApiError(new OctoPrintRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        });
                        //throw respone.ErrorException;
                    }
                    */
                }
                catch (TaskCanceledException texp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    }
                }
                catch (HttpRequestException hexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(hexp, false));
                    }
                }
                catch (TimeoutException toexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(toexp, false));
                    }
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<OctoPrintApiRequestRespone> SendOnlineCheckRestApiRequestAsync(
            OctoPrintCommandBase commandBase,
            string command,
            CancellationTokenSource cts,
            string requestTargetUri = ""
            )
        {
            OctoPrintApiRequestRespone apiRsponeResult = new() { IsOnline = false };
            try
            {
                if (cts == default)
                {
                    cts = new(DefaultTimeout);
                }
                // https://github.com/Arksine/moonraker/blob/master/docs/web_api.md
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                RestRequest request = new(
                    $"{(string.IsNullOrEmpty(requestTargetUri) ? commandBase.ToString() : requestTargetUri)}/{command}")
                {
                    RequestFormat = DataFormat.Json,
                    Method = Method.Get
                };

                request.AddHeader("X-Api-Key", ApiKey);

                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token).ConfigureAwait(false);
                    apiRsponeResult = ValidateRespone(respone, fullUri);
                    /*
                    if (respone.StatusCode == HttpStatusCode.OK && respone.ResponseStatus == ResponseStatus.Completed)
                    {
                        apiRsponeResult.IsOnline = true;
                        AuthenticationFailed = false;
                        apiRsponeResult.Result = respone.Content;
                        apiRsponeResult.Succeeded = true;
                        apiRsponeResult.EventArgs = new OctoPrintRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation
                        || respone.StatusCode == HttpStatusCode.Forbidden
                        || respone.StatusCode == HttpStatusCode.Unauthorized
                        )
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = true;
                        apiRsponeResult.EventArgs = new OctoPrintRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    else
                    {
                        OnRestApiError(new OctoPrintRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        });
                        //throw respone.ErrorException;
                    }
                    */
                }
                catch (TaskCanceledException)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                }
                catch (HttpRequestException)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                }
                catch (TimeoutException)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                }

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<OctoPrintApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            string filePath,
            string location,
            string path,
            bool selectFile = false,
            bool printFile = false,
            int timeout = 100000
            )
        {
            OctoPrintApiRequestRespone apiRsponeResult = new();
            if (!IsOnline) return apiRsponeResult;

            try
            {
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
                RestRequest request = new($"/api/files/{location}");

                request.AddHeader("X-Api-Key", ApiKey);

                request.RequestFormat = DataFormat.Json;
                request.Method = Method.Post;
                request.AlwaysMultipartFormData = true;

                //Multiform
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFile("file", filePath, "application/octet-stream");
                request.AddParameter("select", selectFile ? "true" : "false", ParameterType.GetOrPost);
                request.AddParameter("print", printFile ? "true" : "false", ParameterType.GetOrPost);
                request.AddParameter("path", path, ParameterType.GetOrPost);

                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token);
                    apiRsponeResult = ValidateRespone(respone, fullUri);
                }
                catch (TaskCanceledException texp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    }
                }
                catch (HttpRequestException hexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(hexp, false));
                    }
                }
                catch (TimeoutException toexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(toexp, false));
                    }
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<OctoPrintApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            string filePath,
            OctoPrintFileLocation location,
            string path,
            bool selectFile = false,
            bool printFile = false,
            int timeout = 100000
            )
        {
            return await SendMultipartFormDataFileRestApiRequestAsync(filePath, location.Location, path, selectFile, printFile, timeout)
                .ConfigureAwait(false);
        }

        async Task<OctoPrintApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            byte[] file,
            string fileName,
            string location,
            string path = "",
            bool selectFile = false,
            bool printFile = false,
            int timeout = 100000
            )
        {
            OctoPrintApiRequestRespone apiRsponeResult = new();
            if (!IsOnline) return apiRsponeResult;

            try
            {
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
                RestRequest request = new($"/api/files/{location}");

                request.AddHeader("X-Api-Key", ApiKey);

                request.RequestFormat = DataFormat.Json;
                request.Method = Method.Post;
                request.AlwaysMultipartFormData = true;

                //Multiform
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFile("file", file, fileName, "application/octet-stream");
                request.AddParameter("select", selectFile ? "true" : "false", ParameterType.GetOrPost);
                request.AddParameter("print", printFile ? "true" : "false", ParameterType.GetOrPost);
                request.AddParameter("path", path, ParameterType.GetOrPost);

                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token);
                    apiRsponeResult = ValidateRespone(respone, fullUri);
                }
                catch (TaskCanceledException texp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    }
                }
                catch (HttpRequestException hexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(hexp, false));
                    }
                }
                catch (TimeoutException toexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(toexp, false));
                    }
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<OctoPrintApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            byte[] file,
            string fileName,
            OctoPrintFileLocation location,
            string path = "",
            bool selectFile = false,
            bool printFile = false,
            int timeout = 100000
            )
        {
            return await SendMultipartFormDataFileRestApiRequestAsync(file, fileName, location.Location, path, selectFile, printFile, timeout)
                .ConfigureAwait(false);
        }

        async Task<OctoPrintApiRequestRespone> SendMultipartFormDataFolderRestApiRequestAsync(
            string folderName,
            string location,
            string path,
            int timeout = 100000
            )
        {
            OctoPrintApiRequestRespone apiRsponeResult = new();
            if (!IsOnline) return apiRsponeResult;

            try
            {
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
                RestRequest request = new($"/api/files/{location}");

                request.AddHeader("X-Api-Key", ApiKey);

                request.RequestFormat = DataFormat.Json;
                request.Method = Method.Post;
                request.AlwaysMultipartFormData = true;

                //Multiform
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddParameter("foldername", folderName, ParameterType.GetOrPost);
                request.AddParameter("path", path, ParameterType.GetOrPost);

                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token);
                    apiRsponeResult = ValidateRespone(respone, fullUri);
                }
                catch (TaskCanceledException texp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    }
                }
                catch (HttpRequestException hexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(hexp, false));
                    }
                }
                catch (TimeoutException toexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(toexp, false));
                    }
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<OctoPrintApiRequestRespone> SendMultipartFormDataFolderRestApiRequestAsync(
            string folderName,
            OctoPrintFileLocation location,
            string path,
            int timeout = 100000
            )
        {
            return await SendMultipartFormDataFolderRestApiRequestAsync(folderName, location.Location, path, timeout)
                .ConfigureAwait(false);
        }
        #endregion

        #region Download
        public async Task<byte[]> DownloadFileFromUriAsync(string path, int timeout = 100000)
        {
            try
            {
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                RestRequest request = new(path);
                request.AddHeader("X-Api-Key", ApiKey);

                request.RequestFormat = DataFormat.Json;
                request.Method = Method.Get;
                request.Timeout = timeout;

                Uri fullUrl = restClient.BuildUri(request);
                CancellationTokenSource cts = new(timeout);
                byte[] respone = await restClient.DownloadDataAsync(request, cts.Token)
                    .ConfigureAwait(false)
                    ;

                return respone;
                /*
                // Workaround, because the RestClient returns bad requests
                using WebClient client = new();
                byte[] bytes = await client.DownloadDataTaskAsync(fullUrl);
                return bytes;
                */
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }
        #endregion

        #region StateUpdates
        void UpdatePrinterState(OctoPrintPrinterState newState)
        {
            try
            {
                if (newState == null || newState?.Temperature == null)
                {
                    return;
                }

                Extruders = newState.Temperature?.Tool1 == null ? new()
                {
                    newState.Temperature?.Tool0,
                } :
                new()
                {
                    newState.Temperature?.Tool0,
                    newState.Temperature?.Tool1,
                }
                ;
                HeatedBeds = newState.Temperature?.Bed != null ? new()
                {
                    newState.Temperature?.Bed,
                } :
                new();
                HeatedChambers = newState.Temperature?.Chamber != null ? new()
                {
                    newState.Temperature?.Chamber,
                } :
                new();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        void UpdatePrinterConfig(OctoPrintSettings newConfig)
        {
            try
            {
                if (newConfig == null) return;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        void UpdateConnectionSettings(OctoPrintConnectionSettings newConnectionSettings)
        {
            try
            {
                if (newConnectionSettings == null) return;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        #endregion

        #region Models
        async Task<OctoPrintFiles> GetFilesAsync(string location, string path = "", bool recursive = true)
        {
            try
            {
                string files = string.IsNullOrEmpty(path) ? string.Format("files/{0}", location) : string.Format("files/{0}/{1}", location, path);
                Dictionary<string, string> urlSegements = new();
                //get all files & folders 
                urlSegements.Add("recursive", recursive ? "true" : "false");

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, files, jsonObject: null, cts: default, urlSegements)
                    .ConfigureAwait(false);
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                if (list != null)
                {
                    FreeDiskSpace = list.Free;
                    TotalDiskSpace = list.Total;
                }
                return list;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintFiles();
            }
        }
        ObservableCollection<OctoPrintModel> IterateOctoPrintFileStack(OctoPrintFile[] files)
        {
            return IterateOctoPrintFileStack(files.ToList());
        }

        ObservableCollection<OctoPrintModel> IterateOctoPrintFileStack(List<OctoPrintFile> files)
        {
            ObservableCollection<OctoPrintModel> collectedFiles = new();
            try
            {
                if (files == null)
                {
                    return collectedFiles;
                }

                foreach (OctoPrintFile file in files)
                {
                    Stack<OctoPrintFile> fileStack = new();
                    collectedFiles.Add(new OctoPrintModel()
                    {
                        File = file,
                        Name = file.Name,
                        Type = file.Type,
                        Location = file.Origin,
                        Path = file.Path,

                    });
                    fileStack.Push(file);

                    while (fileStack.Count > 0)
                    {
                        OctoPrintFile currentFile = fileStack.Pop();
                        if (currentFile.Children == null) continue;

                        foreach (OctoPrintFile f in currentFile.Children)
                        {
                            collectedFiles.Add(new OctoPrintModel()
                            {
                                File = f,
                                Name = f.Name,
                                Type = f.Type,
                                Location = f.Origin,
                                Path = f.Path,

                            });
                            fileStack.Push(f);
                        }
                    }
                }
                return collectedFiles;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return collectedFiles;
            }

        }
        #endregion

        #region Printers
        async Task<OctoPrintPrinterProfiles> GetPrinterProfilesAsync()
        {
            try
            {
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, "printerprofiles")
                    .ConfigureAwait(false);
                OctoPrintPrinterProfiles list = JsonConvert.DeserializeObject<OctoPrintPrinterProfiles>(result.Result);
                return list;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintPrinterProfiles() { Profiles = new Dictionary<string, OctoPrintPrinter>() };
            }
        }
        #endregion

        #region Proxy
        Uri GetProxyUri()
        {
            return ProxyAddress.StartsWith("http://") || ProxyAddress.StartsWith("https://") ? new Uri($"{ProxyAddress}:{ProxyPort}") : new Uri($"{(SecureProxyConnection ? "https" : "http")}://{ProxyAddress}:{ProxyPort}");
        }

        WebProxy GetCurrentProxy()
        {
            var proxy = new WebProxy()
            {
                Address = GetProxyUri(),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = ProxyUseDefaultCredentials,
            };
            if (ProxyUseDefaultCredentials && !string.IsNullOrEmpty(ProxyUser))
                proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPassword);
            else
                proxy.UseDefaultCredentials = ProxyUseDefaultCredentials;
            return proxy;
        }
        void UpdateRestClientInstance()
        {
            if (string.IsNullOrEmpty(ServerAddress))
            {
                return;
            }
            if (EnableProxy && !string.IsNullOrEmpty(ProxyAddress))
            {
                RestClientOptions options = new(FullWebAddress)
                {
                    ThrowOnAnyError = true,
                    MaxTimeout = 10000,
                };
                HttpClientHandler httpHandler = new()
                {
                    UseProxy = true,
                    Proxy = GetCurrentProxy(),
                    AllowAutoRedirect = true,
                };

                httpClient = new(handler: httpHandler, disposeHandler: true);
                restClient = new(httpClient: httpClient, options: options);
            }
            else
            {
                httpClient = null;
                restClient = new(baseUrl: FullWebAddress);
            }
        }
        #endregion

        #region Timers
        void StopPingTimer()
        {
            if (PingTimer != null)
            {
                try
                {
                    PingTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    PingTimer = null;
                    IsListeningToWebSocket = false;
                }
                catch (ObjectDisposedException)
                {
                    //PingTimer = null;
                }
            }
        }
        void StopTimer()
        {
            if (Timer != null)
            {
                try
                {
                    Timer.Change(Timeout.Infinite, Timeout.Infinite);
                    Timer = null;
                    IsListening = false;
                }
                catch (ObjectDisposedException)
                {
                    //PingTimer = null;
                }
            }
        }
        #endregion

        #endregion

        #region Public

        #region Proxy
        public void SetProxy(bool Secure, string Address, int Port, bool Enable = true)
        {
            EnableProxy = Enable;
            ProxyUseDefaultCredentials = true;
            ProxyAddress = Address;
            ProxyPort = Port;
            ProxyUser = string.Empty;
            ProxyPassword = null;
            SecureProxyConnection = Secure;
            UpdateRestClientInstance();
        }
        public void SetProxy(bool Secure, string Address, int Port, string User = "", SecureString Password = null, bool Enable = true)
        {
            EnableProxy = Enable;
            ProxyUseDefaultCredentials = false;
            ProxyAddress = Address;
            ProxyPort = Port;
            ProxyUser = User;
            ProxyPassword = Password;
            SecureProxyConnection = Secure;
            UpdateRestClientInstance();
        }
        #endregion

        #region Refresh

        public void StartListening(bool StopActiveListening = false)
        {
            if (IsListening)// avoid multiple sessions
            {
                if (StopActiveListening)
                    StopListening();
                else
                    return; // StopListening();
            }
            ConnectWebSocket();
            Timer = new Timer(async (action) =>
            {
                // Do not check the online state ever tick
                if (RefreshCounter > 5)
                {
                    RefreshCounter = 0;
                    await CheckOnlineAsync(3500).ConfigureAwait(false);
                }
                else RefreshCounter++;
                if (IsOnline)
                {
                    List<Task> tasks = new()
                    {
                        RefreshPrinterStateAsync(),
                        RefreshCurrentPrintInfosAsync(),
                        RefreshConnectionSettingsAsync(),
                    };
                    await Task.WhenAll(tasks).ConfigureAwait(false);
                }
                else if (IsListening)
                    StopListening();
            }, null, 0, RefreshInterval * 1000);
            IsListening = true;
        }
        public void StopListening()
        {
            CancelCurrentRequests();
            StopPingTimer();
            StopTimer();

            if (IsListeningToWebSocket)
                DisconnectWebSocket();
            IsListening = false;
        }
        public async Task RefreshAllAsync()
        {
            try
            {
                // Avoid multiple calls
                if (IsRefreshing) return;
                IsRefreshing = true;

                List<Task> task = new()
                {
                    RefreshConnectionSettingsAsync(),
                    RefreshCurrentPrintInfosAsync(),
                    RefreshPrinterStateAsync(),
                    RefreshFilesAsync(CurrentFileLocation),
                };
                await Task.WhenAll(task).ConfigureAwait(false);
                if (!InitialDataFetched)
                    InitialDataFetched = true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            IsRefreshing = false;
        }
        #endregion

        #region ActivePrinter
        public async Task SetPrinterActiveAsync(int Index = -1, bool RefreshPrinterList = true)
        {
            try
            {
                if (RefreshPrinterList)
                    await RefreshPrinterListAsync().ConfigureAwait(false);
                if (Printers.Count > Index && Index >= 0)
                    ActivePrinter = Printers[Index];
                else
                {
                    // If no index is provided, or it's out of bound, the first online printer is used
                    ActivePrinter = Printers.FirstOrDefault(printer => printer.IsOnline);
                    // If no online printers is found, however there is at least one printer configured, use this one
                    if (ActivePrinter == null && Printers.Count > 0)
                        ActivePrinter = Printers[0];
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public async Task SetPrinterActiveAsync(string Id, bool RefreshPrinterList = true)
        {
            try
            {
                if (RefreshPrinterList)
                    await RefreshPrinterListAsync().ConfigureAwait(false);
                OctoPrintPrinter printer = Printers.FirstOrDefault(prt => prt.Id == Id);
                if (printer != null && ActivePrinter != printer)
                {
                    ActivePrinter = printer;
                    //Disconnect
                    //Connect
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        #endregion

        #region CurrentFileLocation
        public async Task SwitchFileLocationAsync(OctoPrintFileLocations newLocation)
        {
            try
            {
                if (CurrentFileLocation == newLocation) return;
                CurrentFileLocation = newLocation;
                await RefreshFilesAsync(CurrentFileLocation);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        #endregion

        #region Cancel
        public void CancelCurrentRequests()
        {
            try
            {
                if (httpClient != null)
                {
                    httpClient.CancelPendingRequests();
                    UpdateRestClientInstance();
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        #endregion

        #region WebCam
        public string GetWebCamUri()
        {
            try
            {
                string currentPrinter = GetActivePrinterSlug();
                if (string.IsNullOrEmpty(currentPrinter)) return string.Empty;
                return $"{FullWebAddress}/webcam/?action=stream?t={ApiKey}";
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return "";
            }
        }

        #endregion

        #region CheckOnline

        public async Task CheckOnlineAsync(int Timeout = 10000)
        {
            CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, Timeout));
            await CheckOnlineAsync(cts).ConfigureAwait(false);
            cts?.Dispose();
        }

        public async Task CheckOnlineAsync(CancellationTokenSource cts)
        {
            if (IsConnecting) return; // Avoid multiple calls
            IsConnecting = true;
            bool isReachable = false;
            try
            {
                string uriString = FullWebAddress;
                try
                {
                    // Send a blank api request in order to check if the server is reachable
                    OctoPrintApiRequestRespone respone = await SendOnlineCheckRestApiRequestAsync(OctoPrintCommandBase.api, "version", cts).ConfigureAwait(false);
                    isReachable = respone?.IsOnline == true;
                }
                catch (InvalidOperationException iexc)
                {
                    OnError(new UnhandledExceptionEventArgs(iexc, false));
                }
                catch (HttpRequestException rexc)
                {
                    OnError(new UnhandledExceptionEventArgs(rexc, false));
                }
                catch (TaskCanceledException)
                {
                    // Throws an exception on timeout, not actually an error
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            IsConnecting = false;
            // Avoid offline message for short connection loss
            if (!IsOnline || isReachable || _retries > RetriesWhenOffline)
            {
                // Do not check if the previous state was already offline
                _retries = 0;
                IsOnline = isReachable;
            }
            else
            {
                // Retry with shorter timeout to see if the connection loss is real
                _retries++;
                await CheckOnlineAsync(3500).ConfigureAwait(false);
            }
        }

        public async Task<bool> CheckIfApiIsValidAsync(int Timeout = 10000)
        {
            try
            {
                if (IsOnline)
                {
                    CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, Timeout));
                    OctoPrintApiRequestRespone respone =
                        await SendOnlineCheckRestApiRequestAsync(OctoPrintCommandBase.api, "version", cts)
                        .ConfigureAwait(false);
                    if (respone.HasAuthenticationError)
                    {
                        AuthenticationFailed = true;
                        OnRestApiAuthenticationError(respone.EventArgs);
                    }
                    else
                    {
                        AuthenticationFailed = false;
                        OnRestApiAuthenticationSucceeded(respone.EventArgs);
                    }
                    return AuthenticationFailed;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task CheckServerIfApiIsValidAsync(int Timeout = 10000)
        {
            _ = await CheckIfApiIsValidAsync(Timeout).ConfigureAwait(false);
        }
        #endregion

        #region CheckForUpdates
        public async Task CheckForServerUpdateAsync()
        {
            OctoPrintApiRequestRespone result = new();
            try
            {
                result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, "checkForUpdates")
                    .ConfigureAwait(false);
            }
            catch (JsonException jecx)
            {
                OnError(new OctoPrintJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        #endregion

        #region DetectChanges
        public bool CheckIfConfigurationHasChanged(OctoPrintClient temp)
        {
            try
            {
                return
                    !(ServerAddress == temp.ServerAddress &&
                        Port == temp.Port &&
                        ApiKey == temp.ApiKey &&
                        IsSecure == temp.IsSecure
                        );
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region VersionInformation
        public async Task<OctoPrintVersionInfo> GetVersionInfoAsync()
        {
            try
            {
                OctoPrintApiRequestRespone result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, "version")
                    .ConfigureAwait(false);
                OctoPrintVersionInfo respone = JsonConvert.DeserializeObject<OctoPrintVersionInfo>(result.Result);
                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintVersionInfo() { Text = "Error" };
            }
        }
        #endregion

        #region ConnectionHandling
        public async Task<OctoPrintConnectionSettings> GetConnectionSettingsAsync()
        {
            try
            {
                string command = "connection";

                OctoPrintApiRequestRespone result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command)
                    .ConfigureAwait(false);
                OctoPrintConnectionSettings respone = JsonConvert.DeserializeObject<OctoPrintConnectionSettings>(result.Result);
                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintConnectionSettings();
            }
        }
        public async Task RefreshConnectionSettingsAsync()
        {
            try
            {
                ConnectionSettings = await GetConnectionSettingsAsync().ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        public async Task<bool> ConnectPrinterAsync(string port, long baudrate, string printerProfile, bool save, bool autoconnect)
        {
            try
            {
                string command = "connection";
                object parameter;
                if (string.IsNullOrEmpty(port) && baudrate < 0)
                {
                    parameter = new
                    {
                        command = "connect",
                        printerProfile = printerProfile,
                        save = save,
                        autoconnect = autoconnect
                    };
                }
                else if (string.IsNullOrEmpty(port))
                {
                    parameter = new
                    {
                        command = "connect",
                        baudrate = baudrate,
                        printerProfile = printerProfile,
                        save = save,
                        autoconnect = autoconnect
                    };
                }
                else
                {
                    parameter = new
                    {
                        command = "connect",
                        port = port,
                        printerProfile = printerProfile,
                        save = save,
                        autoconnect = autoconnect
                    };
                }

                // no content result
                OctoPrintApiRequestRespone result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return string.IsNullOrEmpty(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> ConnectPrinterAsync(string printerProfile, bool save, bool autoconnect, string port = "", long baudrte = -1)
        {
            // Use auto settings
            return await ConnectPrinterAsync(port, baudrte, printerProfile, save, autoconnect)
                .ConfigureAwait(false);
        }

        public async Task<bool> DisconnectPrinterAsync()
        {
            try
            {
                string command = "connection";
                object parameter = new
                {
                    command = "disconnect",
                };

                // no content for this result
                OctoPrintApiRequestRespone result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return string.IsNullOrEmpty(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region File operations

        public async Task<ObservableCollection<OctoPrintModel>> GetAllFilesAsync(string location, string path = "", bool recursive = true)
        {
            try
            {
                OctoPrintFiles models = await GetFilesAsync(location, path, recursive).ConfigureAwait(false);
                if (models != null)
                {
                    //return IterateOctoPrintFileStack(models.Children ?? models.Files);
                    return IterateOctoPrintFileStack(models?.Children?.Count > 0 ? models?.Children : models?.Files);
                }
                else
                {
                    return new ObservableCollection<OctoPrintModel>();
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new ObservableCollection<OctoPrintModel>();
            }
        }
        //public async Task RefreshFilesAsync(IProgress<int> Prog = null)
        public async Task RefreshFilesAsync()
        {
            try
            {
                ObservableCollection<OctoPrintModel> modelDatas = new();
                if (!IsReady || ActivePrinter == null)
                {
                    Models = modelDatas;
                    return;
                }
                Models = await GetAllFilesAsync(CurrentFileLocation.ToString()).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Models = new ObservableCollection<OctoPrintModel>();
            }
        }
        //public async Task RefreshFilesAsync(string Location, IProgress<int> Prog = null)
        public async Task RefreshFilesAsync(string Location)
        {
            try
            {
                ObservableCollection<OctoPrintModel> modelDatas = new();
                if (!IsReady || ActivePrinter == null)
                {
                    Models = modelDatas;
                    return;
                }
                Models = await GetAllFilesAsync(Location).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Models = new ObservableCollection<OctoPrintModel>();
            }
        }
        //public async Task RefreshFilesAsync(OctoPrintFileLocations Location, IProgress<int> Prog = null)
        public async Task RefreshFilesAsync(OctoPrintFileLocations Location)
        {
            await RefreshFilesAsync(Location.ToString());
            //await RefreshFilesAsync(Location.ToString(), Prog);
        }


        public async Task<OctoPrintFile> GetFileAsync(string location, string filename)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", location, filename);

                // no content for this result
                OctoPrintApiRequestRespone result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command)
                    .ConfigureAwait(false);
                OctoPrintFile respone = JsonConvert.DeserializeObject<OctoPrintFile>(result.Result);
                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintFile();
            }
        }

        public async Task<bool> SelectFileAsync(OctoPrintFile file, bool startPrint = false)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", file.Origin, file.Path);
                object parameter = new { command = "select", print = startPrint ? "true" : "false" };

                // no content for this result
                OctoPrintApiRequestRespone result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return string.IsNullOrEmpty(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        /*
        public async Task<bool> SliceFileAsync(OctoPrintFile file, bool startPrint = false)
        {
            try
            {
                throw new NotImplementedException();
                
                string command = string.Format("files/{0}/{1}", file.Origin, file.Path);
                object parameter = new { command = "select", print = startPrint ? "true" : "false" };

                var result = await sendRestAPIRequestAsync(command, Method.Post, parameter);
                var respone = JsonConvert.DeserializeObject<OctoPrintFileActionRespond>(result);
                return true;
                
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        */

        public async Task<bool> CopyFileAsync(OctoPrintFile file, string destination)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", file.Origin, file.Path);
                object parameter = new
                {
                    command = "copy",
                    destination = destination
                };

                OctoPrintApiRequestRespone result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Copy, command, parameter)
                    .ConfigureAwait(false);
                OctoPrintFileActionRespond respone = JsonConvert.DeserializeObject<OctoPrintFileActionRespond>(result.Result);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> MoveFileAsync(OctoPrintFile file, string destination)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", file.Origin, file.Path);
                object parameter = new
                {
                    command = "move",
                    destination = destination
                };

                OctoPrintApiRequestRespone result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                OctoPrintFileActionRespond respone = JsonConvert.DeserializeObject<OctoPrintFileActionRespond>(result.Result);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> DeleteFileAsync(OctoPrintFile file)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", file.Origin, file.Path);

                //no result
                OctoPrintApiRequestRespone result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Delete, command)
                    .ConfigureAwait(false);
                return string.IsNullOrEmpty(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> UploadFileAsync(OctoPrintFileLocation location, string target, string filePath, bool select = false, bool print = false)
        {
            try
            {
                OctoPrintApiRequestRespone result =
                    await SendMultipartFormDataFileRestApiRequestAsync(filePath, location, target, select, print)
                    .ConfigureAwait(false);
                if (result != null)
                {
                    OctoPrintUploadFileRespone respone = JsonConvert.DeserializeObject<OctoPrintUploadFileRespone>(result.Result);
                    return respone.Done;
                }
                else return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> UploadFileAsync(OctoPrintFileLocation location, string target, byte[] file, string fileName, bool select = false, bool print = false)
        {
            try
            {
                OctoPrintApiRequestRespone result =
                    await SendMultipartFormDataFileRestApiRequestAsync(file, fileName, location, target, select, print)
                    .ConfigureAwait(false);
                if (result != null)
                {
                    OctoPrintUploadFileRespone respone = JsonConvert.DeserializeObject<OctoPrintUploadFileRespone>(result.Result);
                    return respone.Done;
                }
                else return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<byte[]> DownloadFileAsync(string dowmloadUri, int timeout = 100000)
        {
            try
            {
                byte[] result = await DownloadFileFromUriAsync(dowmloadUri, timeout);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }
        #endregion

        #region Folders
        public async Task<bool> CreateNewFolderAsync(string location, string path, string name)
        {
            try
            {
                OctoPrintApiRequestRespone result = await SendMultipartFormDataFolderRestApiRequestAsync(name, location, path);
                if (result != null)
                    return result.Succeeded;
                else return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        #endregion

        #region PrinterOperations
        public async Task<OctoPrintPrinterState> GetCurrentPrinterStateAsync(bool includeHistory, int limit = 0, string[] excludes = null)
        {
            try
            {
                string command = "printer";
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("history", includeHistory ? "true" : "false");
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());
                if (excludes != null && excludes.Length > 0)
                {
                    StringBuilder sb = new();
                    for (int i = 0; i < excludes.Length; i++)
                    {
                        sb.Append(excludes[i]);
                        if (i < excludes.Length - 1)
                            sb.Append(",");
                    }
                    urlSegements.Add("exclude", sb.ToString());
                }

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command, jsonObject: null, cts: default, urlSegements)
                    .ConfigureAwait(false);
                OctoPrintPrinterState respone = JsonConvert.DeserializeObject<OctoPrintPrinterState>(result.Result);
                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintPrinterState();
            }
        }

        public async Task RefreshPrinterStateAsync(bool IncludeHistory = false)
        {
            try
            {
                if (!IsReady)
                {
                    return;
                }
                State = await GetCurrentPrinterStateAsync(IncludeHistory).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        #region PrintHead
        public async Task<bool> JogPrinterAsync(
            double Speed = 100,
            double X = double.PositiveInfinity,
            double Y = double.PositiveInfinity,
            double Z = double.PositiveInfinity,
            //double E = double.PositiveInfinity,
            bool absolute = false)
        {
            try
            {
                string command = "printer/printhead";
                object parameter = new
                {
                    command = "jog",
                    x = double.IsInfinity(X) ? 0 : X,
                    y = double.IsInfinity(Y) ? 0 : Y,
                    z = double.IsInfinity(Z) ? 0 : Z,
                    speed = Speed,
                    absolute = absolute ? "true" : "false"
                };

                // no content for this result
                OctoPrintApiRequestRespone result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        [Obsolete]
        public async Task<bool> JogPrinterAsync(double x, double y, double z, int speed = -1, bool absolute = false)
        {
            try
            {
                string command = "printer/printhead";
                object parameter = new { command = "jog", x = x, y = y, z = z, absolute = absolute };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> HomePrinterAsync(bool x, bool y, bool z)
        {
            try
            {
                List<string> axes = new();
                if (x) axes.Add("x");
                if (y) axes.Add("y");
                if (z) axes.Add("z");

                string command = "printer/printhead";
                object parameter = new { command = "home", axes = axes };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> SetFeedRateAsync(int feedRate)
        {
            try
            {

                string command = "printer/printhead";
                object parameter = new { command = "feedrate", factor = feedRate };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                LastFeedRate = feedRate;
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> SetFeedRateAsync(double feedRate)
        {
            try
            {

                string command = "printer/printhead";
                object parameter = new { command = "feedrate", factor = feedRate };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                LastFeedRate = feedRate;
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Tool
        public async Task<OctoPrintToolState> GetCurrentToolStateAsync(bool includeHistory, int limit = 0)
        {
            try
            {
                string command = "printer/tool";
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("history", includeHistory ? "true" : "false");
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command, jsonObject: null, cts: default, urlSegements)
                    .ConfigureAwait(false);
                OctoPrintToolState respone = JsonConvert.DeserializeObject<OctoPrintToolState>(result.Result);
                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintToolState();
            }
        }
        public async Task<bool> SetToolTemperatureAsync(long tool0, long tool1 = 0)
        {
            return await SetToolTemperatureAsync(Convert.ToInt32(tool0), Convert.ToInt32(tool1));
        }
        public async Task<bool> SetToolTemperatureAsync(int tool0 = int.MinValue, int tool1 = int.MinValue)
        {
            try
            {

                string command = "printer/tool";
                object parameter = null;
                if (tool0 != int.MinValue && tool1 != int.MinValue)
                    parameter = new { command = "target", targets = new { tool0 = tool0, tool1 = tool1 } };
                else if (tool0 != int.MinValue)
                    parameter = new { command = "target", targets = new { tool0 = tool0 } };
                else
                    parameter = new { command = "target", targets = new { tool1 = tool1 } };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> SetToolOffsetTemperatureAsync(int tool0, int tool1 = 0)
        {
            try
            {

                string command = "printer/tool";
                object parameter = new { command = "offset", offsets = new { tool0 = tool0, tool1 = tool1 } };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> SelectToolAsync(int tool)
        {
            try
            {
                string command = "printer/tool";
                object parameter = new { command = "select", tool = string.Format("tool{0}", tool) };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> ExtrudeSelectedToolAsync(double length, double speed = 100)
        {
            try
            {
                string command = "printer/tool";
                object parameter = new { command = "extrude", amount = length, speed = speed };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> RetractSelectedToolAsync(double length, double speed = 100)
        {
            return await ExtrudeSelectedToolAsync(-length, speed);
        }
        public async Task<bool> SetFlowRateAsync(int flowRate)
        {
            try
            {
                string command = "printer/tool";
                object parameter = new { command = "flowrate", factor = flowRate };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                LastFlowRate = flowRate;
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> SetFlowRateAsync(double flowRate)
        {
            try
            {
                string command = "printer/tool";
                object parameter = new { command = "flowrate", factor = flowRate };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                LastFlowRate = flowRate;
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Bed
        public async Task<OctoPrintBedState> GetCurrentBedStateAsync(bool includeHistory, int limit = 0)
        {
            try
            {
                string command = "printer/bed";
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("history", includeHistory ? "true" : "false");
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, jsonObject: null, cts: default, urlSegements)
                    .ConfigureAwait(false);
                OctoPrintBedState respone = JsonConvert.DeserializeObject<OctoPrintBedState>(result.Result);
                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintBedState();
            }
        }
        public async Task<bool> SetBedTemperatureAsync(long target)
        {
            return await SetBedTemperatureAsync(Convert.ToInt32(target)).ConfigureAwait(false);
        }
        public async Task<bool> SetBedTemperatureAsync(int target)
        {
            try
            {
                string command = "printer/bed";
                object parameter = new { command = "target", target = target };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return string.IsNullOrEmpty(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> SetBedOffsetTemperatureAsync(int offset)
        {
            try
            {
                string command = "printer/bed";
                object parameter = new { command = "offset", offset = offset };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Chamber
        public async Task<OctoPrintChamberState> GetCurrentChamberStateAsync(bool includeHistory, int limit = 0)
        {
            try
            {
                string command = "printer/chamber";
                Dictionary<string, string> urlSegements = new()
                {
                    { "history", includeHistory ? "true" : "false" }
                };
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, jsonObject: null, cts: default, urlSegements)
                    .ConfigureAwait(false);
                OctoPrintChamberState respone = JsonConvert.DeserializeObject<OctoPrintChamberState>(result.Result);
                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintChamberState();
            }
        }
        public async Task<bool> SetChamberTemperatureAsync(long target)
        {
            return await SetChamberTemperatureAsync(Convert.ToInt32(target)).ConfigureAwait(false);
        }
        public async Task<bool> SetChamberTemperatureAsync(int target)
        {
            try
            {
                string command = "printer/chamber";
                object parameter = new { command = "target", target = target };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> SetChamberOffsetTemperatureAsync(int offset)
        {
            try
            {
                string command = "printer/chamber";
                object parameter = new { command = "offset", offset = offset };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Sd
        public async Task<OctoPrintPrinterStateSd> GetCurrentSdState(bool includeHistory, int limit = 0)
        {
            try
            {
                string command = "printer/chamber";
                Dictionary<string, string> urlSegements = new()
                {
                    { "history", includeHistory ? "true" : "false" }
                };
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command, jsonObject: null, cts: default, urlSegements)
                    .ConfigureAwait(false);
                OctoPrintPrinterStateSd respone = JsonConvert.DeserializeObject<OctoPrintPrinterStateSd>(result.Result);
                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintPrinterStateSd();
            }
        }
        public async Task<bool> InitSdCardAsync()
        {
            try
            {
                string command = "printer/sd";
                object parameter = new { command = "init" };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> RefreshSdCardAsync()
        {
            try
            {
                string command = "printer/sd";
                object parameter = new { command = "refresh" };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> ReleaseSdCardAsync()
        {
            try
            {
                string command = "printer/sd";
                object parameter = new { command = "release" };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        #endregion

        #region Commands
        public async Task<bool> SendCommandAsync(string cmd)
        {
            try
            {
                string command = "printer/command";
                object parameter = new { command = cmd };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> SendCommandsAsync(string[] cmds)
        {
            try
            {
                string command = "printer/command";
                object parameter = new { commands = cmds };

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region CustomControls
        /*
        public async Task<OctoPrintPrinterStateSd> GetCustomControlsAsync(bool includeHistory, int limit = 0)
        {
            throw new NotImplementedException();
            
            try
            {             
                
                string command = "printer/command/custom";
                Dictionary<string, string> urlSegements = new Dictionary<string, string>();
                urlSegements.Add("history", includeHistory ? "true" : "false");
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());

                var result = await SendRestApiRequestAsync(Method.Get, command, "", urlSegements).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintPrinterStateSd>(result.Result);
                return respone;
                
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintPrinterStateSd();
            }
            
        }*/
        #endregion

        #endregion

        #region Printer profile operations

        public async Task RefreshPrinterListAsync()
        {
            try
            {
                Printers = await GetAllPrinterProfilesAsync().ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public async Task<ObservableCollection<OctoPrintPrinter>> GetAllPrinterProfilesAsync()
        {
            try
            {
                OctoPrintPrinterProfiles result = await GetPrinterProfilesAsync().ConfigureAwait(false);
                ObservableCollection<OctoPrintPrinter> profile = new ObservableCollection<OctoPrintPrinter>(result.Profiles.Select(pair => pair.Value));
                return profile;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new ObservableCollection<OctoPrintPrinter>();
            }
        }

        public async Task<OctoPrintPrinter> GetPrinterProfileAsync(string Id)
        {
            try
            {
                OctoPrintPrinterProfiles result = await GetPrinterProfilesAsync().ConfigureAwait(false);
                OctoPrintPrinter profile = new ObservableCollection<OctoPrintPrinter>(result.Profiles.Select(pair => pair.Value)).FirstOrDefault(prof => prof.Id == Id);
                return profile;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintPrinter();
            }
        }
        #endregion

        #region Job operations
        public async Task<bool> StartJobAsync()
        {
            try
            {
                string command = "job";
                object parameter = new { command = "start" };
                //string parameter = string.Format("{{\"command\":{0}}}", "start");

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result?.Result);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> CancelJobAsync()
        {
            try
            {
                string command = "job";
                object parameter = new { command = "cancel" };
                //string parameter = string.Format("{{\"command\":{0}}}", "cancel");

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> RestartJobAsync()
        {
            try
            {
                string command = "job";
                object parameter = new { command = "restart" };
                //string parameter = string.Format("{{\"command\":{0}}}", "restart");

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> PauseJobAsync()
        {
            try
            {
                string command = "job";
                object parameter = new { command = "pause", action = "pause" };
                //string parameter = string.Format("{{\"command\":{0}, \"action\":{1}}}", "pause", "pause");

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> ResumeJobAsync()
        {
            try
            {
                string command = "job";
                object parameter = new { command = "pause", action = "resume" };
                //string parameter = string.Format("{{\"command\":{0}, \"action\":{1}}}", "pause", "resume");

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> TooglePauseJobAsync()
        {
            try
            {
                string command = "job";
                object parameter = new { command = "pause", action = "toggle" };
                //string parameter = string.Format("{{\"command\":{0}, \"action\":{1}}}", "pause", "toggle");

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<OctoPrintJobInfo> GetJobInfoAsync()
        {
            try
            {
                string command = "job";

                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command)
                    .ConfigureAwait(false);
                OctoPrintJobInfo list = JsonConvert.DeserializeObject<OctoPrintJobInfo>(result.Result);
                return list;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintJobInfo() { };
            }
        }

        public async Task RefreshCurrentPrintInfosAsync()
        {
            try
            {
                ActivePrintInfo = await GetJobInfoAsync().ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                ActivePrintInfo = null;
            }

        }
        #endregion

        #region Settings
        public async Task<OctoPrintSettings> GetSettingsAsync()
        {
            try
            {
                string command = "settings";

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command)
                    .ConfigureAwait(false);
                OctoPrintSettings respone = JsonConvert.DeserializeObject<OctoPrintSettings>(result.Result);
                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintSettings();
            }
        }
        public async Task RefreshSettingsAsync()
        {
            try
            {
                Config = await GetSettingsAsync().ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        public async Task<bool> SaveSettingsAsync(object SettingsTreeObject)
        {
            try
            {
                string command = "settings";

                // no content for this result
                OctoPrintApiRequestRespone result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, SettingsTreeObject)
                    .ConfigureAwait(false);
                OctoPrintSettings respone = JsonConvert.DeserializeObject<OctoPrintSettings>(result.Result);
                return true;
                //return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #endregion

        #endregion

        #region Overrides
        public override string ToString()
        {
            try
            {
                return FullWebAddress;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return string.Empty;
            }
        }
        public override bool Equals(object obj)
        {
            if
                (obj is not OctoPrintClient item)
                return false;
            return this.Id.Equals(item.Id);
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            // Ordinarily, we release unmanaged resources here;
            // but all are wrapped by safe handles.

            // Release disposable objects.
            if (disposing)
            {
                if (WebSocket != null)
                {
                    WebSocket.Close();
                    WebSocket = null;
                }
            }
        }
        #endregion
    }
}

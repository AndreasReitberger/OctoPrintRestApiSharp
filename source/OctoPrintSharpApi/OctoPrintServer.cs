using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// Thirdparty
using RestSharp;
using Newtonsoft.Json;
using AndreasReitberger.Enum;
using AndreasReitberger.Models;
using AndreasReitberger.Models.Settings;
using AndreasReitberger.Utilities;
using System.Threading;
using System.Xml.Serialization;
using AndreasReitberger.Interfaces;
using System.Net.Http;
using System.Security.Authentication;
using System.Security;

using WebSocket4Net;
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;

namespace AndreasReitberger
{
    //http://docs.octoprint.org/en/master/api/
    public class OctoPrintServer : IPrintServerClient
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Variables
        //string httpProtocol = "http://";
        CancellationTokenSource cancellation;
        static HttpClient client = new HttpClient();
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
        static OctoPrintServer _instance = null;
        static readonly object Lock = new object();
        public static OctoPrintServer Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                        _instance = new OctoPrintServer();
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

        bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive == value)
                    return;
                _isActive = value;
                OnPropertyChanged();
            }
        }

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
                    InitInstance(this.ServerAddress, this.API, this.Port, this.IsSecure);

                OnPropertyChanged();
            }
        }

        bool _isInitialized = false;
        public bool IsInitialized
        {
            get => _isInitialized;
            set
            {
                if (_isInitialized == value)
                    return;
                _isInitialized = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RefreshTimer
        [JsonIgnore]
        [XmlIgnore]
        Timer _timer;
        [JsonIgnore]
        [XmlIgnore]
        public Timer Timer
        {
            get => _timer;
            set
            {
                if (_timer == value) return;
                _timer = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(RefreshInterval))]
        int _refreshInterval = 2;
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

        [JsonIgnore]
        [XmlIgnore]
        bool _isListening = false;
        [JsonIgnore]
        [XmlIgnore]
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
                    IsListeningToWebSocket = IsListeningToWebsocket,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        bool _initialDataFetched = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool InitialDataFetched
        {
            get => _initialDataFetched;
            set
            {
                if (_initialDataFetched == value) return;
                _initialDataFetched = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Properties

        #region Connection
        [JsonIgnore]
        [XmlIgnore]
        HttpMessageHandler _httpHandler;
        [JsonIgnore]
        [XmlIgnore]
        public HttpMessageHandler HttpHandler
        {
            get => _httpHandler;
            set
            {
                if (_httpHandler == value) return;
                _httpHandler = value;
                UpdateWebClientInstance();
                OnPropertyChanged();

            }
        }

        [JsonIgnore]
        [XmlIgnore]
        string _sessionId = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
        public string SessionId
        {
            get => _sessionId;
            set
            {
                if (_sessionId == value) return;
                _sessionId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ServerAddress))]
        string _address = string.Empty;
        [JsonIgnore]
        public string ServerAddress
        {
            get => _address;
            set
            {
                if (_address == value) return;
                _address = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsSecure))]
        bool _isSecure = false;
        [JsonIgnore]
        public bool IsSecure
        {
            get => _isSecure;
            set
            {
                if (_isSecure == value) return;
                _isSecure = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(API))]
        string _api = string.Empty;
        [JsonIgnore]
        public string API
        {
            get => _api;
            set
            {
                if (_api == value) return;
                _api = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Port))]
        int _port = 8080;
        [JsonIgnore]
        public int Port
        {
            get => _port;
            set
            {
                if (_port == value) return;
                _port = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(OverrideValidationRules))]
        [XmlAttribute(nameof(OverrideValidationRules))]
        bool _overrideValidationRules = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool OverrideValidationRules
        {
            get => _overrideValidationRules;
            set
            {
                if (_overrideValidationRules == value)
                    return;
                _overrideValidationRules = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsOnline))]
        [XmlAttribute(nameof(IsOnline))]
        bool _isOnline = false;
        [JsonIgnore]
        [XmlIgnore]
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

        [JsonProperty(nameof(IsConnecting))]
        [XmlAttribute(nameof(IsConnecting))]
        bool _isConnecting = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool IsConnecting
        {
            get => _isConnecting;
            set
            {
                if (_isConnecting == value) return;
                _isConnecting = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(AuthenticationFailed))]
        [XmlAttribute(nameof(AuthenticationFailed))]
        bool _authenticationFailed = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool AuthenticationFailed
        {
            get => _authenticationFailed;
            set
            {
                if (_authenticationFailed == value) return;
                _authenticationFailed = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsRefreshing))]
        [XmlAttribute(nameof(IsRefreshing))]
        bool _isRefreshing = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing == value) return;
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(RetriesWhenOffline))]
        [XmlAttribute(nameof(RetriesWhenOffline))]
        int _retriesWhenOffline = 2;
        [JsonIgnore]
        [XmlIgnore]
        public int RetriesWhenOffline
        {
            get => _retriesWhenOffline;
            set
            {
                if (_retriesWhenOffline == value) return;
                _retriesWhenOffline = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region General
        [JsonIgnore]
        [XmlIgnore]
        bool _updateAvailable = false;
        [JsonIgnore]
        [XmlIgnore]
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

        [JsonIgnore]
        [XmlIgnore]
        object _update;
        [JsonIgnore]
        [XmlIgnore]
        public object Update
        {
            get => _update;
            private set
            {
                if (_update == value) return;
                _update = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Proxy
        [JsonProperty(nameof(EnableProxy))]
        [XmlAttribute(nameof(EnableProxy))]
        bool _enableProxy = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool EnableProxy
        {
            get => _enableProxy;
            set
            {
                if (_enableProxy == value) return;
                _enableProxy = value;
                OnPropertyChanged();
                UpdateWebClientInstance();
            }
        }

        [JsonProperty(nameof(ProxyUseDefaultCredentials))]
        [XmlAttribute(nameof(ProxyUseDefaultCredentials))]
        bool _proxyUseDefaultCredentials = true;
        [JsonIgnore]
        [XmlIgnore]
        public bool ProxyUseDefaultCredentials
        {
            get => _proxyUseDefaultCredentials;
            set
            {
                if (_proxyUseDefaultCredentials == value) return;
                _proxyUseDefaultCredentials = value;
                OnPropertyChanged();
                UpdateWebClientInstance();
            }
        }

        [JsonProperty(nameof(SecureProxyConnection))]
        [XmlAttribute(nameof(SecureProxyConnection))]
        bool _secureProxyConnection = true;
        [JsonIgnore]
        [XmlIgnore]
        public bool SecureProxyConnection
        {
            get => _secureProxyConnection;
            private set
            {
                if (_secureProxyConnection == value) return;
                _secureProxyConnection = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyAddress))]
        [XmlAttribute(nameof(ProxyAddress))]
        string _proxyAddress = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
        public string ProxyAddress
        {
            get => _proxyAddress;
            private set
            {
                if (_proxyAddress == value) return;
                _proxyAddress = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyPort))]
        [XmlAttribute(nameof(ProxyPort))]
        int _proxyPort = 443;
        [JsonIgnore]
        [XmlIgnore]
        public int ProxyPort
        {
            get => _proxyPort;
            private set
            {
                if (_proxyPort == value) return;
                _proxyPort = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyUser))]
        [XmlAttribute(nameof(ProxyUser))]
        string _proxyUser = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
        public string ProxyUser
        {
            get => _proxyUser;
            private set
            {
                if (_proxyUser == value) return;
                _proxyUser = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyPassword))]
        [XmlAttribute(nameof(ProxyPassword))]
        SecureString _proxyPassword;
        [JsonIgnore]
        [XmlIgnore]
        public SecureString ProxyPassword
        {
            get => _proxyPassword;
            private set
            {
                if (_proxyPassword == value) return;
                _proxyPassword = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region DiskSpace
        [JsonProperty(nameof(FreeDiskSpace))]
        long _freeDiskspace = 0;
        [JsonIgnore]
        public long FreeDiskSpace
        {
            get => _freeDiskspace;
            set
            {
                if (_freeDiskspace == value) return;
                _freeDiskspace = value;
                OnPropertyChanged();

            }
        }

        [JsonProperty(nameof(TotalDiskSpace))]
        long _totalDiskSpace = 0;
        [JsonIgnore]
        public long TotalDiskSpace
        {
            get => _totalDiskSpace;
            set
            {
                if (_totalDiskSpace == value) return;
                _totalDiskSpace = value;
                OnPropertyChanged();

            }
        }
        #endregion

        #region PrinterStateInformation
        [JsonProperty(nameof(LastFlowRate))]
        double _lastFlowRate = 100;
        [JsonIgnore]
        public double LastFlowRate
        {
            get => _lastFlowRate;
            set
            {
                if (_lastFlowRate == value) return;
                _lastFlowRate = value;
                OnPropertyChanged();

            }
        }

        [JsonProperty(nameof(LastFeedRate))]
        double _lastFeedRate = 100;
        [JsonIgnore]
        public double LastFeedRate
        {
            get => _lastFeedRate;
            set
            {
                if (_lastFeedRate == value) return;
                _lastFeedRate = value;
                OnPropertyChanged();

            }
        }

        [JsonProperty(nameof(CurrentFileLocation))]
        OctoPrintFileLocations _currentFileLocation = OctoPrintFileLocations.local;
        [JsonIgnore]
        public OctoPrintFileLocations CurrentFileLocation
        {
            get => _currentFileLocation;
            set
            {
                if (_currentFileLocation == value) return;
                _currentFileLocation = value;
                OnPropertyChanged();

            }
        }
        #endregion

        #region Printers
        [JsonIgnore]
        [XmlIgnore]
        OctoPrintPrinter _activePrinter;
        [JsonIgnore]
        [XmlIgnore]
        public OctoPrintPrinter ActivePrinter
        {
            get => _activePrinter;
            set
            {
                if (_activePrinter == value) return;
                _activePrinter = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        ObservableCollection<OctoPrintPrinter> _printers = new ObservableCollection<OctoPrintPrinter>();
        [JsonIgnore]
        [XmlIgnore]
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
        [JsonIgnore]
        [XmlIgnore]
        OctoPrintSettings _config;
        [JsonIgnore]
        [XmlIgnore]
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

        [JsonIgnore]
        [XmlIgnore]
        OctoPrintConnectionSettings _connectionSettings;
        [JsonIgnore]
        [XmlIgnore]
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

        [JsonIgnore]
        [XmlIgnore]
        OctoPrintPrinterState _state;
        [JsonIgnore]
        [XmlIgnore]
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

        [JsonIgnore]
        [XmlIgnore]
        OctoPrintJobInfo _activePrintInfo;
        [JsonIgnore]
        [XmlIgnore]
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
        #endregion

        #region Models
        [JsonIgnore]
        [XmlIgnore]
        ObservableCollection<OctoPrintModel> _models = new ObservableCollection<OctoPrintModel>();
        [JsonIgnore]
        [XmlIgnore]
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
                    !string.IsNullOrEmpty(ServerAddress) && !string.IsNullOrEmpty(API)) && Port > 0 &&
                    (
                        // Address
                        (Regex.IsMatch(ServerAddress, RegexHelper.IPv4AddressRegex) || Regex.IsMatch(ServerAddress, RegexHelper.IPv6AddressRegex) || Regex.IsMatch(ServerAddress, RegexHelper.Fqdn)) &&
                        // API-Key
                        (Regex.IsMatch(API, RegexHelper.OctoPrintApiKey))
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
        [JsonIgnore]
        [XmlIgnore]
        WebSocket _webSocket;
        [JsonIgnore]
        [XmlIgnore]
        public WebSocket WebSocket
        {
            get => _webSocket;
            set
            {
                if (_webSocket == value) return;
                _webSocket = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        Timer _pingTimer;
        [JsonIgnore]
        [XmlIgnore]
        public Timer PingTimer
        {
            get => _pingTimer;
            set
            {
                if (_pingTimer == value) return;
                _pingTimer = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        int _pingCounter = 0;
        [JsonIgnore]
        [XmlIgnore]
        public int PingCounter
        {
            get => _pingCounter;
            set
            {
                if (_pingCounter == value) return;
                _pingCounter = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        int _refreshCounter = 0;
        [JsonIgnore]
        [XmlIgnore]
        public int RefreshCounter
        {
            get => _refreshCounter;
            set
            {
                if (_refreshCounter == value) return;
                _refreshCounter = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        bool _isListeningToWebSocket = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool IsListeningToWebsocket
        {
            get => _isListeningToWebSocket;
            set
            {
                if (_isListeningToWebSocket == value) return;
                _isListeningToWebSocket = value;
                OnListeningChanged(new OctoPrintEventListeningChangedEventArgs()
                {
                    SessonId = SessionId,
                    IsListening = IsListening,
                    IsListeningToWebSocket = value,
                });
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public OctoPrintServer()
        {
            this.cancellation = new CancellationTokenSource();
            Id = Guid.NewGuid();
            UpdateWebClientInstance();
        }
        public OctoPrintServer(string serverAddress, string api, int port = 80, bool isSecure = false)
        {
            this.cancellation = new CancellationTokenSource();
            Id = Guid.NewGuid();
            InitInstance(serverAddress, api, port, isSecure);
            UpdateWebClientInstance();
        }
        #endregion

        #region Destructor
        ~OctoPrintServer()
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
        public static void UpdateSingleInstance(OctoPrintServer Inst)
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
                this.ServerAddress = serverAddress;
                this.API = api;
                this.Port = port;
                this.IsSecure = isSecure;

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
        #endregion

        #endregion

        #region WebSocket
        public void ConnectWebSocket()
        {
            try
            {
                if (!IsReady) return;
                //if (!IsReady || IsListeningToWebsocket) return;

                DisconnectWebSocket();

                string target = $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/sockjs/websocket?t={API}";
                WebSocket = new WebSocket(target);
                WebSocket.EnableAutoSendPing = false;

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
            IsListeningToWebsocket = false;
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
            IsListeningToWebsocket = false;
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

            IsListeningToWebsocket = true;
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
            Method Method, 
            string Command, 
            string JsonDataString = "", 
            Dictionary<string, string> UrlSegments = null, 
            int Timeout = 10000
            )
        {
            OctoPrintApiRequestRespone apiRsponeResult = new OctoPrintApiRequestRespone();
            if (!IsOnline) return apiRsponeResult;
            try
            {
                var cts = new CancellationTokenSource(new TimeSpan(0, 0, 0, 0, Timeout));
                var client = new RestClient(FullWebAddress);

                var request = new RestRequest("/api/{slug}");
                request.AddHeader("X-Api-Key", API);
                request.RequestFormat = DataFormat.Json;
                request.Method = Method;
                request.AddUrlSegment("slug", Command);
                if (UrlSegments != null)
                {
                    foreach (KeyValuePair<string, string> pair in UrlSegments)
                        request.AddParameter(pair.Key, pair.Value);
                }

                if (!string.IsNullOrEmpty(JsonDataString) && JsonDataString != "{}")
                {
                    request.AddJsonBody(JsonDataString);
                }
                var fullUri = client.BuildUri(request);

                try
                {
                    var respone = await client.ExecuteAsync(request, cts.Token).ConfigureAwait(false);

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
                            Uri = fullUri,
                        };
                    }
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation || respone.StatusCode == HttpStatusCode.Forbidden)
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
                    // For instance if printer is not connected
                    else if (respone.StatusCode == HttpStatusCode.Conflict)
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = false;
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
                }
                catch (TaskCanceledException texp)
                {
                    if (!IsOnline)
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                }

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<OctoPrintApiRequestRespone> SendRestApiRequestAsync(
            Method Method,
            string Command, 
            object JsonData, 
            Dictionary<string, string> UrlSegments = null, 
            int Timeout = 10000
            )
        {
            return await SendRestApiRequestAsync(Method, Command, JsonConvert.SerializeObject(JsonData), UrlSegments, Timeout).ConfigureAwait(false);
        }

        async Task<OctoPrintApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            OctoPrintFileLocation Location, 
            string Target, 
            byte[] File, 
            bool SelectFile, 
            bool PrintFile,
            int Timeout = 10000)
        {
            OctoPrintApiRequestRespone apiRsponeResult = new OctoPrintApiRequestRespone();
            if (!IsOnline) return apiRsponeResult;
            
            try
            {
                var cts = new CancellationTokenSource(new TimeSpan(0, 0, 0, 0, Timeout));
                var client = new RestClient(FullWebAddress);

                var request = new RestRequest("/api/files/{slug}");
                request.AddHeader("X-Api-Key", API);
                request.RequestFormat = DataFormat.Json;
                request.Method = Method.POST;
                request.AlwaysMultipartFormData = true;
                request.AddUrlSegment("slug", Location.Location);

                //Multiform
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFileBytes("file", File, "application/octet-stream");
                request.AddParameter("select", SelectFile ? "true" : "false", "multipart/form-data)", ParameterType.GetOrPost);
                request.AddParameter("print", PrintFile ? "true" : "false", "multipart/form-data", ParameterType.GetOrPost);
                request.AddParameter("path", Target, "multipart/form-data", ParameterType.GetOrPost);

                var fullUri = client.BuildUri(request);

                try
                {
                    var respone = await client.ExecuteAsync(request, cts.Token);

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
                            Uri = fullUri,
                        };
                    }
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation || respone.StatusCode == HttpStatusCode.Forbidden)
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
                    // For instance if printer is not connected
                    else if (respone.StatusCode == HttpStatusCode.Conflict)
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = false;
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
                }
                catch (TaskCanceledException texp)
                {
                    if (!IsOnline)
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));               
            }
            return apiRsponeResult;
        }

        async Task<OctoPrintApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            OctoPrintFileLocation Location, 
            string Target, 
            string FilePath, 
            bool SelectFile, 
            bool PrintFile,
            int Timeout = 10000)
        {
            OctoPrintApiRequestRespone apiRsponeResult = new OctoPrintApiRequestRespone();
            if (!IsOnline) return apiRsponeResult;

            try
            {
                var cts = new CancellationTokenSource(new TimeSpan(0, 0, 0, 0, Timeout));
                var client = new RestClient(FullWebAddress);

                var request = new RestRequest("/api/files/{slug}");
                request.AddHeader("X-Api-Key", API);
                request.RequestFormat = DataFormat.Json;
                request.Method = Method.POST;
                request.AlwaysMultipartFormData = true;
                request.AddUrlSegment("slug", Location.Location);

                //Multiform
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFile("file", FilePath, "application/octet-stream");
                request.AddParameter("select", SelectFile ? "true" : "false", "multipart/form-data)", ParameterType.GetOrPost);
                request.AddParameter("print", PrintFile ? "true" : "false", "multipart/form-data", ParameterType.GetOrPost);
                request.AddParameter("path", Target, "multipart/form-data", ParameterType.GetOrPost);

                var fullUri = client.BuildUri(request);

                try
                {
                    var respone = await client.ExecuteAsync(request, cts.Token);

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
                            Uri = fullUri,
                        };
                    }
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation || respone.StatusCode == HttpStatusCode.Forbidden)
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
                    // For instance if printer is not connected
                    else if (respone.StatusCode == HttpStatusCode.Conflict)
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = false;
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
                }
                catch (TaskCanceledException texp)
                {
                    if (!IsOnline)
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));               
            }
            return apiRsponeResult;
        }

        async Task<OctoPrintApiRequestRespone> SendMultipartFormDataFolderCreationRestApiRequestAsync(
            string Target, 
            string FolderName, 
            string Path, 
            int Timeout = 10000)
        {
            OctoPrintApiRequestRespone apiRsponeResult = new OctoPrintApiRequestRespone();
            if (!IsOnline) return apiRsponeResult;
            try
            {
                var cts = new CancellationTokenSource(new TimeSpan(0, 0, 0, 0, Timeout));
                var client = new RestClient(FullWebAddress);

                var request = new RestRequest("/api/files/{slug}");
                request.AddHeader("X-Api-Key", API);
                request.RequestFormat = DataFormat.Json;
                request.Method = Method.POST;
                request.AlwaysMultipartFormData = true;
                request.AddUrlSegment("slug", Target);

                //Multiform
                request.AddHeader("content-type", "multipart/form-data");
                request.AddParameter("foldername", FolderName, "multipart/form-data", ParameterType.GetOrPost);
                request.AddParameter("path", Path, "multipart/form-data", ParameterType.GetOrPost);

                var fullUri = client.BuildUri(request);

                try
                {
                    var respone = await client.ExecuteAsync(request, cts.Token);

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
                            Uri = fullUri,
                        };
                    }
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation || respone.StatusCode == HttpStatusCode.Forbidden)
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
                    // For instance if printer is not connected
                    else if (respone.StatusCode == HttpStatusCode.Conflict)
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = false;
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
                }
                catch (TaskCanceledException texp)
                {
                    if (!IsOnline)
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));               
            }
            return apiRsponeResult;
        }
        #endregion

        #region Download
        byte[] DownloadFile(Uri downloadUri, int Timeout = 5000)
        {
            try
            {
                var client = new RestClient(downloadUri);
                var request = new RestRequest("");
                request.AddHeader("X-Api-Key", API);
                request.RequestFormat = DataFormat.Json;
                request.Method = Method.GET;
                request.Timeout = Timeout;

                var fullUrl = client.BuildUri(request);
                var respone = client.DownloadData(request);

                return respone;
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
                if (newState == null) return;

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
                Dictionary<string, string> urlSegements = new Dictionary<string, string>();
                //get all files & folders 
                urlSegements.Add("recursive", recursive ? "true" : "false");

                var result = await SendRestApiRequestAsync(Method.GET, files, "", urlSegements).ConfigureAwait(false);
                var list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                if(list != null)
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
            ObservableCollection<OctoPrintModel> collectedFiles = new ObservableCollection<OctoPrintModel>();
            try
            {
                foreach (OctoPrintFile file in files)
                {
                    Stack<OctoPrintFile> fileStack = new Stack<OctoPrintFile>();
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
                var result = await SendRestApiRequestAsync(Method.GET, "printerprofiles").ConfigureAwait(false);
                var list = JsonConvert.DeserializeObject<OctoPrintPrinterProfiles>(result.Result);
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
        void UpdateWebClientInstance()
        {
            if (EnableProxy && !string.IsNullOrEmpty(ProxyAddress))
            {
                //var proxy = GetCurrentProxy();
                var handler = HttpHandler ?? new HttpClientHandler()
                {
                    UseProxy = true,
                    Proxy = GetCurrentProxy(),
                    AllowAutoRedirect = true,
                };

                client = new HttpClient(handler: handler, disposeHandler: true);
            }
            else
            {
                if (HttpHandler == null)
                    client = new HttpClient();
                else
                    client = new HttpClient(handler: HttpHandler, disposeHandler: true);
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
                    IsListeningToWebsocket = false;
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
            UpdateWebClientInstance();
            Instance = this;
        }
        public void SetProxy(bool Secure, string Address, int Port, string User = "", SecureString Password = null, bool Enable = true)
        {
            EnableProxy = Enable;
            ProxyUseDefaultCredentials = false;
            ProxyAddress = Address;
            ProxyPort = Port;
            ProxyUser = User;
            ProxyPassword = Password;
            UpdateWebClientInstance();
            Instance = this;
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
            Timer = new Timer(async (action) => {
                // Do not check the online state ever tick
                if (RefreshCounter > 5)
                {
                    RefreshCounter = 0;
                    await CheckOnlineAsync(3500).ConfigureAwait(false);
                }
                else RefreshCounter++;
                if (IsOnline)
                {
                    List<Task> tasks = new List<Task>()
                    {
                        //CheckServerOnlineAsync(),
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

            if (IsListeningToWebsocket)
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
                //await RefreshPrinterListAsync();
                List<Task> task = new List<Task>()
                {
                    RefreshConnectionSettingsAsync(),
                    RefreshCurrentPrintInfosAsync(),
                    RefreshPrinterStateAsync(),
                    RefreshFilesAsync(CurrentFileLocation),
                    //RefreshDiskSpaceAsync(),
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
        public async Task SetPrinterActiveAsync(int Index, bool RefreshPrinterList = true)
        {
            try
            {
                if (RefreshPrinterList)
                    await RefreshPrinterListAsync().ConfigureAwait(false);
                if (Printers.Count > Index)
                    ActivePrinter = Printers[Index];
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
                var printer = Printers.FirstOrDefault(prt => prt.Id == Id);
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
                if (client != null)
                {
                    client.CancelPendingRequests();
                    UpdateWebClientInstance();
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        #endregion

        #region WebCam
        public string GetWebCamUri(int CamIndex = 0)
        {
            try
            {
                string currentPrinter = GetActivePrinterSlug();
                if (string.IsNullOrEmpty(currentPrinter)) return string.Empty;

                //return string.Format("{0}{1}:{2}/printer/cammjpg/{3}?cam={4}&apikey={5}", httpProtocol, ServerAddress, Port, currentPrinter, CamIndex, API);
                //return string.Format("{0}{1}:{2}/webcam/?action=stream?t={3}", httpProtocol, ServerAddress, Port, API);
                return $"{FullWebAddress}/webcam/?action=stream?t={API}";
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
            if (IsConnecting) return; // Avoid multiple calls
            IsConnecting = true;
            bool isReachable = false;
            try
            {
                // Cancel after timeout
                var cts = new CancellationTokenSource(new TimeSpan(0, 0, 0, 0, Timeout));
                string uriString = FullWebAddress;

                try
                {
                    HttpResponseMessage response = await client.GetAsync(uriString, cts.Token).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    if (response != null)
                    {
                        isReachable = response.IsSuccessStatusCode;
                    }
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
                    // Throws exception on timeout, not actually an error
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            IsConnecting = false;
            // Avoid offline message for short connection loss
            if (isReachable || _retries > RetriesWhenOffline)
            {
                _retries = 0;
                IsOnline = isReachable;
            }
            else
            {
                // Retry with shorter timeout to see if the connection loss is real
                _retries++;
                await CheckOnlineAsync(2000).ConfigureAwait(false);
            }
        }

        public async Task<bool> CheckIfApiIsValidAsync(int Timeout = 10000)
        {
            try
            {
                if (IsOnline)
                {
                    // Send an empty command to check the respone
                    var respone = await SendRestApiRequestAsync(Method.GET, "version", Timeout).ConfigureAwait(false);
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
                    return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task CheckServerIfApiIsValidAsync(int Timeout = 10000)
        {
            await CheckIfApiIsValidAsync(Timeout).ConfigureAwait(false);
        }
        #endregion

        #region CheckForUpdates
        public async Task CheckForServerUpdateAsync()
        {
            OctoPrintApiRequestRespone result = new OctoPrintApiRequestRespone();
            try
            {
                result = await SendRestApiRequestAsync(Method.GET, "checkForUpdates").ConfigureAwait(false);
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
        public bool CheckIfConfigurationHasChanged(OctoPrintServer temp)
        {
            try
            {
                return
                    !(this.ServerAddress == temp.ServerAddress &&
                        this.Port == temp.Port &&
                        this.API == temp.API &&
                        this.IsSecure == temp.IsSecure
                        )
                    ;
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
                string command = "version";

                var result = await SendRestApiRequestAsync(Method.GET, command).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintVersionInfo>(result.Result);
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

                var result = await SendRestApiRequestAsync(Method.GET, command).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintConnectionSettings>(result.Result);
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
                object parameter = new
                {
                    command = "connect",
                    port = port,
                    baudrate = baudrate,
                    printerProfile = printerProfile,
                    save = save,
                    autoconnect = autoconnect
                };

                // no content result
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
                return string.IsNullOrEmpty(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var models = await GetFilesAsync(location, path, recursive).ConfigureAwait(false);
                if (models != null)
                {
                    ObservableCollection<OctoPrintModel> col = IterateOctoPrintFileStack(models.Children ?? models.Files);
                    return col;
                }
                else
                    return new ObservableCollection<OctoPrintModel>();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new ObservableCollection<OctoPrintModel>();
            }
        }
        public async Task RefreshFilesAsync(IProgress<int> Prog = null)
        {
            try
            {
                var modelDatas = new ObservableCollection<OctoPrintModel>();
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
        public async Task RefreshFilesAsync(string Location, IProgress<int> Prog = null)
        {
            try
            {
                var modelDatas = new ObservableCollection<OctoPrintModel>();
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
        public async Task RefreshFilesAsync(OctoPrintFileLocations Location, IProgress<int> Prog = null)
        {
            await RefreshFilesAsync(Location.ToString(), Prog);
        }


        public async Task<OctoPrintFile> GetFileAsync(string location, string filename)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", location, filename);

                // no content for this result
                var result = await SendRestApiRequestAsync(Method.GET, command).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintFile>(result.Result);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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

                var result = await sendRestAPIRequestAsync(command, Method.POST, parameter);
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
                object parameter = new { command = "copy", destination = destination };

                var result = await SendRestApiRequestAsync(Method.COPY, command, parameter).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintFileActionRespond>(result.Result);
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
                object parameter = new { command = "move", destination = destination };

                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintFileActionRespond>(result.Result);
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
                var result = await SendRestApiRequestAsync(Method.DELETE, command).ConfigureAwait(false);
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
                var result = await SendMultipartFormDataFileRestApiRequestAsync(location, target, filePath, select, print);
                if (result != null)
                {
                    var respone = JsonConvert.DeserializeObject<OctoPrintUploadFileRespone>(result.Result);
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

        public async Task<bool> UploadFileAsync(OctoPrintFileLocation location, string target, byte[] file, bool select = false, bool print = false)
        {
            try
            {
                var result = await SendMultipartFormDataFileRestApiRequestAsync(location, target, file, select, print);
                if (result != null)
                {
                    var respone = JsonConvert.DeserializeObject<OctoPrintUploadFileRespone>(result.Result);
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

        public byte[] DownloadFile(Uri dowmloadUri)
        {
            try
            {
                var result = DownloadFile(dowmloadUri);
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
                var result = await SendMultipartFormDataFolderCreationRestApiRequestAsync(location, name, path);
                if(result != null)
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
                Dictionary<string, string> urlSegements = new Dictionary<string, string>();
                urlSegements.Add("history", includeHistory ? "true" : "false");
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());
                if (excludes != null && excludes.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < excludes.Length; i++)
                    {
                        sb.Append(excludes[i]);
                        if (i < excludes.Length - 1)
                            sb.Append(",");
                    }
                    urlSegements.Add("exclude", sb.ToString());
                }

                var result = await SendRestApiRequestAsync(Method.GET, command, "", urlSegements).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintPrinterState>(result.Result);
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
                object parameter = new { 
                    command = "jog", 
                    x = double.IsInfinity(X) ? 0 : X, 
                    y = double.IsInfinity(Y) ? 0 : Y, 
                    z = double.IsInfinity(Z) ? 0 : Z, 
                    speed = Speed,
                    absolute = absolute ? "true" : "false" 
                };

                // no content for this result
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                List<string> axes = new List<string>();
                if (x) axes.Add("x");
                if (y) axes.Add("y");
                if (z) axes.Add("z");

                string command = "printer/printhead";
                object parameter = new { command = "home", axes = axes };

                // no content for this result
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                Dictionary<string, string> urlSegements = new Dictionary<string, string>();
                urlSegements.Add("history", includeHistory ? "true" : "false");
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());

                var result = await SendRestApiRequestAsync(Method.GET, command, "", urlSegements).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintToolState>(result.Result);
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
                if(tool0 != int.MinValue && tool1 != int.MinValue)
                    parameter = new { command = "target", targets = new { tool0 = tool0, tool1 = tool1 } };
                else if (tool0 != int.MinValue)
                    parameter = new { command = "target", targets = new { tool0 = tool0 } };
                else
                    parameter = new { command = "target", targets = new { tool1 = tool1 } };

                // no content for this result
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                Dictionary<string, string> urlSegements = new Dictionary<string, string>();
                urlSegements.Add("history", includeHistory ? "true" : "false");
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());

                var result = await SendRestApiRequestAsync(Method.POST, command, "", urlSegements).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintBedState>(result.Result);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                Dictionary<string, string> urlSegements = new Dictionary<string, string>();
                urlSegements.Add("history", includeHistory ? "true" : "false");
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());

                var result = await SendRestApiRequestAsync(Method.POST, command, "", urlSegements).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintChamberState>(result.Result);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                Dictionary<string, string> urlSegements = new Dictionary<string, string>();
                urlSegements.Add("history", includeHistory ? "true" : "false");
                if (limit > 0)
                    urlSegements.Add("limit", limit.ToString());

                var result = await SendRestApiRequestAsync(Method.GET, command, "", urlSegements).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintPrinterStateSd>(result.Result);
                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintPrinterStateSd();
            }
        }
        public async Task<bool> InitSdCardAsync(int target)
        {
            try
            {
                string command = "printer/sd";
                object parameter = new { command = "init" };

                // no content for this result
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> RefreshSdCardAsync(int target)
        {
            try
            {
                string command = "printer/sd";
                object parameter = new { command = "refresh" };

                // no content for this result
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
                return true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> ReleaseSdCardAsync(int target)
        {
            try
            {
                string command = "printer/sd";
                object parameter = new { command = "release" };

                // no content for this result
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
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

                var result = await SendRestApiRequestAsync(Method.GET, command, "", urlSegements).ConfigureAwait(false);
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
                var result = await GetPrinterProfilesAsync().ConfigureAwait(false);
                var profile = new ObservableCollection<OctoPrintPrinter>(result.Profiles.Select(pair => pair.Value));
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
                var result = await GetPrinterProfilesAsync().ConfigureAwait(false);
                var profile = new ObservableCollection<OctoPrintPrinter>(result.Profiles.Select(pair => pair.Value)).FirstOrDefault(prof => prof.Id == Id);
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

                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
                var list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
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

                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
                var list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
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

                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
                var list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
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

                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
                var list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
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

                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
                var list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
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

                var result = await SendRestApiRequestAsync(Method.POST, command, parameter).ConfigureAwait(false);
                var list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
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

                var result = await SendRestApiRequestAsync(Method.GET, command).ConfigureAwait(false);
                var list = JsonConvert.DeserializeObject<OctoPrintJobInfo>(result.Result);
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
                var result = await SendRestApiRequestAsync(Method.GET, command, null).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintSettings>(result.Result);
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
                var result = await SendRestApiRequestAsync(Method.POST, command, SettingsTreeObject).ConfigureAwait(false);
                var respone = JsonConvert.DeserializeObject<OctoPrintSettings>(result.Result);
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

        #region Static
        public static string ConvertStackToPath(Stack<string> stack, string separator)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = stack.Count - 1; i >= 0; i--)
            {
                sb.Append(stack.ElementAt(i));
                if (i > 0)
                    sb.Append(separator);
            }
            return sb.ToString();
        }

        public static OctoPrintConnectionStates ConvertConnectionStateString(string ConnectionState)
        {
            try
            {
                OctoPrintConnectionStates state = OctoPrintConnectionStates.Unkown;
                string cropped = ConnectionState.Replace(" ", string.Empty);
                System.Enum.TryParse(cropped, out state);
                if(state == OctoPrintConnectionStates.Unkown)
                {
                    // Just for debugging
                }
                return state;
            }
            catch(Exception exc)
            {
                var msg = exc.Message;
                return OctoPrintConnectionStates.Unkown;
            }
        }
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
            var item = obj as OctoPrintServer;
            if (item == null)
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

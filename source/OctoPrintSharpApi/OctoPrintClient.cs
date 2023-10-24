using AndreasReitberger.API.OctoPrint.Enum;
using AndreasReitberger.API.OctoPrint.Interfaces;
using AndreasReitberger.API.OctoPrint.Models;
using AndreasReitberger.API.OctoPrint.Structs;
using AndreasReitberger.API.Print3dServer.Core;
using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Events;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace AndreasReitberger.API.OctoPrint
{
    //http://docs.octoprint.org/en/master/api/
    public partial class OctoPrintClient : Print3dServerClient, IPrintServerClient
    {

        #region Instance
        static OctoPrintClient _instance = null;
        static readonly object Lock = new();
        public new static OctoPrintClient Instance
        {
            get
            {
                lock (Lock)
                {
                    _instance ??= new OctoPrintClient();
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

        #endregion

        #region Properties

        #region General

        [JsonIgnore, XmlIgnore]
        [ObservableProperty]
        object _update;

        #endregion

        #region PrinterStateInformation
        //[JsonProperty(nameof(LastFlowRate))]
        [ObservableProperty]
        double lastFlowRate = 100;

        //[JsonProperty(nameof(LastFeedRate))]
        [ObservableProperty]
        double lastFeedRate = 100;

        //[JsonProperty(nameof(CurrentFileLocation))]
        [ObservableProperty]
        OctoPrintFileLocations currentFileLocation = OctoPrintFileLocations.local;
        #endregion

        #region State & Config
        [JsonIgnore, XmlIgnore]
        OctoPrintSettings config;
        [JsonIgnore, XmlIgnore]
        public OctoPrintSettings Config
        {
            get => config;
            set
            {
                if (config == value) return;
                config = value;
                OnOctoPrintPrinterConfigChanged(new OctoPrintPrinterConfigChangedEventArgs()
                {
                    NewConfiguration = value,
                    SessionId = SessionId,
                    CallbackId = -1,
                    Printer = GetActivePrinterSlug()
                });
                UpdatePrinterConfig(value);
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        OctoPrintConnectionSettings connectionSettings;
        [JsonIgnore, XmlIgnore]
        public OctoPrintConnectionSettings ConnectionSettings
        {
            get => connectionSettings;
            set
            {
                if (connectionSettings == value) return;
                connectionSettings = value;
                OnOctoPrintConnectionSettingsChanged(new OctoPrintConnectionSettingsChangedEventArgs()
                {
                    NewConnectionSettings = value,
                    SessionId = SessionId,
                    CallbackId = -1,
                    Printer = GetActivePrinterSlug(),
                });
                UpdateConnectionSettings(value);
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        OctoPrintPrinterState state;
        [JsonIgnore, XmlIgnore]
        public OctoPrintPrinterState State
        {
            get => state;
            set
            {
                if (state == value) return;
                state = value;
                OnOctoPrintPrinterStateChanged(new OctoPrintPrinterStateChangedEventArgs()
                {
                    NewPrinterState = value,
                    SessionId = SessionId,
                    CallbackId = -1,
                    Printer = GetActivePrinterSlug(),
                });
                UpdatePrinterState(value);
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        OctoPrintJobInfo activePrintInfo;
        [JsonIgnore, XmlIgnore]
        public OctoPrintJobInfo ActivePrintInfo
        {
            get => activePrintInfo;
            set
            {
                if (activePrintInfo == value) return;
                activePrintInfo = value;
                OnPrintInfoChanged(new OctoPrintActivePrintInfoChangedEventArgs()
                {
                    SessionId = SessionId,
                    NewActivePrintInfo = value,
                    Printer = GetActivePrinterSlug(),
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore, Obsolete("Use Toolheads instead")]
        [ObservableProperty]
        ObservableCollection<OctoPrintPrinterStateToolheadInfo> extruders = new();

        [JsonIgnore, XmlIgnore, Obsolete("Use base.HeatedBeds instead")]
        [ObservableProperty]
        ObservableCollection<OctoPrintPrinterStateTemperatureInfo> heatedBeds = new();

        [JsonIgnore, XmlIgnore, Obsolete("Use base.HeatedChambers instead")]
        [ObservableProperty]
        ObservableCollection<OctoPrintPrinterStateTemperatureInfo> heatedChambers = new();
        #endregion

        #region Models
        /* */
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
                    SessionId = SessionId,
                    CallbackId = -1,
                    Printer = GetActivePrinterSlug(),
                });
                OnPropertyChanged();
            }
        }
       
        #endregion

        #region ReadOnly

        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        public new bool IsReady
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

        #region Constructor
        public OctoPrintClient()
        {
            Id = Guid.NewGuid();
            Target = Print3dServerTarget.OctoPrint;
            ApiKeyRegexPattern = "";
            WebSocketTarget = "/sockjs/websocket";
            WebSocketMessageReceived += Client_WebSocketMessageReceived;
            UpdateRestClientInstance();
        }
        public OctoPrintClient(string serverAddress, string api, int port = 80, bool isSecure = false)
        {
            Id = Guid.NewGuid();
            Target = Print3dServerTarget.OctoPrint;
            ApiKeyRegexPattern = "";
            WebSocketTarget = "/sockjs/websocket";
            WebSocketMessageReceived += Client_WebSocketMessageReceived;
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
            WebSocketMessageReceived -= Client_WebSocketMessageReceived;
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

        #region Methods

        #region Private

        #region RestApi

        [Obsolete("Use method from Core library instead")]
        async Task<IRestApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            string filePath,
            string location,
            string path,
            bool selectFile = false,
            bool printFile = false,
            int timeout = 100000
            )
        {
            IRestApiRequestRespone apiResponseResult = null;
            if (!IsOnline) return apiResponseResult;

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
                    RestResponse response = await restClient.ExecuteAsync(request, cts.Token);
                    apiResponseResult = ValidateResponse(response, fullUri);
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
            return apiResponseResult;
        }

        [Obsolete("Use method from Core library instead")]
        Task<IRestApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            string filePath,
            OctoPrintFileLocation location,
            string path,
            bool selectFile = false,
            bool printFile = false,
            int timeout = 100000
            ) => SendMultipartFormDataFileRestApiRequestAsync(filePath, location.Location, path, selectFile, printFile, timeout);

        [Obsolete("Use method from Core library instead")]
        async Task<IRestApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            byte[] file,
            string fileName,
            string location,
            string path = "",
            bool selectFile = false,
            bool printFile = false,
            int timeout = 100000
            )
        {
            IRestApiRequestRespone apiResponseResult = new RestApiRequestRespone();
            if (!IsOnline) return apiResponseResult;

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
                    RestResponse response = await restClient.ExecuteAsync(request, cts.Token);
                    apiResponseResult = ValidateResponse(response, fullUri);
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
            return apiResponseResult;
        }

        [Obsolete("Use method from Core library instead")]
        Task<IRestApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            byte[] file,
            string fileName,
            OctoPrintFileLocation location,
            string path = "",
            bool selectFile = false,
            bool printFile = false,
            int timeout = 100000
            ) => SendMultipartFormDataFileRestApiRequestAsync(file, fileName, location.Location, path, selectFile, printFile, timeout);


        [Obsolete("Use method from Core library instead")]
        async Task<IRestApiRequestRespone> SendMultipartFormDataFolderRestApiRequestAsync(
            string folderName,
            string location,
            string path,
            int timeout = 100000
            )
        {
            IRestApiRequestRespone apiResponseResult = new RestApiRequestRespone();
            if (!IsOnline) return apiResponseResult;

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
                    RestResponse response = await restClient.ExecuteAsync(request, cts.Token);
                    apiResponseResult = ValidateResponse(response, fullUri);
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
            return apiResponseResult;
        }


        [Obsolete("Use method from Core library instead")]
        Task<IRestApiRequestRespone> SendMultipartFormDataFolderRestApiRequestAsync(
            string folderName,
            OctoPrintFileLocation location,
            string path,
            int timeout = 100000
            ) => SendMultipartFormDataFolderRestApiRequestAsync(folderName, location.Location, path, timeout);
        #endregion

        #region Download
        [Obsolete("Check if can be replaced by base method")]
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
                byte[] response = await restClient.DownloadDataAsync(request, cts.Token)
                    .ConfigureAwait(false)
                    ;

                return response;
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
                Dictionary<string, string> urlSegments = new()
                {
                    //get all files & folders 
                    { "recursive", recursive ? "true" : "false" }
                };

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: files,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, files, jsonObject: null, cts: default, urlSegments)
                    .ConfigureAwait(false);
                */
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
        ObservableCollection<OctoPrintModel> IterateOctoPrintFileStack(IGcode[] files) =>  IterateOctoPrintFileStack(files.ToList());      

        ObservableCollection<OctoPrintModel> IterateOctoPrintFileStack(List<IGcode> files)
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
                        Name = file.FileName,
                        Type = file.Type,
                        Location = file.Origin,
                        Path = file.FilePath,

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
                                Name = f.FileName,
                                Type = f.Type,
                                Location = f.Origin,
                                Path = f.FilePath,

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

        #endregion

        #region Public

        #region Refresh

        public new Task StartListeningAsync(bool stopActiveListening = false) => StartListeningAsync(WebSocketTargetUri, stopActiveListening, () => Task.Run(async() =>
        {
            List<Task> tasks = new()
            {
                RefreshPrinterStateAsync(),
                RefreshCurrentPrintInfosAsync(),
                RefreshConnectionSettingsAsync(),
            };
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }));

        public new async Task RefreshAllAsync()
        {
            try
            {
                await base.RefreshAllAsync().ConfigureAwait(false);
                // Avoid multiple calls
                if (IsRefreshing) return;
                IsRefreshing = true;

                List<Task> tasks = new()
                {
                    RefreshConnectionSettingsAsync(),
                    RefreshCurrentPrintInfosAsync(),
                    RefreshPrinterStateAsync(),
                    RefreshFilesAsync(CurrentFileLocation),
                };
                await Task.WhenAll(tasks).ConfigureAwait(false);
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

        #region CheckOnline

        public async Task CheckOnlineAsync(int Timeout = 10000)
        {
            CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, Timeout));
            await CheckOnlineAsync(cts).ConfigureAwait(false);
            cts?.Dispose();
        }

        public Task CheckOnlineAsync(CancellationTokenSource cts) => CheckOnlineAsync($"{OctoPrintCommands.Base}", AuthHeaders, "version", cts);
        
        public Task<bool> CheckIfApiIsValidAsync(int timeout = 10000) => CheckIfApiIsValidAsync($"{OctoPrintCommands.Base}", AuthHeaders, "version", timeout);
       
        public Task CheckServerIfApiIsValidAsync(int Timeout = 10000) => CheckIfApiIsValidAsync(Timeout);
        
        #endregion

        #region CheckForUpdates
        public async Task CheckForServerUpdateAsync()
        {
            IRestApiRequestRespone result = new RestApiRequestRespone();
            try
            {
                string targetUri = $"{OctoPrintCommands.Api}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "checkForUpdates",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, "checkForUpdates")
                    .ConfigureAwait(false);
                */
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
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
                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, "version")
                    .ConfigureAwait(false);
                */
                OctoPrintVersionInfo response = JsonConvert.DeserializeObject<OctoPrintVersionInfo>(result.Result);
                return response;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command)
                    .ConfigureAwait(false);
                */
                OctoPrintConnectionSettings response = JsonConvert.DeserializeObject<OctoPrintConnectionSettings>(result.Result);
                return response;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content result
                /*
                OctoPrintApiRequestResponse result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return string.IsNullOrEmpty(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public Task<bool> ConnectPrinterAsync(string printerProfile, bool save, bool autoconnect, string port = "", long baudRate = -1) 
            => ConnectPrinterAsync(port, baudRate, printerProfile, save, autoconnect);
          

        public async Task<bool> DisconnectPrinterAsync()
        {
            try
            {
                string command = "connection";
                object parameter = new
                {
                    command = "disconnect",
                };

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return string.IsNullOrEmpty(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion
     
        #region PrinterOperations
       
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                LastFeedRate = feedRate;
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                LastFeedRate = feedRate;
                return result.Succeeded;
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
                Dictionary<string, string> urlSegments = new()
                {
                    { "history", includeHistory ? "true" : "false" }
                };
                if (limit > 0)
                    urlSegments.Add("limit", limit.ToString());

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command, jsonObject: null, cts: default, urlSegments)
                    .ConfigureAwait(false);
                */
                OctoPrintToolState response = JsonConvert.DeserializeObject<OctoPrintToolState>(result.Result);
                return response;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintToolState();
            }
        }
        public Task<bool> SetToolTemperatureAsync(long tool0, long tool1 = 0) => SetToolTemperatureAsync(Convert.ToInt32(tool0), Convert.ToInt32(tool1));
        
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                LastFlowRate = flowRate;
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                LastFlowRate = flowRate;
                return result.Succeeded;
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
                Dictionary<string, string> urlSegments = new()
                {
                    { "history", includeHistory ? "true" : "false" }
                };
                if (limit > 0)
                    urlSegments.Add("limit", limit.ToString());

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, jsonObject: null, cts: default, urlSegments)
                    .ConfigureAwait(false);
                */
                OctoPrintBedState response = JsonConvert.DeserializeObject<OctoPrintBedState>(result.Result);
                return response;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintBedState();
            }
        }
        public Task<bool> SetBedTemperatureAsync(long target) => SetBedTemperatureAsync(Convert.ToInt32(target));
        
        public async Task<bool> SetBedTemperatureAsync(int target)
        {
            try
            {
                string command = "printer/bed";
                object parameter = new { command = "target", target = target };

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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
                Dictionary<string, string> urlSegments = new()
                {
                    { "history", includeHistory ? "true" : "false" }
                };
                if (limit > 0)
                    urlSegments.Add("limit", limit.ToString());

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, jsonObject: null, cts: default, urlSegments)
                    .ConfigureAwait(false);
                */
                OctoPrintChamberState response = JsonConvert.DeserializeObject<OctoPrintChamberState>(result.Result);
                return response;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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
                Dictionary<string, string> urlSegments = new()
                {
                    { "history", includeHistory ? "true" : "false" }
                };
                if (limit > 0)
                    urlSegments.Add("limit", limit.ToString());

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command, jsonObject: null, cts: default, urlSegments)
                    .ConfigureAwait(false);
                */
                OctoPrintPrinterStateSd response = JsonConvert.DeserializeObject<OctoPrintPrinterStateSd>(result.Result);
                return response;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return result.Succeeded;
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
                Dictionary<string, string> urlSegments = new Dictionary<string, string>();
                urlSegments.Add("history", includeHistory ? "true" : "false");
                if (limit > 0)
                    urlSegments.Add("limit", limit.ToString());

                var result = await SendRestApiRequestAsync(Method.Get, command, "", urlSegments).ConfigureAwait(false);
                var response = JsonConvert.DeserializeObject<OctoPrintPrinterStateSd>(result.Result);
                return response;
                
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintPrinterStateSd();
            }
            
        }*/
        #endregion

        #endregion

        #region Job operations
        public async Task<bool> StartJobAsync()
        {
            try
            {
                string command = "job";
                object parameter = new { command = "start" };
                //string parameter = string.Format("{{\"command\":{0}}}", "start");

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result?.Result);
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                return result.Succeeded;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> TogglePauseJobAsync()
        {
            try
            {
                string command = "job";
                object parameter = new { command = "pause", action = "toggle" };
                //string parameter = string.Format("{{\"command\":{0}, \"action\":{1}}}", "pause", "toggle");

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                OctoPrintFiles list = JsonConvert.DeserializeObject<OctoPrintFiles>(result.Result);
                return result.Succeeded;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command)
                    .ConfigureAwait(false);
                */
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command)
                    .ConfigureAwait(false);
                */
                OctoPrintSettings response = JsonConvert.DeserializeObject<OctoPrintSettings>(result.Result);
                return response;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, SettingsTreeObject)
                    .ConfigureAwait(false);
                */
                OctoPrintSettings response = JsonConvert.DeserializeObject<OctoPrintSettings>(result.Result);
                return result.Succeeded;
                //return response;
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
            return Id.Equals(item.Id);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

    }
}

using AndreasReitberger.API.OctoPrint.Enum;
using AndreasReitberger.API.OctoPrint.Models;
using AndreasReitberger.API.OctoPrint.Structs;
using AndreasReitberger.API.Print3dServer.Core;
using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Events;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace AndreasReitberger.API.OctoPrint
{
    //http://docs.octoprint.org/en/master/api/
    public partial class OctoPrintClient : Print3dServerClient, IPrint3dServerClient
    {

        #region Instance
        static OctoPrintClient? _instance = null;
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

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        object? update;

        #endregion

        #region PrinterStateInformation
        [ObservableProperty]
        double lastFlowRate = 100;

        [ObservableProperty]
        double lastFeedRate = 100;

        [ObservableProperty]
        OctoPrintFileLocations currentFileLocation = OctoPrintFileLocations.local;
        #endregion

        #region State & Config
        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        OctoPrintSettings? config;
        partial void OnConfigChanged(OctoPrintSettings? value)
        {
            OnOctoPrintPrinterConfigChanged(new OctoPrintPrinterConfigChangedEventArgs()
            {
                NewConfiguration = value,
                SessionId = SessionId,
                CallbackId = -1,
                Printer = GetActivePrinterSlug()
            });
            UpdatePrinterConfig(value);
        }
     
        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        OctoPrintConnectionSettings? connectionSettings;
        partial void OnConnectionSettingsChanged(OctoPrintConnectionSettings? value)
        {
            OnOctoPrintConnectionSettingsChanged(new OctoPrintConnectionSettingsChangedEventArgs()
            {
                NewConnectionSettings = value,
                SessionId = SessionId,
                CallbackId = -1,
                Printer = GetActivePrinterSlug(),
            });
            UpdateConnectionSettings(value);
        }

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        OctoPrintPrinterState? state;
        partial void OnStateChanged(OctoPrintPrinterState? value)
        {
            OnOctoPrintPrinterStateChanged(new OctoPrintPrinterStateChangedEventArgs()
            {
                NewPrinterState = value,
                SessionId = SessionId,
                CallbackId = -1,
                Printer = GetActivePrinterSlug(),
            });
            UpdatePrinterState(value);
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
            if (WebSocket is not null)
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

                if (Instance is not null)
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

        #region Download
        public Task<byte[]?> DownloadFileFromUriAsync(string path)
            => DownloadFileFromUriAsync(path: path, authHeaders: AuthHeaders, urlSegments: null, timeout: 100000);
        public Task<byte[]?> DownloadFileFromUriAsync(string path, int timeout = 100000)
            => DownloadFileFromUriAsync(path: path, authHeaders: AuthHeaders, urlSegments: null, timeout: timeout);
        #endregion

        #region StateUpdates
        void UpdatePrinterState(OctoPrintPrinterState? newState)
        {
            try
            {
                if (newState is null || newState?.Temperature is null)
                {
                    return;
                }
                ConcurrentDictionary<int, IToolhead> toolheads = [];
                if (newState.Temperature?.Tool0 is not null) toolheads.TryAdd(0, newState.Temperature.Tool0);
                if (newState.Temperature?.Tool1 is not null) toolheads.TryAdd(1, newState.Temperature.Tool1);
                Toolheads = toolheads;
                /*
                List<OctoPrintPrinterStateToolheadInfo> extruders = [];
                if (newState.Temperature?.Tool0 is not null) extruders.Add(newState.Temperature.Tool0);
                if (newState.Temperature?.Tool1 is not null) extruders.Add(newState.Temperature.Tool1);
                Extruders = [.. extruders];
                */

                ConcurrentDictionary<int, IHeaterComponent> heatedBeds = [];
                if (newState.Temperature?.Bed is not null) heatedBeds.TryAdd(0, newState.Temperature.Bed);
                HeatedBeds = heatedBeds;
                /*
                List<OctoPrintPrinterStateTemperatureInfo> heatedBeds = [];
                if (newState.Temperature?.Bed is not null) heatedBeds.Add(newState.Temperature.Bed);
                HeatedBeds = [..  heatedBeds];
                */

                ConcurrentDictionary<int, IHeaterComponent> heatedChambers = [];
                if (newState.Temperature?.Chamber is not null) heatedChambers.TryAdd(0, newState.Temperature.Chamber);
                HeatedChambers = heatedChambers;
                /*
                List<OctoPrintPrinterStateTemperatureInfo> heatedChambers = [];
                if (newState.Temperature?.Chamber is not null) heatedChambers.Add(newState.Temperature.Chamber);
                HeatedChambers = [.. heatedChambers];
                */             
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        void UpdatePrinterConfig(OctoPrintSettings? newConfig)
        {
            try
            {
                if (newConfig is null) return;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        void UpdateConnectionSettings(OctoPrintConnectionSettings? newConnectionSettings)
        {
            try
            {
                if (newConnectionSettings is null) return;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        #endregion

        #region Models
        async Task<OctoPrintFiles?> GetFilesAsync(string location, string path = "", bool recursive = true)
        {
            try
            {
                string files = string.IsNullOrEmpty(path) ? $"files/{location}" : $"files/{location}/{path}";
                Dictionary<string, string> urlSegments = new()
                {
                    //get all files & folders 
                    { "recursive", recursive ? "true" : "false" }
                };

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintFiles? list = GetObjectFromJson<OctoPrintFiles>(result?.Result);
                if (list is not null)
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
        protected List<OctoPrintModel> IterateOctoPrintFileStack(IGcode[] files) =>  IterateOctoPrintFileStack(files.ToList());

        protected List<OctoPrintModel> IterateOctoPrintFileStack(List<IGcode>? files)
        {
            List<OctoPrintModel> collectedFiles = [];
            try
            {
                if (files is null)
                {
                    return collectedFiles;
                }
                foreach (OctoPrintFile file in files.Cast<OctoPrintFile>())
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
                        if (currentFile.Children is null) continue;
                        foreach (OctoPrintFile f in currentFile.Children.Cast<OctoPrintFile>())
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

        public new Task StartListeningAsync(bool stopActiveListening = false, string[]? commandsOnConnect = null) => StartListeningAsync(WebSocketTargetUri, stopActiveListening, () => Task.Run(async() =>
        {
            List<Task> tasks = new()
            {
                RefreshPrinterStateAsync(),
                RefreshCurrentPrintInfosAsync(),
                RefreshConnectionSettingsAsync(),
            };
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }), commandsOnConnect: commandsOnConnect);

        public new async Task RefreshAllAsync()
        {
            try
            {
                await base.RefreshAllAsync().ConfigureAwait(false);
                // Avoid multiple calls
                if (IsRefreshing) return;
                IsRefreshing = true;

                List<Task> tasks =
                [
                    RefreshConnectionSettingsAsync(),
                    RefreshCurrentPrintInfosAsync(),
                    RefreshPrinterStateAsync(),
                    RefreshFilesAsync(CurrentFileLocation),
                ];
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

        public Task CheckOnlineAsync(CancellationTokenSource cts) => CheckOnlineAsync($"{OctoPrintCommands.Base}", AuthHeaders, "version", cts);
        
        public Task<bool> CheckIfApiIsValidAsync(int timeout = 10000) => CheckIfApiIsValidAsync($"{OctoPrintCommands.Base}", AuthHeaders, "version", timeout);
       
        public Task CheckServerIfApiIsValidAsync(int Timeout = 10000) => CheckIfApiIsValidAsync(Timeout);
        
        #endregion

        #region CheckForUpdates
        public async Task CheckForServerUpdateAsync()
        {
            IRestApiRequestRespone? result = new RestApiRequestRespone();
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
                    OriginalString = result?.Result,
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
        public async Task<OctoPrintVersionInfo?> GetVersionInfoAsync()
        {
            try
            {
                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintVersionInfo? response = GetObjectFromJson<OctoPrintVersionInfo>(result?.Result);
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
        public async Task<OctoPrintConnectionSettings?> GetConnectionSettingsAsync()
        {
            try
            {
                string command = "connection";

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintConnectionSettings? response = GetObjectFromJson<OctoPrintConnectionSettings>(result?.Result, NewtonsoftJsonSerializerSettings);
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return string.IsNullOrEmpty(result?.Result);
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return string.IsNullOrEmpty(result?.Result);
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Tool
        public async Task<OctoPrintToolState?> GetCurrentToolStateAsync(bool includeHistory, int limit = 0)
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintToolState? response = GetObjectFromJson<OctoPrintToolState>(result?.Result, NewtonsoftJsonSerializerSettings);
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
                object? parameter = null;
                if (tool0 != int.MinValue && tool1 != int.MinValue)
                    parameter = new { command = "target", targets = new { tool0 = tool0, tool1 = tool1 } };
                else if (tool0 != int.MinValue)
                    parameter = new { command = "target", targets = new { tool0 = tool0 } };
                else
                    parameter = new { command = "target", targets = new { tool1 = tool1 } };

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Bed
        public async Task<OctoPrintBedState?> GetCurrentBedStateAsync(bool includeHistory, int limit = 0)
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintBedState? response = GetObjectFromJson<OctoPrintBedState>(result?.Result, NewtonsoftJsonSerializerSettings);
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return string.IsNullOrEmpty(result?.Result);
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Chamber
        public async Task<OctoPrintChamberState?> GetCurrentChamberStateAsync(bool includeHistory, int limit = 0)
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintChamberState? response = GetObjectFromJson<OctoPrintChamberState>(result?.Result, NewtonsoftJsonSerializerSettings);
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Sd
        public async Task<OctoPrintPrinterStateSd?> GetCurrentSdState(bool includeHistory, int limit = 0)
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintPrinterStateSd? response = GetObjectFromJson<OctoPrintPrinterStateSd>(result?.Result, NewtonsoftJsonSerializerSettings);
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return result?.Succeeded ?? false;
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
                var response = GetObjectFromJson<OctoPrintPrinterStateSd>(result?.Result, NewtonsoftJsonSerializerSettings);
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintFiles? list = GetObjectFromJson<OctoPrintFiles>(result?.Result);
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintFiles? list = GetObjectFromJson<OctoPrintFiles>(result?.Result, NewtonsoftJsonSerializerSettings);
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintFiles? list = GetObjectFromJson<OctoPrintFiles>(result?.Result, NewtonsoftJsonSerializerSettings);
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintFiles? list = GetObjectFromJson<OctoPrintFiles>(result?.Result, NewtonsoftJsonSerializerSettings);
                return result?.Succeeded ?? false;
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

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintFiles? list = GetObjectFromJson<OctoPrintFiles>(result?.Result, NewtonsoftJsonSerializerSettings);
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintFiles? list = GetObjectFromJson<OctoPrintFiles>(result?.Result, NewtonsoftJsonSerializerSettings);
                return result?.Succeeded ?? false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<OctoPrintJobInfo?> GetJobInfoAsync()
        {
            try
            {
                string command = "job";
                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintJobInfo? list = GetObjectFromJson<OctoPrintJobInfo>(result?.Result, NewtonsoftJsonSerializerSettings);
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
                //ActivePrintInfo = await GetJobInfoAsync().ConfigureAwait(false);
                ActiveJob = await GetJobInfoAsync().ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                //ActivePrintInfo = null;
                ActiveJob = null;
            }

        }
        #endregion

        #region Settings
        public async Task<OctoPrintSettings?> GetSettingsAsync()
        {
            try
            {
                string command = "settings";
                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintSettings? response = GetObjectFromJson<OctoPrintSettings>(result?.Result, NewtonsoftJsonSerializerSettings);
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: SettingsTreeObject,
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
                OctoPrintSettings? response = GetObjectFromJson<OctoPrintSettings>(result?.Result, NewtonsoftJsonSerializerSettings);
                return result?.Succeeded ?? false;
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
        public override bool Equals(object? obj)
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

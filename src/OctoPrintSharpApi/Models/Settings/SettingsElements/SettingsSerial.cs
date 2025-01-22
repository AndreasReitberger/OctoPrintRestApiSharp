using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsSerial : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("abortHeatupOnCancel", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? AbortHeatupOnCancel { get; set; }

        [ObservableProperty]
        
        [JsonProperty("ackMax", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? AckMax { get; set; }

        [ObservableProperty]
        
        [JsonProperty("additionalBaudrates", NullValueHandling = NullValueHandling.Ignore)]
        public partial object[] AdditionalBaudrates { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("additionalPorts", NullValueHandling = NullValueHandling.Ignore)]
        public partial object[] AdditionalPorts { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("alwaysSendChecksum", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? AlwaysSendChecksum { get; set; }

        [ObservableProperty]
        
        [JsonProperty("autoconnect", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Autoconnect { get; set; }

        [ObservableProperty]
        
        [JsonProperty("baudrate", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Baudrate { get; set; }

        [ObservableProperty]
        
        [JsonProperty("baudrateOptions", NullValueHandling = NullValueHandling.Ignore)]
        public partial long[] BaudrateOptions { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("blockWhileDwelling", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? BlockWhileDwelling { get; set; }

        [ObservableProperty]
        
        [JsonProperty("blockedCommands", NullValueHandling = NullValueHandling.Ignore)]
        public partial string[] BlockedCommands { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("capAutoreportSdStatus", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? CapAutoreportSdStatus { get; set; }

        [ObservableProperty]
        
        [JsonProperty("capAutoreportTemp", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? CapAutoreportTemp { get; set; }

        [ObservableProperty]
        
        [JsonProperty("capBusyProtocol", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? CapBusyProtocol { get; set; }

        [ObservableProperty]
        
        [JsonProperty("capEmergencyParser", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? CapEmergencyParser { get; set; }

        [ObservableProperty]
        
        [JsonProperty("checksumRequiringCommands", NullValueHandling = NullValueHandling.Ignore)]
        public partial string[] ChecksumRequiringCommands { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("disconnectOnErrors", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? DisconnectOnErrors { get; set; }

        [ObservableProperty]
        
        [JsonProperty("emergencyCommands", NullValueHandling = NullValueHandling.Ignore)]
        public partial string[] EmergencyCommands { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("exclusive", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Exclusive { get; set; }

        [ObservableProperty]
        
        [JsonProperty("externalHeatupDetection", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? ExternalHeatupDetection { get; set; }

        [ObservableProperty]
        
        [JsonProperty("firmwareDetection", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? FirmwareDetection { get; set; }

        [ObservableProperty]
        
        [JsonProperty("helloCommand", NullValueHandling = NullValueHandling.Ignore)]
        public partial string HelloCommand { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("ignoreErrorsFromFirmware", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? IgnoreErrorsFromFirmware { get; set; }

        [ObservableProperty]
        
        [JsonProperty("ignoreIdenticalResends", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? IgnoreIdenticalResends { get; set; }

        [ObservableProperty]
        
        [JsonProperty("log", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Log { get; set; }

        [ObservableProperty]
        
        [JsonProperty("logPositionOnCancel", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? LogPositionOnCancel { get; set; }

        [ObservableProperty]
        
        [JsonProperty("logPositionOnPause", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? LogPositionOnPause { get; set; }

        [ObservableProperty]
        
        [JsonProperty("longRunningCommands", NullValueHandling = NullValueHandling.Ignore)]
        public partial string[] LongRunningCommands { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("maxTimeoutsIdle", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? MaxTimeoutsIdle { get; set; }

        [ObservableProperty]
        
        [JsonProperty("maxTimeoutsLong", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? MaxTimeoutsLong { get; set; }

        [ObservableProperty]
        
        [JsonProperty("maxTimeoutsPrinting", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? MaxTimeoutsPrinting { get; set; }

        [ObservableProperty]
        
        [JsonProperty("neverSendChecksum", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? NeverSendChecksum { get; set; }

        [ObservableProperty]
        
        [JsonProperty("pausingCommands", NullValueHandling = NullValueHandling.Ignore)]
        public partial string[] PausingCommands { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Port { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("portOptions", NullValueHandling = NullValueHandling.Ignore)]
        public partial string[] PortOptions { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("repetierTargetTemp", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? RepetierTargetTemp { get; set; }

        [ObservableProperty]
        
        [JsonProperty("sdAlwaysAvailable", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? SdAlwaysAvailable { get; set; }

        [ObservableProperty]
        
        [JsonProperty("sdRelativePath", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? SdRelativePath { get; set; }

        [ObservableProperty]
        
        [JsonProperty("sendM112OnError", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? SendM112OnError { get; set; }

        [ObservableProperty]
        
        [JsonProperty("supportResendsWithoutOk", NullValueHandling = NullValueHandling.Ignore)]
        public partial string SupportResendsWithoutOk { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("swallowOkAfterResend", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? SwallowOkAfterResend { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutBaudrateDetectionPause", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutBaudrateDetectionPause { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutCommunication", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutCommunication { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutCommunicationBusy", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutCommunicationBusy { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutConnection", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutConnection { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutDetection", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutDetection { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutPositionLogWait", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutPositionLogWait { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutSdStatus", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutSdStatus { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutSdStatusAutoreport", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutSdStatusAutoreport { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutTemperature", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutTemperature { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutTemperatureAutoreport", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutTemperatureAutoreport { get; set; }

        [ObservableProperty]
        
        [JsonProperty("timeoutTemperatureTargetSet", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? TimeoutTemperatureTargetSet { get; set; }

        [ObservableProperty]
        
        [JsonProperty("triggerOkForM29", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? TriggerOkForM29 { get; set; }

        [ObservableProperty]
        
        [JsonProperty("useParityWorkaround", NullValueHandling = NullValueHandling.Ignore)]
        public partial string UseParityWorkaround { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("waitForStart", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? WaitForStart { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

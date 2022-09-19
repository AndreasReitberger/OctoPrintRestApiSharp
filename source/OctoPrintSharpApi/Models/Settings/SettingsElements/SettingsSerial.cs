using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsSerial
    {
        #region Properties
        [JsonProperty("abortHeatupOnCancel", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AbortHeatupOnCancel { get; set; }

        [JsonProperty("ackMax", NullValueHandling = NullValueHandling.Ignore)]
        public long? AckMax { get; set; }

        [JsonProperty("additionalBaudrates", NullValueHandling = NullValueHandling.Ignore)]
        public object[] AdditionalBaudrates { get; set; }

        [JsonProperty("additionalPorts", NullValueHandling = NullValueHandling.Ignore)]
        public object[] AdditionalPorts { get; set; }

        [JsonProperty("alwaysSendChecksum", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AlwaysSendChecksum { get; set; }

        [JsonProperty("autoconnect", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Autoconnect { get; set; }

        [JsonProperty("baudrate", NullValueHandling = NullValueHandling.Ignore)]
        public long? Baudrate { get; set; }

        [JsonProperty("baudrateOptions", NullValueHandling = NullValueHandling.Ignore)]
        public long[] BaudrateOptions { get; set; }

        [JsonProperty("blockWhileDwelling", NullValueHandling = NullValueHandling.Ignore)]
        public bool? BlockWhileDwelling { get; set; }

        [JsonProperty("blockedCommands", NullValueHandling = NullValueHandling.Ignore)]
        public string[] BlockedCommands { get; set; }

        [JsonProperty("capAutoreportSdStatus", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CapAutoreportSdStatus { get; set; }

        [JsonProperty("capAutoreportTemp", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CapAutoreportTemp { get; set; }

        [JsonProperty("capBusyProtocol", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CapBusyProtocol { get; set; }

        [JsonProperty("capEmergencyParser", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CapEmergencyParser { get; set; }

        [JsonProperty("checksumRequiringCommands", NullValueHandling = NullValueHandling.Ignore)]
        public string[] ChecksumRequiringCommands { get; set; }

        [JsonProperty("disconnectOnErrors", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DisconnectOnErrors { get; set; }

        [JsonProperty("emergencyCommands", NullValueHandling = NullValueHandling.Ignore)]
        public string[] EmergencyCommands { get; set; }

        [JsonProperty("exclusive", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Exclusive { get; set; }

        [JsonProperty("externalHeatupDetection", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ExternalHeatupDetection { get; set; }

        [JsonProperty("firmwareDetection", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FirmwareDetection { get; set; }

        [JsonProperty("helloCommand", NullValueHandling = NullValueHandling.Ignore)]
        public string HelloCommand { get; set; }

        [JsonProperty("ignoreErrorsFromFirmware", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IgnoreErrorsFromFirmware { get; set; }

        [JsonProperty("ignoreIdenticalResends", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IgnoreIdenticalResends { get; set; }

        [JsonProperty("log", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Log { get; set; }

        [JsonProperty("logPositionOnCancel", NullValueHandling = NullValueHandling.Ignore)]
        public bool? LogPositionOnCancel { get; set; }

        [JsonProperty("logPositionOnPause", NullValueHandling = NullValueHandling.Ignore)]
        public bool? LogPositionOnPause { get; set; }

        [JsonProperty("longRunningCommands", NullValueHandling = NullValueHandling.Ignore)]
        public string[] LongRunningCommands { get; set; }

        [JsonProperty("maxTimeoutsIdle", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxTimeoutsIdle { get; set; }

        [JsonProperty("maxTimeoutsLong", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxTimeoutsLong { get; set; }

        [JsonProperty("maxTimeoutsPrinting", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxTimeoutsPrinting { get; set; }

        [JsonProperty("neverSendChecksum", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NeverSendChecksum { get; set; }

        [JsonProperty("pausingCommands", NullValueHandling = NullValueHandling.Ignore)]
        public string[] PausingCommands { get; set; }

        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public string Port { get; set; }

        [JsonProperty("portOptions", NullValueHandling = NullValueHandling.Ignore)]
        public string[] PortOptions { get; set; }

        [JsonProperty("repetierTargetTemp", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RepetierTargetTemp { get; set; }

        [JsonProperty("sdAlwaysAvailable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SdAlwaysAvailable { get; set; }

        [JsonProperty("sdRelativePath", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SdRelativePath { get; set; }

        [JsonProperty("sendM112OnError", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SendM112OnError { get; set; }

        [JsonProperty("supportResendsWithoutOk", NullValueHandling = NullValueHandling.Ignore)]
        public string SupportResendsWithoutOk { get; set; }

        [JsonProperty("swallowOkAfterResend", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SwallowOkAfterResend { get; set; }

        [JsonProperty("timeoutBaudrateDetectionPause", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutBaudrateDetectionPause { get; set; }

        [JsonProperty("timeoutCommunication", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutCommunication { get; set; }

        [JsonProperty("timeoutCommunicationBusy", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutCommunicationBusy { get; set; }

        [JsonProperty("timeoutConnection", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutConnection { get; set; }

        [JsonProperty("timeoutDetection", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutDetection { get; set; }

        [JsonProperty("timeoutPositionLogWait", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutPositionLogWait { get; set; }

        [JsonProperty("timeoutSdStatus", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutSdStatus { get; set; }

        [JsonProperty("timeoutSdStatusAutoreport", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutSdStatusAutoreport { get; set; }

        [JsonProperty("timeoutTemperature", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutTemperature { get; set; }

        [JsonProperty("timeoutTemperatureAutoreport", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutTemperatureAutoreport { get; set; }

        [JsonProperty("timeoutTemperatureTargetSet", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeoutTemperatureTargetSet { get; set; }

        [JsonProperty("triggerOkForM29", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TriggerOkForM29 { get; set; }

        [JsonProperty("useParityWorkaround", NullValueHandling = NullValueHandling.Ignore)]
        public string UseParityWorkaround { get; set; }

        [JsonProperty("waitForStart", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WaitForStart { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

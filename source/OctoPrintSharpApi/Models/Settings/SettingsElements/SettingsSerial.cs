using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsSerial : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("abortHeatupOnCancel", NullValueHandling = NullValueHandling.Ignore)]
        bool? abortHeatupOnCancel;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ackMax", NullValueHandling = NullValueHandling.Ignore)]
        long? ackMax;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("additionalBaudrates", NullValueHandling = NullValueHandling.Ignore)]
        object[] additionalBaudrates = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("additionalPorts", NullValueHandling = NullValueHandling.Ignore)]
        object[] additionalPorts = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("alwaysSendChecksum", NullValueHandling = NullValueHandling.Ignore)]
        bool? alwaysSendChecksum;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("autoconnect", NullValueHandling = NullValueHandling.Ignore)]
        bool? autoconnect;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("baudrate", NullValueHandling = NullValueHandling.Ignore)]
        long? baudrate;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("baudrateOptions", NullValueHandling = NullValueHandling.Ignore)]
        long[] baudrateOptions = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("blockWhileDwelling", NullValueHandling = NullValueHandling.Ignore)]
        bool? blockWhileDwelling;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("blockedCommands", NullValueHandling = NullValueHandling.Ignore)]
        string[] blockedCommands = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("capAutoreportSdStatus", NullValueHandling = NullValueHandling.Ignore)]
        bool? capAutoreportSdStatus;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("capAutoreportTemp", NullValueHandling = NullValueHandling.Ignore)]
        bool? capAutoreportTemp;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("capBusyProtocol", NullValueHandling = NullValueHandling.Ignore)]
        bool? capBusyProtocol;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("capEmergencyParser", NullValueHandling = NullValueHandling.Ignore)]
        bool? capEmergencyParser;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("checksumRequiringCommands", NullValueHandling = NullValueHandling.Ignore)]
        string[] checksumRequiringCommands = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("disconnectOnErrors", NullValueHandling = NullValueHandling.Ignore)]
        bool? disconnectOnErrors;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("emergencyCommands", NullValueHandling = NullValueHandling.Ignore)]
        string[] emergencyCommands = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("exclusive", NullValueHandling = NullValueHandling.Ignore)]
        bool? exclusive;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("externalHeatupDetection", NullValueHandling = NullValueHandling.Ignore)]
        bool? externalHeatupDetection;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("firmwareDetection", NullValueHandling = NullValueHandling.Ignore)]
        bool? firmwareDetection;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("helloCommand", NullValueHandling = NullValueHandling.Ignore)]
        string helloCommand = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ignoreErrorsFromFirmware", NullValueHandling = NullValueHandling.Ignore)]
        bool? ignoreErrorsFromFirmware;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ignoreIdenticalResends", NullValueHandling = NullValueHandling.Ignore)]
        bool? ignoreIdenticalResends;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("log", NullValueHandling = NullValueHandling.Ignore)]
        bool? log;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("logPositionOnCancel", NullValueHandling = NullValueHandling.Ignore)]
        bool? logPositionOnCancel;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("logPositionOnPause", NullValueHandling = NullValueHandling.Ignore)]
        bool? logPositionOnPause;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("longRunningCommands", NullValueHandling = NullValueHandling.Ignore)]
        string[] longRunningCommands = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("maxTimeoutsIdle", NullValueHandling = NullValueHandling.Ignore)]
        long? maxTimeoutsIdle;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("maxTimeoutsLong", NullValueHandling = NullValueHandling.Ignore)]
        long? maxTimeoutsLong;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("maxTimeoutsPrinting", NullValueHandling = NullValueHandling.Ignore)]
        long? maxTimeoutsPrinting;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("neverSendChecksum", NullValueHandling = NullValueHandling.Ignore)]
        bool? neverSendChecksum;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pausingCommands", NullValueHandling = NullValueHandling.Ignore)]
        string[] pausingCommands = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        string port = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("portOptions", NullValueHandling = NullValueHandling.Ignore)]
        string[] portOptions = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("repetierTargetTemp", NullValueHandling = NullValueHandling.Ignore)]
        bool? repetierTargetTemp;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sdAlwaysAvailable", NullValueHandling = NullValueHandling.Ignore)]
        bool? sdAlwaysAvailable;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sdRelativePath", NullValueHandling = NullValueHandling.Ignore)]
        bool? sdRelativePath;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sendM112OnError", NullValueHandling = NullValueHandling.Ignore)]
        bool? sendM112OnError;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("supportResendsWithoutOk", NullValueHandling = NullValueHandling.Ignore)]
        string supportResendsWithoutOk = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("swallowOkAfterResend", NullValueHandling = NullValueHandling.Ignore)]
        bool? swallowOkAfterResend;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutBaudrateDetectionPause", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutBaudrateDetectionPause;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutCommunication", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutCommunication;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutCommunicationBusy", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutCommunicationBusy;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutConnection", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutConnection;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutDetection", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutDetection;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutPositionLogWait", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutPositionLogWait;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutSdStatus", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutSdStatus;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutSdStatusAutoreport", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutSdStatusAutoreport;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutTemperature", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutTemperature;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutTemperatureAutoreport", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutTemperatureAutoreport;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeoutTemperatureTargetSet", NullValueHandling = NullValueHandling.Ignore)]
        long? timeoutTemperatureTargetSet;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("triggerOkForM29", NullValueHandling = NullValueHandling.Ignore)]
        bool? triggerOkForM29;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("useParityWorkaround", NullValueHandling = NullValueHandling.Ignore)]
        string useParityWorkaround = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("waitForStart", NullValueHandling = NullValueHandling.Ignore)]
        bool? waitForStart;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

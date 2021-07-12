using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.Models.Settings
{
    public partial class OctoPrintSettings
    {
        [JsonProperty("api", NullValueHandling = NullValueHandling.Ignore)]
        public Api Api { get; set; }

        [JsonProperty("appearance", NullValueHandling = NullValueHandling.Ignore)]
        public Appearance Appearance { get; set; }

        [JsonProperty("feature", NullValueHandling = NullValueHandling.Ignore)]
        public Feature Feature { get; set; }

        [JsonProperty("folder", NullValueHandling = NullValueHandling.Ignore)]
        public Folder Folder { get; set; }

        [JsonProperty("gcodeAnalysis", NullValueHandling = NullValueHandling.Ignore)]
        public GcodeAnalysis GcodeAnalysis { get; set; }

        [JsonProperty("plugins", NullValueHandling = NullValueHandling.Ignore)]
        public Plugins Plugins { get; set; }

        [JsonProperty("printer", NullValueHandling = NullValueHandling.Ignore)]
        public Printer Printer { get; set; }

        [JsonProperty("scripts", NullValueHandling = NullValueHandling.Ignore)]
        public Scripts Scripts { get; set; }

        [JsonProperty("serial", NullValueHandling = NullValueHandling.Ignore)]
        public Serial Serial { get; set; }

        [JsonProperty("server", NullValueHandling = NullValueHandling.Ignore)]
        public Server Server { get; set; }

        [JsonProperty("system", NullValueHandling = NullValueHandling.Ignore)]
        public SystemClass System { get; set; }

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public Temperature Temperature { get; set; }

        [JsonProperty("terminalFilters", NullValueHandling = NullValueHandling.Ignore)]
        public TerminalFilter[] TerminalFilters { get; set; }

        [JsonProperty("webcam", NullValueHandling = NullValueHandling.Ignore)]
        public Webcam Webcam { get; set; }
    }

    public partial class Api
    {
        [JsonProperty("allowCrossOrigin", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowCrossOrigin { get; set; }

        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }
    }

    public partial class Appearance
    {
        [JsonProperty("closeModalsWithClick", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CloseModalsWithClick { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("colorIcon", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ColorIcon { get; set; }

        [JsonProperty("colorTransparent", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ColorTransparent { get; set; }

        [JsonProperty("defaultLanguage", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultLanguage { get; set; }

        [JsonProperty("fuzzyTimes", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FuzzyTimes { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("showFahrenheitAlso", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowFahrenheitAlso { get; set; }
    }

    public partial class Feature
    {
        [JsonProperty("autoUppercaseBlacklist", NullValueHandling = NullValueHandling.Ignore)]
        public string[] AutoUppercaseBlacklist { get; set; }

        [JsonProperty("g90InfluencesExtruder", NullValueHandling = NullValueHandling.Ignore)]
        public bool? G90InfluencesExtruder { get; set; }

        [JsonProperty("gcodeViewer", NullValueHandling = NullValueHandling.Ignore)]
        public bool? GcodeViewer { get; set; }

        [JsonProperty("keyboardControl", NullValueHandling = NullValueHandling.Ignore)]
        public bool? KeyboardControl { get; set; }

        [JsonProperty("mobileSizeThreshold", NullValueHandling = NullValueHandling.Ignore)]
        public long? MobileSizeThreshold { get; set; }

        [JsonProperty("modelSizeDetection", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ModelSizeDetection { get; set; }

        [JsonProperty("pollWatched", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PollWatched { get; set; }

        [JsonProperty("printCancelConfirmation", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PrintCancelConfirmation { get; set; }

        [JsonProperty("printStartConfirmation", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PrintStartConfirmation { get; set; }

        [JsonProperty("sdSupport", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SdSupport { get; set; }

        [JsonProperty("sizeThreshold", NullValueHandling = NullValueHandling.Ignore)]
        public long? SizeThreshold { get; set; }

        [JsonProperty("temperatureGraph", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TemperatureGraph { get; set; }
    }

    public partial class Folder
    {
        [JsonProperty("logs", NullValueHandling = NullValueHandling.Ignore)]
        public string Logs { get; set; }

        [JsonProperty("timelapse", NullValueHandling = NullValueHandling.Ignore)]
        public string Timelapse { get; set; }

        [JsonProperty("timelapseTmp", NullValueHandling = NullValueHandling.Ignore)]
        public string TimelapseTmp { get; set; }

        [JsonProperty("uploads", NullValueHandling = NullValueHandling.Ignore)]
        public string Uploads { get; set; }

        [JsonProperty("watched", NullValueHandling = NullValueHandling.Ignore)]
        public string Watched { get; set; }
    }

    public partial class GcodeAnalysis
    {
        [JsonProperty("runAt", NullValueHandling = NullValueHandling.Ignore)]
        public string RunAt { get; set; }
    }

    public partial class Plugins
    {
        [JsonProperty("action_command_prompt", NullValueHandling = NullValueHandling.Ignore)]
        public ActionCommandPrompt ActionCommandPrompt { get; set; }

        [JsonProperty("announcements", NullValueHandling = NullValueHandling.Ignore)]
        public Announcements Announcements { get; set; }

        [JsonProperty("discovery", NullValueHandling = NullValueHandling.Ignore)]
        public Discovery Discovery { get; set; }

        [JsonProperty("errortracking", NullValueHandling = NullValueHandling.Ignore)]
        public Errortracking Errortracking { get; set; }

        [JsonProperty("pi_support", NullValueHandling = NullValueHandling.Ignore)]
        public PiSupport PiSupport { get; set; }

        [JsonProperty("pluginmanager", NullValueHandling = NullValueHandling.Ignore)]
        public Pluginmanager Pluginmanager { get; set; }

        [JsonProperty("slic3r", NullValueHandling = NullValueHandling.Ignore)]
        public Slic3R Slic3R { get; set; }

        [JsonProperty("softwareupdate", NullValueHandling = NullValueHandling.Ignore)]
        public Softwareupdate Softwareupdate { get; set; }

        [JsonProperty("tracking", NullValueHandling = NullValueHandling.Ignore)]
        public Tracking Tracking { get; set; }
    }

    public partial class ActionCommandPrompt
    {
        [JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)]
        public string Command { get; set; }

        [JsonProperty("enable", NullValueHandling = NullValueHandling.Ignore)]
        public string Enable { get; set; }

        [JsonProperty("enable_emergency_sending", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnableEmergencySending { get; set; }

        [JsonProperty("enable_signal_support", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnableSignalSupport { get; set; }
    }

    public partial class Announcements
    {
        [JsonProperty("channel_order", NullValueHandling = NullValueHandling.Ignore)]
        public string[] ChannelOrder { get; set; }

        [JsonProperty("channels", NullValueHandling = NullValueHandling.Ignore)]
        public Channels Channels { get; set; }

        [JsonProperty("display_limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? DisplayLimit { get; set; }

        [JsonProperty("enabled_channels", NullValueHandling = NullValueHandling.Ignore)]
        public string[] EnabledChannels { get; set; }

        [JsonProperty("forced_channels", NullValueHandling = NullValueHandling.Ignore)]
        public string[] ForcedChannels { get; set; }

        [JsonProperty("summary_limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? SummaryLimit { get; set; }

        [JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? Ttl { get; set; }
    }

    public partial class Channels
    {
        [JsonProperty("_blog", NullValueHandling = NullValueHandling.Ignore)]
        public Blog Blog { get; set; }

        [JsonProperty("_important", NullValueHandling = NullValueHandling.Ignore)]
        public Blog Important { get; set; }

        [JsonProperty("_octopi", NullValueHandling = NullValueHandling.Ignore)]
        public Blog Octopi { get; set; }

        [JsonProperty("_plugins", NullValueHandling = NullValueHandling.Ignore)]
        public Blog Plugins { get; set; }

        [JsonProperty("_releases", NullValueHandling = NullValueHandling.Ignore)]
        public Blog Releases { get; set; }
    }

    public partial class Blog
    {
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
        public long? Priority { get; set; }

        [JsonProperty("read_until", NullValueHandling = NullValueHandling.Ignore)]
        public long? ReadUntil { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
    }

    public partial class Discovery
    {
        [JsonProperty("httpPassword")]
        public object HttpPassword { get; set; }

        [JsonProperty("httpUsername")]
        public object HttpUsername { get; set; }

        [JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        public Model Model { get; set; }

        [JsonProperty("pathPrefix")]
        public object PathPrefix { get; set; }

        [JsonProperty("publicHost")]
        public object PublicHost { get; set; }

        [JsonProperty("publicPort", NullValueHandling = NullValueHandling.Ignore)]
        public long? PublicPort { get; set; }

        [JsonProperty("upnpUuid", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? UpnpUuid { get; set; }

        [JsonProperty("zeroConf", NullValueHandling = NullValueHandling.Ignore)]
        public object[] ZeroConf { get; set; }
    }

    public partial class Model
    {
        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("name")]
        public object Name { get; set; }

        [JsonProperty("number")]
        public object Number { get; set; }

        [JsonProperty("serial")]
        public object Serial { get; set; }

        [JsonProperty("url")]
        public object Url { get; set; }

        [JsonProperty("vendor")]
        public object Vendor { get; set; }

        [JsonProperty("vendorUrl")]
        public object VendorUrl { get; set; }
    }

    public partial class Errortracking
    {
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }

        [JsonProperty("enabled_unreleased", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnabledUnreleased { get; set; }

        [JsonProperty("unique_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? UniqueId { get; set; }

        [JsonProperty("url_coreui", NullValueHandling = NullValueHandling.Ignore)]
        public Uri UrlCoreui { get; set; }

        [JsonProperty("url_server", NullValueHandling = NullValueHandling.Ignore)]
        public Uri UrlServer { get; set; }
    }

    public partial class PiSupport
    {
        [JsonProperty("vcgencmd_throttle_check_command", NullValueHandling = NullValueHandling.Ignore)]
        public string VcgencmdThrottleCheckCommand { get; set; }

        [JsonProperty("vcgencmd_throttle_check_enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? VcgencmdThrottleCheckEnabled { get; set; }
    }

    public partial class Pluginmanager
    {
        [JsonProperty("confirm_disable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ConfirmDisable { get; set; }

        [JsonProperty("dependency_links", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DependencyLinks { get; set; }

        [JsonProperty("hidden", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Hidden { get; set; }

        [JsonProperty("ignore_throttled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IgnoreThrottled { get; set; }

        [JsonProperty("notices", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Notices { get; set; }

        [JsonProperty("notices_ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? NoticesTtl { get; set; }

        [JsonProperty("pip_args")]
        public object PipArgs { get; set; }

        [JsonProperty("pip_force_user", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PipForceUser { get; set; }

        [JsonProperty("repository", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Repository { get; set; }

        [JsonProperty("repository_ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? RepositoryTtl { get; set; }
    }

    public partial class Slic3R
    {
        [JsonProperty("debug_logging", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DebugLogging { get; set; }

        [JsonProperty("default_profile")]
        public object DefaultProfile { get; set; }

        [JsonProperty("slic3r_engine")]
        public object Slic3REngine { get; set; }
    }

    public partial class Softwareupdate
    {
        [JsonProperty("cache_ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? CacheTtl { get; set; }

        [JsonProperty("ignore_throttled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IgnoreThrottled { get; set; }

        [JsonProperty("notify_users", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NotifyUsers { get; set; }

        [JsonProperty("octoprint_branch_mappings", NullValueHandling = NullValueHandling.Ignore)]
        public OctoprintBranchMapping[] OctoprintBranchMappings { get; set; }

        [JsonProperty("octoprint_checkout_folder")]
        public object OctoprintCheckoutFolder { get; set; }

        [JsonProperty("octoprint_method", NullValueHandling = NullValueHandling.Ignore)]
        public string OctoprintMethod { get; set; }

        [JsonProperty("octoprint_pip_target", NullValueHandling = NullValueHandling.Ignore)]
        public string OctoprintPipTarget { get; set; }

        [JsonProperty("octoprint_release_channel", NullValueHandling = NullValueHandling.Ignore)]
        public string OctoprintReleaseChannel { get; set; }

        [JsonProperty("octoprint_tracked_branch")]
        public object OctoprintTrackedBranch { get; set; }

        [JsonProperty("octoprint_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OctoprintType { get; set; }

        [JsonProperty("pip_command")]
        public object PipCommand { get; set; }

        [JsonProperty("pip_enable_check", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PipEnableCheck { get; set; }
    }

    public partial class OctoprintBranchMapping
    {
        [JsonProperty("branch", NullValueHandling = NullValueHandling.Ignore)]
        public string Branch { get; set; }

        [JsonProperty("commitish", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Commitish { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public partial class Tracking
    {
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }

        [JsonProperty("events", NullValueHandling = NullValueHandling.Ignore)]
        public Events Events { get; set; }

        [JsonProperty("ping")]
        public object Ping { get; set; }

        [JsonProperty("pong", NullValueHandling = NullValueHandling.Ignore)]
        public long? Pong { get; set; }

        [JsonProperty("server")]
        public object Server { get; set; }

        [JsonProperty("unique_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? UniqueId { get; set; }
    }

    public partial class Events
    {
        [JsonProperty("commerror", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Commerror { get; set; }

        [JsonProperty("plugin", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Plugin { get; set; }

        [JsonProperty("pong", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Pong { get; set; }

        [JsonProperty("printer", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Printer { get; set; }

        [JsonProperty("printer_safety_check", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PrinterSafetyCheck { get; set; }

        [JsonProperty("printjob", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Printjob { get; set; }

        [JsonProperty("slicing", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Slicing { get; set; }

        [JsonProperty("startup", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Startup { get; set; }

        [JsonProperty("throttled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Throttled { get; set; }

        [JsonProperty("update", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Update { get; set; }
    }

    public partial class Printer
    {
        [JsonProperty("defaultExtrusionLength", NullValueHandling = NullValueHandling.Ignore)]
        public long? DefaultExtrusionLength { get; set; }
    }

    public partial class Scripts
    {
        [JsonProperty("gcode", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Gcode { get; set; }
    }

    public partial class Gcode
    {
        [JsonProperty("afterPrintCancelled", NullValueHandling = NullValueHandling.Ignore)]
        public string AfterPrintCancelled { get; set; }

        [JsonProperty("snippets/disable_bed", NullValueHandling = NullValueHandling.Ignore)]
        public string SnippetsDisableBed { get; set; }

        [JsonProperty("snippets/disable_hotends", NullValueHandling = NullValueHandling.Ignore)]
        public string SnippetsDisableHotends { get; set; }
    }

    public partial class Serial
    {
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
    }

    public partial class Server
    {
        [JsonProperty("allowFraming", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowFraming { get; set; }

        [JsonProperty("commands", NullValueHandling = NullValueHandling.Ignore)]
        public Commands Commands { get; set; }

        [JsonProperty("diskspace", NullValueHandling = NullValueHandling.Ignore)]
        public Diskspace Diskspace { get; set; }

        [JsonProperty("onlineCheck", NullValueHandling = NullValueHandling.Ignore)]
        public OnlineCheck OnlineCheck { get; set; }

        [JsonProperty("pluginBlacklist", NullValueHandling = NullValueHandling.Ignore)]
        public PluginBlacklist PluginBlacklist { get; set; }
    }

    public partial class Commands
    {
        [JsonProperty("serverRestartCommand", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerRestartCommand { get; set; }

        [JsonProperty("systemRestartCommand", NullValueHandling = NullValueHandling.Ignore)]
        public string SystemRestartCommand { get; set; }

        [JsonProperty("systemShutdownCommand", NullValueHandling = NullValueHandling.Ignore)]
        public string SystemShutdownCommand { get; set; }
    }

    public partial class Diskspace
    {
        [JsonProperty("critical", NullValueHandling = NullValueHandling.Ignore)]
        public long? Critical { get; set; }

        [JsonProperty("warning", NullValueHandling = NullValueHandling.Ignore)]
        public long? Warning { get; set; }
    }

    public partial class OnlineCheck
    {
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }

        [JsonProperty("host", NullValueHandling = NullValueHandling.Ignore)]
        public string Host { get; set; }

        [JsonProperty("interval", NullValueHandling = NullValueHandling.Ignore)]
        public long? Interval { get; set; }

        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public long? Port { get; set; }
    }

    public partial class PluginBlacklist
    {
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }

        [JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? Ttl { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
    }

    public partial class SystemClass
    {
        [JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Actions { get; set; }

        [JsonProperty("events")]
        public object Events { get; set; }
    }

    public partial class Temperature
    {
        [JsonProperty("cutoff", NullValueHandling = NullValueHandling.Ignore)]
        public long? Cutoff { get; set; }

        [JsonProperty("profiles", NullValueHandling = NullValueHandling.Ignore)]
        public Profile[] Profiles { get; set; }

        [JsonProperty("sendAutomatically", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SendAutomatically { get; set; }

        [JsonProperty("sendAutomaticallyAfter", NullValueHandling = NullValueHandling.Ignore)]
        public long? SendAutomaticallyAfter { get; set; }
    }

    public partial class Profile
    {
        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public long? Bed { get; set; }

        [JsonProperty("chamber")]
        public object Chamber { get; set; }

        [JsonProperty("extruder", NullValueHandling = NullValueHandling.Ignore)]
        public long? Extruder { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public partial class TerminalFilter
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("regex", NullValueHandling = NullValueHandling.Ignore)]
        public string Regex { get; set; }
    }

    public partial class Webcam
    {
        [JsonProperty("bitrate", NullValueHandling = NullValueHandling.Ignore)]
        public string Bitrate { get; set; }

        [JsonProperty("ffmpegPath", NullValueHandling = NullValueHandling.Ignore)]
        public string FfmpegPath { get; set; }

        [JsonProperty("ffmpegThreads", NullValueHandling = NullValueHandling.Ignore)]
        public long? FfmpegThreads { get; set; }

        [JsonProperty("ffmpegVideoCodec", NullValueHandling = NullValueHandling.Ignore)]
        public string FfmpegVideoCodec { get; set; }

        [JsonProperty("flipH", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FlipH { get; set; }

        [JsonProperty("flipV", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FlipV { get; set; }

        [JsonProperty("rotate90", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Rotate90 { get; set; }

        [JsonProperty("snapshotSslValidation", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SnapshotSslValidation { get; set; }

        [JsonProperty("snapshotTimeout", NullValueHandling = NullValueHandling.Ignore)]
        public long? SnapshotTimeout { get; set; }

        [JsonProperty("snapshotUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri SnapshotUrl { get; set; }

        [JsonProperty("streamRatio", NullValueHandling = NullValueHandling.Ignore)]
        public string StreamRatio { get; set; }

        [JsonProperty("streamTimeout", NullValueHandling = NullValueHandling.Ignore)]
        public long? StreamTimeout { get; set; }

        [JsonProperty("streamUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string StreamUrl { get; set; }

        [JsonProperty("timelapseEnabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TimelapseEnabled { get; set; }

        [JsonProperty("watermark", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Watermark { get; set; }

        [JsonProperty("webcamEnabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WebcamEnabled { get; set; }
    }
}

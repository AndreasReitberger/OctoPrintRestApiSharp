using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("bitrate", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Bitrate { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("ffmpegPath", NullValueHandling = NullValueHandling.Ignore)]
        public partial string FfmpegPath { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("ffmpegThreads", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? FfmpegThreads { get; set; }

        [ObservableProperty]

        [JsonProperty("ffmpegVideoCodec", NullValueHandling = NullValueHandling.Ignore)]
        public partial string FfmpegVideoCodec { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("flipH", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? FlipH { get; set; }

        [ObservableProperty]

        [JsonProperty("flipV", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? FlipV { get; set; }

        [ObservableProperty]

        [JsonProperty("rotate90", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Rotate90 { get; set; }

        [ObservableProperty]

        [JsonProperty("snapshotSslValidation", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? SnapshotSslValidation { get; set; }

        [ObservableProperty]

        [JsonProperty("snapshotTimeout", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? SnapshotTimeout { get; set; }

        [ObservableProperty]

        [JsonProperty("snapshotUrl", NullValueHandling = NullValueHandling.Ignore)]
        public partial Uri? SnapshotUrl { get; set; }

        [ObservableProperty]

        [JsonProperty("streamRatio", NullValueHandling = NullValueHandling.Ignore)]
        public partial string StreamRatio { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("streamTimeout", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? StreamTimeout { get; set; }

        [ObservableProperty]

        [JsonProperty("streamUrl", NullValueHandling = NullValueHandling.Ignore)]
        public partial string StreamUrl { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("timelapseEnabled", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? TimelapseEnabled { get; set; }

        [ObservableProperty]

        [JsonProperty("watermark", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Watermark { get; set; }

        [ObservableProperty]

        [JsonProperty("webcamEnabled", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? WebcamEnabled { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

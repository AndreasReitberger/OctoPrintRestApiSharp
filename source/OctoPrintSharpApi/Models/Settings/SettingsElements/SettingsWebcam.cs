using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public partial class SettingsWebcam
    {
        #region Properties
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
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

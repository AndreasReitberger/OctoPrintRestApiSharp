using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("bitrate", NullValueHandling = NullValueHandling.Ignore)]
        string bitrate = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ffmpegPath", NullValueHandling = NullValueHandling.Ignore)]
        string ffmpegPath = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ffmpegThreads", NullValueHandling = NullValueHandling.Ignore)]
        long? ffmpegThreads;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ffmpegVideoCodec", NullValueHandling = NullValueHandling.Ignore)]
        string ffmpegVideoCodec = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flipH", NullValueHandling = NullValueHandling.Ignore)]
        bool? flipH;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flipV", NullValueHandling = NullValueHandling.Ignore)]
        bool? flipV;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rotate90", NullValueHandling = NullValueHandling.Ignore)]
        bool? rotate90;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("snapshotSslValidation", NullValueHandling = NullValueHandling.Ignore)]
        bool? snapshotSslValidation;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("snapshotTimeout", NullValueHandling = NullValueHandling.Ignore)]
        long? snapshotTimeout;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("snapshotUrl", NullValueHandling = NullValueHandling.Ignore)]
        Uri? snapshotUrl;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("streamRatio", NullValueHandling = NullValueHandling.Ignore)]
        string streamRatio = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("streamTimeout", NullValueHandling = NullValueHandling.Ignore)]
        long? streamTimeout;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("streamUrl", NullValueHandling = NullValueHandling.Ignore)]
        string streamUrl = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timelapseEnabled", NullValueHandling = NullValueHandling.Ignore)]
        bool? timelapseEnabled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("watermark", NullValueHandling = NullValueHandling.Ignore)]
        bool? watermark;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("webcamEnabled", NullValueHandling = NullValueHandling.Ignore)]
        bool? webcamEnabled;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

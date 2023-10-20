using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFile : ObservableObject, IGcode
    {
        #region Properties

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [ObservableProperty]
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        List<OctoPrintFile> children = new();

        [ObservableProperty]
        [JsonProperty("date")]
        long date;

        [ObservableProperty]
        [JsonProperty("display", NullValueHandling = NullValueHandling.Ignore)]
        string display;

        [ObservableProperty]
        [JsonProperty("gcodeAnalysis", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintFileGcodeAnalysis gcodeAnalysis;

        [ObservableProperty]
        [JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
        string hash;

        [ObservableProperty]
        [JsonProperty("name")]
        string fileName;

        [ObservableProperty]
        [JsonProperty("origin")]
        string origin;

        [ObservableProperty]
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        string filePath;

        [ObservableProperty]
        [JsonProperty("prints")]
        OctoPrintFilePrints prints;

        [ObservableProperty]
        [JsonProperty("refs", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintFileChildRefs refs;

        [ObservableProperty]
        [JsonProperty("size")]
        long size;

        [ObservableProperty]
        [JsonProperty("statistics")]
        OctoPrintFileStatistics statistics;

        [ObservableProperty]
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        string type;

        [ObservableProperty]
        [JsonProperty("typePath", NullValueHandling = NullValueHandling.Ignore)]
        List<string> typePath = new();

        #endregion

        #region Interface, unused

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double modified;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double volume;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double filament;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string permissions;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string group;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        IGcodeMeta meta;
        #endregion

        #region JsonIgnore

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        long identifier;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string printerName;

        [ObservableProperty]
        [JsonIgnore]
        bool isVisible;

        [ObservableProperty]
        [JsonIgnore]
        bool isLoadingImage = false;

        [ObservableProperty]
        [JsonIgnore]
        byte[] image = Array.Empty<byte>();

        [ObservableProperty]
        [JsonIgnore]
        byte[] thumbnail = Array.Empty<byte>();

        [ObservableProperty]
        [JsonIgnore]
        GcodeImageType imageType = GcodeImageType.Thumbnail;

        [JsonIgnore]
        public bool IsAnalysed
        {
            get => GcodeAnalysis != null;
        }
        [JsonIgnore]
        public bool Printed
        {
            get => Statistics != null;
        }
        #endregion

        #region Methods
        public Task MoveToAsync(IPrint3dServerClient client, string targetPath, bool copy = false)
        {
            throw new NotImplementedException();
        }

        public Task MoveToQueueAsync(IPrint3dServerClient client, bool printIfReady = false)
        {
            throw new NotImplementedException();
        }

        public Task PrintAsync(IPrint3dServerClient client)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

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
                // Nothing to do here
            }
        }
        #endregion

        #region Clone

        public object Clone() => MemberwiseClone();

        #endregion

    }
}

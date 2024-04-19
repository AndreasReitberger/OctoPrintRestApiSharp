using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFile : ObservableObject, IGcode
    {
        #region Properties

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        GcodeTimeBaseTarget timeBaseTarget = GcodeTimeBaseTarget.LongSeconds;

        [ObservableProperty]
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonIgnore]
        List<IGcode> children = new();

        [ObservableProperty]
        [JsonProperty("date")]
        [property: JsonIgnore]
        long date;

        [ObservableProperty]
        [JsonProperty("display", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonIgnore]
        string display;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(PrintTime))]
        [NotifyPropertyChangedFor(nameof(Volume))]
        [NotifyPropertyChangedFor(nameof(Filament))]
        [property: JsonProperty("gcodeAnalysis", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintFileGcodeAnalysis gcodeAnalysis;
        partial void OnGcodeAnalysisChanged(OctoPrintFileGcodeAnalysis value)
        {
            if (value is not null)
            {
                PrintTime = value.EstimatedPrintTime;
                Volume = value.TotalFilamentVolume;
                Filament = value.TotalFilamentLength;
            }
        }

        [ObservableProperty]
        [JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonIgnore]
        string hash;

        [ObservableProperty]
        [JsonProperty("name")]
        [property: JsonIgnore]
        string fileName;

        [ObservableProperty]
        [JsonProperty("origin")]
        [property: JsonIgnore]
        string origin;

        [ObservableProperty]
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonIgnore]
        string filePath;

        [ObservableProperty]
        [JsonProperty("prints")]
        [property: JsonIgnore]
        OctoPrintFilePrints prints;

        [ObservableProperty]
        [JsonProperty("refs", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonIgnore]
        OctoPrintFileChildRefs refs;

        [ObservableProperty]
        [JsonProperty("size")]
        [property: JsonIgnore]
        long size;

        [ObservableProperty]
        [JsonProperty("statistics")]
        [property: JsonIgnore]
        OctoPrintFileStatistics statistics;

        [ObservableProperty]
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonIgnore]
        string type;

        [ObservableProperty]
        [JsonProperty("typePath", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonIgnore]
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
        double printTime;
        partial void OnPrintTimeChanged(double value)
        {
            PrintTimeGeneralized = TimeBaseConvertHelper.FromLongSeconds(value);
        }

        [ObservableProperty]
        TimeSpan? printTimeGeneralized;

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

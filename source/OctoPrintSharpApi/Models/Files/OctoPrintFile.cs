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
        Guid id;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        GcodeTimeBaseTarget timeBaseTarget = GcodeTimeBaseTarget.LongSeconds;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        List<IGcode> children = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("date")]
        long date;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("display", NullValueHandling = NullValueHandling.Ignore)]
        string display = string.Empty;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(PrintTime))]
        [NotifyPropertyChangedFor(nameof(Volume))]
        [NotifyPropertyChangedFor(nameof(Filament))]
        [property: JsonProperty("gcodeAnalysis", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintFileGcodeAnalysis? gcodeAnalysis;
        partial void OnGcodeAnalysisChanged(OctoPrintFileGcodeAnalysis? value)
        {
            if (value is not null)
            {
                PrintTime = value.EstimatedPrintTime;
                Volume = value.TotalFilamentVolume;
                Filament = value.TotalFilamentLength;
            }
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
        string hash = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name")]
        string fileName = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("origin")]
        string origin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        string filePath = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("prints")]
        OctoPrintFilePrints? prints;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("refs", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintFileChildRefs? refs;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("size")]
        long size;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("statistics")]
        OctoPrintFileStatistics? statistics;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        string type = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("typePath", NullValueHandling = NullValueHandling.Ignore)]
        List<string> typePath = [];

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

        [ObservableProperty, JsonIgnore]
        TimeSpan? printTimeGeneralized;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string permissions = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string group = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        IGcodeMeta? meta;
        #endregion

        #region JsonIgnore

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        long identifier;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string printerName = string.Empty;

        [ObservableProperty, JsonIgnore]
        bool isVisible;

        [ObservableProperty, JsonIgnore]
        bool isLoadingImage = false;

        [ObservableProperty, JsonIgnore]
        byte[] image = [];

        [ObservableProperty, JsonIgnore]
        byte[] thumbnail = [];

        [ObservableProperty, JsonIgnore]
        GcodeImageType imageType = GcodeImageType.Thumbnail;

        [JsonIgnore]
        public bool IsAnalysed => GcodeAnalysis is not null;
        
        [JsonIgnore]
        public bool Printed => Statistics is not null;
        
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

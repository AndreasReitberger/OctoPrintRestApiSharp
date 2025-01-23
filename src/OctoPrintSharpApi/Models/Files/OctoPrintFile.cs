using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFile : ObservableObject, IGcode
    {
        #region Properties

        [ObservableProperty]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public partial GcodeTimeBaseTarget TimeBaseTarget { get; set; } = GcodeTimeBaseTarget.LongSeconds;

        [ObservableProperty]
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<IGcode> Children { get; set; } = [];

        [ObservableProperty]
        [JsonProperty("date")]
        public partial long Date { get; set; }

        [ObservableProperty]
        [JsonProperty("display", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Display { get; set; } = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PrintTime))]
        [NotifyPropertyChangedFor(nameof(Volume))]
        [NotifyPropertyChangedFor(nameof(Filament))]
        [JsonProperty("gcodeAnalysis", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintFileGcodeAnalysis? GcodeAnalysis { get; set; }

        partial void OnGcodeAnalysisChanged(OctoPrintFileGcodeAnalysis? value)
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
        public partial string Hash { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("name")]
        public partial string FileName { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("origin")]
        public partial string Origin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public partial string FilePath { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("prints")]
        public partial OctoPrintFilePrints? Prints { get; set; }

        [ObservableProperty]
        [JsonProperty("refs", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintFileChildRefs? Refs { get; set; }

        [ObservableProperty]
        [JsonProperty("size")]
        public partial long Size { get; set; }

        [ObservableProperty]
        [JsonProperty("statistics")]
        public partial OctoPrintFileStatistics? Statistics { get; set; }

        [ObservableProperty]
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("typePath", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<string> TypePath { get; set; } = [];

        #endregion

        #region Interface, unused

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CreatedGeneralized))]
        public partial double? Created { get; set; } = 0;
        partial void OnCreatedChanged(double? value)
        {
            if (value is not null)
                CreatedGeneralized = TimeBaseConvertHelper.FromUnixDoubleMiliseconds(value);
        }

        [ObservableProperty]
        public partial DateTime? CreatedGeneralized { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public partial double? Modified { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public partial double Volume { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public partial double Filament { get; set; }

        [ObservableProperty]
        public partial double PrintTime { get; set; }

        partial void OnPrintTimeChanged(double value)
        {
            PrintTimeGeneralized = TimeBaseConvertHelper.FromLongSeconds(value);
        }

        [ObservableProperty]
        public partial TimeSpan? PrintTimeGeneralized { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public partial string Permissions { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonIgnore]
        public partial string Group { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonIgnore]
        public partial IGcodeMeta? Meta { get; set; }
        #endregion

        #region JsonIgnore

        [ObservableProperty]
        [JsonIgnore]
        public partial long Identifier { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public partial string PrinterName { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonIgnore]
        public partial bool IsVisible { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public partial bool IsLoadingImage { get; set; } = false;

        [ObservableProperty]
        [JsonIgnore]
        public partial byte[]? Image { get; set; } = [];

        [ObservableProperty]
        [JsonIgnore]
        public partial byte[]? Thumbnail { get; set; } = [];

        [ObservableProperty]
        [JsonIgnore]
        public partial GcodeImageType ImageType { get; set; } = GcodeImageType.Thumbnail;

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

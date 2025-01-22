using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinter : ObservableObject, IPrinter3d
    {
        #region Properties


        [ObservableProperty]
        [JsonIgnore]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        [JsonProperty("axes")]
        public partial OctoPrintPrinterAxes? Axes { get; set; }

        [ObservableProperty]
        [JsonProperty("color")]
        public partial string Color { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("current")]
        public partial bool Current { get; set; }

        [ObservableProperty]
        [JsonProperty("default")]
        public partial bool DefaultDefault { get; set; }

        [ObservableProperty]
        [JsonProperty("extruder")]
        public partial OctoPrintPrinterExtruder? Extruder { get; set; }

        [ObservableProperty]
        [JsonProperty("heatedBed")]
        public partial bool HasHeatedBed { get; set; }

        [ObservableProperty]
        [JsonProperty("heatedChamber")]
        public partial bool HasHeatedChamber { get; set; }

        [ObservableProperty]
        [JsonProperty("id")]
        public partial string Slug { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("model")]
        public partial string Model { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonProperty("resource")]
        public partial Uri? Resource { get; set; }

        [ObservableProperty]
        [JsonProperty("volume")]
        public partial OctoPrintPrinterVolume? Volume { get; set; }
        #endregion

        #region Interface, unused

        [ObservableProperty]
        public partial long? LineSent { get; set; }

        [ObservableProperty]
        public partial long? Layers { get; set; }

        [ObservableProperty]
        public partial long? PauseState { get; set; }

        [ObservableProperty]
        public partial long? Start { get; set; }

        [ObservableProperty]
        public partial long? TotalLines { get; set; }

        [ObservableProperty]
        public partial int? Repeat { get; set; }

        #endregion

        #region JsonIgnored

        [ObservableProperty]

        public partial bool IsActive { get; set; } = true;
        [ObservableProperty]
        [JsonIgnore]
        public partial bool IsOnline { get; set; } = true;

        [ObservableProperty]
        [JsonIgnore]
        public partial double Progress { get; set; } = 0;

        [ObservableProperty]
        [JsonIgnore]
        public partial string ActiveJobName { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonIgnore]
        public partial string ActiveJobId { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonIgnore]
        public partial string ActiveJobState { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonIgnore]
        public partial bool IsPrinting { get; set; } = false;

        [ObservableProperty]
        [JsonIgnore]
        public partial bool IsPaused { get; set; } = false;

        [ObservableProperty]
        [JsonIgnore]
        public partial bool IsSelected { get; set; } = false;

        [ObservableProperty]
        [JsonIgnore]
        public partial double? Extruder1Temperature { get; set; } = 0;

        [ObservableProperty]
        [JsonIgnore]
        public partial double? Extruder2Temperature { get; set; } = 0;

        [ObservableProperty]
        [JsonIgnore]
        public partial double? Extruder3Temperature { get; set; } = 0;

        [ObservableProperty]
        [JsonIgnore]
        public partial double? Extruder4Temperature { get; set; } = 0;

        [ObservableProperty]
        [JsonIgnore]
        public partial double? Extruder5Temperature { get; set; } = 0;

        [ObservableProperty]
        [JsonIgnore]
        public partial double? HeatedBedTemperature { get; set; } = 0;

        [ObservableProperty]
        [JsonIgnore]
        public partial double? HeatedChamberTemperature { get; set; } = 0;

        [ObservableProperty]
        [JsonIgnore]
        public partial double? PrintProgress { get; set; } = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(RemainingPrintDurationGeneralized))]
        [JsonIgnore]
        public partial double? RemainingPrintDuration { get; set; } = 0;
        partial void OnRemainingPrintDurationChanged(double? value)
        {
            if (value is not null)
                RemainingPrintDurationGeneralized = TimeBaseConvertHelper.FromDoubleSeconds(value);
        }

        [ObservableProperty]
        public partial TimeSpan? RemainingPrintDurationGeneralized { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PrintStartedGeneralized))]
        [JsonIgnore]
        public partial double? PrintStarted { get; set; } = 0;
        partial void OnPrintStartedChanged(double? value)
        {
            if (value is not null)
                PrintStartedGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty]
        public partial DateTime? PrintStartedGeneralized { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PrintDurationGeneralized))]
        [JsonIgnore]
        public partial double? PrintDuration { get; set; } = 0;
        partial void OnPrintDurationChanged(double? value)
        {
            if (value is not null)
                PrintDurationGeneralized = TimeBaseConvertHelper.FromDoubleHours(value);
            if (value > 0)
                RemainingPrintDuration = value * PrintProgress;
        }

        [ObservableProperty]
        public partial TimeSpan? PrintDurationGeneralized { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PrintDurationEstimatedGeneralized))]
        [JsonIgnore]
        public partial double? PrintDurationEstimated { get; set; } = 0;
        partial void OnPrintDurationEstimatedChanged(double? value)
        {
            if (value is not null)
                PrintDurationEstimatedGeneralized = TimeBaseConvertHelper.FromDoubleHours(value);
        }

        [ObservableProperty]
        public partial TimeSpan? PrintDurationEstimatedGeneralized { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public partial byte[] CurrentPrintImage { get; set; } = [];

        #endregion

        #region Methods

        public Task<bool> HomeAsync(IPrint3dServerClient client, bool x, bool y, bool z) => client.HomeAsync(x, y, z);

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        public override bool Equals(object? obj)
        {
            if (obj is not OctoPrintPrinter item)
                return false;
            return Slug.Equals(item.Slug);
        }

        public override int GetHashCode()
        {
            return Slug.GetHashCode();
        }

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
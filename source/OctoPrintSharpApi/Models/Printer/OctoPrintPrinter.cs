using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinter : ObservableObject, IPrinter3d
    {
        #region Properties


        [ObservableProperty]
        [property: JsonIgnore]
        Guid id;

        [ObservableProperty]
        [JsonProperty("axes")]
        OctoPrintPrinterAxes axes;

        [ObservableProperty]
        [JsonProperty("color")]
        string color;

        [ObservableProperty]
        [JsonProperty("current")]
        bool current;

        [ObservableProperty]
        [JsonProperty("default")]
        bool defaultDefault;

        [ObservableProperty]
        [JsonProperty("extruder")]
        OctoPrintPrinterExtruder extruder;

        [ObservableProperty]
        [JsonProperty("heatedBed")]
        bool hasHeatedBed;

        [ObservableProperty]
        [JsonProperty("heatedChamber")]
        bool hasHeatedChamber;

        [ObservableProperty]
        [JsonProperty("id")]
        string slug;

        [ObservableProperty]
        [JsonProperty("model")]
        string model;

        [ObservableProperty]
        [JsonProperty("name")]
        string name;

        [ObservableProperty]
        [JsonProperty("resource")]
        Uri resource;

        [ObservableProperty]
        [JsonProperty("volume")]
        OctoPrintPrinterVolume volume;
        #endregion

        #region Interface, unused

        [ObservableProperty]
        [JsonIgnore]
        long? lineSent;

        [ObservableProperty]
        [JsonIgnore]
        long? layers;

        [ObservableProperty]
        [JsonIgnore]
        long? pauseState;

        [ObservableProperty]
        [JsonIgnore]
        long? start;

        [ObservableProperty]
        [JsonIgnore]
        long? totalLines;

        [ObservableProperty]
        [JsonIgnore]
        int? repeat;
    
        #endregion

        #region JsonIgnored

        [ObservableProperty]
        [JsonIgnore]
        bool isActive = true;

        [ObservableProperty]
        [JsonIgnore]
        bool isOnline = true;

        [ObservableProperty]
        [JsonIgnore]
        double progress = 0;

        [ObservableProperty]
        [JsonIgnore]
        string activeJobName;

        [ObservableProperty]
        [JsonIgnore]
        string activeJobId;

        [ObservableProperty]
        [JsonIgnore]
        string activeJobState;

        [ObservableProperty]
        [JsonIgnore]
        bool isPrinting = false;

        [ObservableProperty]
        [JsonIgnore]
        bool isPaused = false;

        [ObservableProperty]
        [JsonIgnore]
        bool isSelected = false;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? extruder1Temperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? extruder2Temperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? extruder3Temperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? extruder4Temperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? extruder5Temperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? heatedBedTemperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? heatedChamberTemperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? printProgress = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? remainingPrintDuration = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? printStarted = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? printDuration = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? printDurationEstimated = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        byte[] currentPrintImage = Array.Empty<byte>();

        #endregion


        #region Methods

        public Task<bool> HomeAsync(IPrint3dServerClient client, bool x, bool y, bool z) => client?.HomeAsync(x, y, z);

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        public override bool Equals(object obj)
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
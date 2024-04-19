using AndreasReitberger.API.OctoPrint.Enum;
using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateTemperatureInfo : ObservableObject, IHeaterComponent
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [JsonProperty("actual", NullValueHandling = NullValueHandling.Ignore)]
        public double? TempRead { get; set; } = 0;

        [JsonProperty("target")]
        public double? TempSet { get; set; } = 0;

        [JsonProperty("offset", NullValueHandling = NullValueHandling.Ignore)]
        public double? Offset { get; set; } = 0;

        [JsonIgnore]
        public OctoPrintCurrentToolState State { get => GetCurrentState(); }
        [ObservableProperty]
        Printer3dHeaterType type = Printer3dHeaterType.Extruder;

        #region Interface, unused
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string name;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        long error;
        #endregion

        #endregion

        #region Static
        public static OctoPrintPrinterStateTemperatureInfo Default = new()
        {
            TempRead = 0,
            TempSet = 0,
            Offset = 0,
        };
        #endregion

        #region Methods
        OctoPrintCurrentToolState GetCurrentState()
        {
            if (TempSet is null || TempSet < 0 || TempRead is null || TempRead < 0)
                return OctoPrintCurrentToolState.Error;
            else
            {
                if (TempSet <= 0)
                    return OctoPrintCurrentToolState.Idle;
                // Check if temperature is reached with a hysteresis
                else if (TempSet > TempRead && Math.Abs((double)TempSet - (double)TempRead) > 2)
                    return OctoPrintCurrentToolState.Heating;
                else
                    return OctoPrintCurrentToolState.Ready;
            }
        }
        public Task<bool> SetTemperatureAsync(IPrint3dServerClient client, string command, object data) => client?.SetExtruderTemperatureAsync(command, data);
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}

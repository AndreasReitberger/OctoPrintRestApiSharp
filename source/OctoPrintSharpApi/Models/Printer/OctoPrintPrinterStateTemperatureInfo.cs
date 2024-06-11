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

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("actual", NullValueHandling = NullValueHandling.Ignore)]
        double? tempRead = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("target")]
        double? tempSet = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("offset", NullValueHandling = NullValueHandling.Ignore)]
        double? offset = 0;

        [JsonIgnore]
        public Printer3dToolHeadState State { get => GetCurrentState(); }
        //public OctoPrintCurrentToolState State { get => GetCurrentState(); }

        [ObservableProperty]
        Printer3dHeaterType type = Printer3dHeaterType.Extruder;

        #region Interface, unused
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string name = string.Empty;

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
        public Printer3dToolHeadState GetCurrentState()
        {
            if (TempSet is null || TempSet < 0 || TempRead is null || TempRead < 0)
                return Printer3dToolHeadState.Error;
            else
            {
                if (TempSet <= 0)
                    return Printer3dToolHeadState.Idle;
                // Check if temperature is reached with a hysteresis
                else if (TempSet > TempRead && Math.Abs((double)TempSet - (double)TempRead) > 2)
                    return Printer3dToolHeadState.Heating;
                else
                    return Printer3dToolHeadState.Ready;
            }
        }
        public Task<bool> SetTemperatureAsync(IPrint3dServerClient client, string command, object? data) => client.SetExtruderTemperatureAsync(command, data);
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}

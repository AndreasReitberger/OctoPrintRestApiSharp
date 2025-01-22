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
        [ObservableProperty]
        
        [JsonIgnore]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        
        [JsonProperty("actual", NullValueHandling = NullValueHandling.Ignore)]
        public partial double? TempRead { get; set; } = 0;

        [ObservableProperty]
        
        [JsonProperty("target")]
        public partial double? TempSet { get; set; } = 0;

        [ObservableProperty]
        
        [JsonProperty("offset", NullValueHandling = NullValueHandling.Ignore)]
        public partial double? Offset { get; set; } = 0;
        [JsonIgnore]
        public Printer3dToolHeadState State { get => GetCurrentState(); }
        //public OctoPrintCurrentToolState State { get => GetCurrentState(); }

        [ObservableProperty]
        public partial Printer3dHeaterType Type { get; set; } = Printer3dHeaterType.Extruder;

        #region Interface, unused
        [ObservableProperty]
        
        [JsonIgnore]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonIgnore]
        public partial long Error { get; set; }
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

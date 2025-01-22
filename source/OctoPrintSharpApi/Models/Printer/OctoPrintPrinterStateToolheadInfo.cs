using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateToolheadInfo : OctoPrintPrinterStateTemperatureInfo, IToolhead
    {
        #region Properties

        #region Interface, unused

        [ObservableProperty]
       
        [JsonIgnore]
        public partial double X { get; set; } = 0;

        [ObservableProperty]
        
        [JsonIgnore]
        public partial double Y { get; set; } = 0;

        [ObservableProperty]
        
        [JsonIgnore]
        public partial double Z { get; set; } = 0;

        #endregion

        #endregion

        #region Static
        public new static OctoPrintPrinterStateToolheadInfo Default = new()
        {
            TempRead = 0,
            TempSet = 0,
            Offset = 0,
        };
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}

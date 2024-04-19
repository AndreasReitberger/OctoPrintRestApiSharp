using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateToolheadInfo : OctoPrintPrinterStateTemperatureInfo, IToolhead
    {
        #region Properties

        [ObservableProperty]
        Printer3dHeaterType type = Printer3dHeaterType.Extruder;

        #region Interface, unused
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double x = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double y = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double z = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        long error;
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

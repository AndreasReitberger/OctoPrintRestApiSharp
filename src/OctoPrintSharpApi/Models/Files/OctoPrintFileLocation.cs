using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileLocation : ObservableObject
    {
        #region Static
        public static List<OctoPrintFileLocation> GetList() =>
        [
            new() { Location = "local", Icon = "\U000f02ca" },
            new() { Location = "sdcard", Icon = "\U000f07dc" }
        ];

        #endregion

        #region Properties
        [ObservableProperty]
        public partial string Location { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Icon { get; set; } = string.Empty;
        #endregion

        #region Constructor
        public OctoPrintFileLocation() { }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return Location;
        }
        #endregion
    }
}

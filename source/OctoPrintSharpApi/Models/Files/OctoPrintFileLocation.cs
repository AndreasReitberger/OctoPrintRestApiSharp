using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintFileLocation
    {
        #region Static
        public static List<OctoPrintFileLocation> GetList()
        {
            return new List<OctoPrintFileLocation>
            {
                new OctoPrintFileLocation() { Location = "local", Icon = "\U000f02ca" },
                new OctoPrintFileLocation() { Location = "sdcard", Icon = "\U000f07dc" }
            };
        }

        #endregion

        #region Properties
        public string Location { get; set; }
        public string Icon { get; set; }
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

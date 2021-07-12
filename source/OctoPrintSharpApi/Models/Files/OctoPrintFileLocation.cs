using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models
{
    public class OctoPrintFileLocation
    {
        #region Static
        public static List<OctoPrintFileLocation> GetList()
        {
            var list = new List<OctoPrintFileLocation>();

            list.Add(new OctoPrintFileLocation() { Location = "local", Icon = "\U000f02ca" });
            list.Add(new OctoPrintFileLocation() { Location = "sdcard", Icon = "\U000f07dc" });

            return list;
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

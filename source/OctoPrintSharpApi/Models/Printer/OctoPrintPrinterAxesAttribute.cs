﻿using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterAxesAttribute
    {
        #region Properties
        [JsonProperty("inverted")]
        public bool Inverted { get; set; }

        [JsonProperty("speed")]
        public long Speed { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateState
    {
        #region Properties
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, bool> Flags { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

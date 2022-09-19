using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileChildRefs
    {
        #region Properties
        [JsonProperty("download")]
        public Uri Download { get; set; }

        [JsonProperty("resource")]
        public Uri Resource { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

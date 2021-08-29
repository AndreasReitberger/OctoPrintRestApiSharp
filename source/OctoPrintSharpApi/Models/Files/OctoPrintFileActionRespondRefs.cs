using System;
using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileActionRespondRefs
    {
        #region Properties
        [JsonProperty("download", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Download { get; set; }

        [JsonProperty("resource", NullValueHandling = NullValueHandling.Ignore)]
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

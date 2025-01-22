using System;
using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileActionRespondRefs : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("download", NullValueHandling = NullValueHandling.Ignore)]
        public partial Uri? Download { get; set; }

        [ObservableProperty]

        [JsonProperty("resource", NullValueHandling = NullValueHandling.Ignore)]
        public partial Uri? Resource { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileChildRefs : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("download")]
        public partial Uri? Download { get; set; }

        [ObservableProperty]
        
        [JsonProperty("resource")]
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

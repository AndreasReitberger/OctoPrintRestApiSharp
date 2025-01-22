using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateState : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Text { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        public partial Dictionary<string, bool> Flags { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

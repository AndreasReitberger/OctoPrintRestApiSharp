using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateState : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        string text = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        Dictionary<string, bool> flags = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

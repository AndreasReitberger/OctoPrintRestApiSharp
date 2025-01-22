using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterProfiles : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("profiles")]
        public partial Dictionary<string, OctoPrintPrinter> Profiles { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }

}

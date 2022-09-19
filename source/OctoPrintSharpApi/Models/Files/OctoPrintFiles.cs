using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFiles
    {
        #region Properties
        [JsonProperty("files")]
        public List<OctoPrintFile> Files { get; set; } = new();


        [JsonProperty("children")]
        public List<OctoPrintFile> Children { get; set; } = new();

        [JsonProperty("free")]
        public long Free { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

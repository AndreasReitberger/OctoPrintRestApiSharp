using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintVersionInfo
    {
        #region Properties
        [JsonProperty("api")]
        public string Api { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

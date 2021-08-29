using Newtonsoft.Json;
using System;


namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinter
    {
        #region Properties
        [JsonProperty("axes")]
        public OctoPrintPrinterAxes Axes { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("current")]
        public bool Current { get; set; }

        [JsonProperty("default")]
        public bool DefaultDefault { get; set; }

        [JsonProperty("extruder")]
        public OctoPrintPrinterExtruder Extruder { get; set; }

        [JsonProperty("heatedBed")]
        public bool HeatedBed { get; set; }

        [JsonProperty("heatedChamber")]
        public bool HeatedChamber { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("resource")]
        public Uri Resource { get; set; }

        [JsonProperty("volume")]
        public OctoPrintPrinterVolume Volume { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

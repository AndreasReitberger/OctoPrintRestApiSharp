using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintJobInfoJob
    {
        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintJobInfoFile File { get; set; }
        //public OctoPrintFile File { get; set; }

        [JsonProperty("estimatedPrintTime", NullValueHandling = NullValueHandling.Ignore)]
        public long EstimatedPrintTime { get; set; }

        [JsonProperty("filament", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintJobInfoFilament Filament { get; set; }
    }
}

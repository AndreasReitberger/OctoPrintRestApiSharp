using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintJobInfo
    {
        [JsonProperty("job")]
        public OctoPrintJobInfoJob Job { get; set; }

        [JsonProperty("progress")]
        public OctoPrintJobInfoProgress Progress { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }

}

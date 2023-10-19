using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFile : ObservableObject, IGcode
    {
        #region Properties
        [JsonIgnore]
        public bool IsAnalysed
        {
            get => GcodeAnalysis != null;
        }
        [JsonIgnore]
        public bool Printed
        {
            get => Statistics != null;
        }

        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<OctoPrintFile> Children { get; set; } = new();

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("display", NullValueHandling = NullValueHandling.Ignore)]
        public string Display { get; set; }

        [JsonProperty("gcodeAnalysis", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintFileGcodeAnalysis GcodeAnalysis { get; set; }

        [JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
        public string Hash { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("prints")]
        public OctoPrintFilePrints Prints { get; set; }

        [JsonProperty("refs", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintFileChildRefs Refs { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("statistics")]
        public OctoPrintFileStatistics Statistics { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("typePath", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> TypePath { get; set; } = new();

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        
        #endregion

    }
}

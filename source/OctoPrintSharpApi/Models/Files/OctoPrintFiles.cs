using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFiles
    {
        #region Properties
        [JsonProperty("files")]
        public List<IGcode> Files { get; set; } = new();
        //public List<OctoPrintFile> Files { get; set; } = new();


        [JsonProperty("children")]
        public List<IGcode> Children { get; set; } = new();
        //public List<OctoPrintFile> Children { get; set; } = new();

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

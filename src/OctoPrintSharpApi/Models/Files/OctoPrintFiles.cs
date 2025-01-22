using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFiles : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("files")]
        public partial List<IGcode> Files { get; set; } = [];

        //public List<OctoPrintFile> Files; = new();

        [ObservableProperty]

        [JsonProperty("children")]
        public partial List<IGcode> Children { get; set; } = [];

        //public List<OctoPrintFile> Children; = new();

        [ObservableProperty]

        [JsonProperty("free")]
        public partial long Free { get; set; }

        [ObservableProperty]

        [JsonProperty("total")]
        public partial long Total { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

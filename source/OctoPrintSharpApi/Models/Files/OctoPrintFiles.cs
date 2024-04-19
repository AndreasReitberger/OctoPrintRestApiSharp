using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFiles : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("files")]
        List<IGcode> files = [];
        //public List<OctoPrintFile> Files; = new();

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("children")]
        List<IGcode> children = [];
        //public List<OctoPrintFile> Children; = new();

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("free")]
        long free;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("total")]
        long total;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

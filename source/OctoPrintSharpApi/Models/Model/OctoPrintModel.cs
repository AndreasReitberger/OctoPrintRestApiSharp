using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintModel : ObservableObject
    {
        #region Properties
        public bool IsFolder => !string.IsNullOrEmpty(Type) && Type?.ToLower() == "folder";

        [ObservableProperty, JsonIgnore]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        string type = string.Empty;

        [ObservableProperty, JsonIgnore]
        string location = string.Empty;

        [ObservableProperty, JsonIgnore]
        string path = string.Empty;

        [ObservableProperty, JsonIgnore]
        OctoPrintFile? file;
        #endregion

        #region Constructor
        public OctoPrintModel() { }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

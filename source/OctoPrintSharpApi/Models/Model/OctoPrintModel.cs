using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintModel : ObservableObject
    {
        #region Properties
        public bool IsFolder => !string.IsNullOrEmpty(Type) && Type?.ToLower() == "folder";

        [ObservableProperty]
        
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        
        public partial string Location { get; set; } = string.Empty;

        [ObservableProperty]
        
        public partial string Path { get; set; } = string.Empty;

        [ObservableProperty]
        
        public partial OctoPrintFile? File { get; set; }
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

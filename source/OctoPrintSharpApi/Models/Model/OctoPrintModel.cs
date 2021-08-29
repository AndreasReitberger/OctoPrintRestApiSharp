using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class OctoPrintModel
    {
        #region Properties
        public bool IsFolder
        {
            get => !string.IsNullOrEmpty(Type) && Type == "folder";
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Path { get; set; }
        public OctoPrintFile File { get; set; }
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

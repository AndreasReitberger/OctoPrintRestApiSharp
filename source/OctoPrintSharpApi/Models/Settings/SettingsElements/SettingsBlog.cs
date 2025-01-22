using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsBlog : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Description { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Priority { get; set; }

        [ObservableProperty]
        
        [JsonProperty("read_until", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? ReadUntil { get; set; }

        [ObservableProperty]
        
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public partial Uri? Url { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

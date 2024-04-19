using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintWebCamSettingsInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(Id))]
        Guid id = Guid.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(IsDefault))]
        bool isDefault;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(Autostart))]
        bool autostart = false;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(Name))]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(Slug))]
        string slug = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(CamIndex))]
        int camIndex = -1;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(RotationAngle))]
        int rotationAngle = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(NetworkBufferTime))]
        int networkBufferTime = 150;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(FileCachingTime))]
        int fileCachingTime = 1000;
        
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

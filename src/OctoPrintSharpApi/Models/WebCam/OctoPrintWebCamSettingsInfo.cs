using AndreasReitberger.Shared.Core.Utilities;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintWebCamSettingsInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty(nameof(Id))]
        public partial Guid Id { get; set; } = Guid.Empty;

        [ObservableProperty]

        [JsonProperty(nameof(IsDefault))]
        public partial bool IsDefault { get; set; }

        [ObservableProperty]

        [JsonProperty(nameof(Autostart))]
        public partial bool Autostart { get; set; } = false;

        [ObservableProperty]

        [JsonProperty(nameof(Name))]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty(nameof(Slug))]
        public partial string Slug { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty(nameof(CamIndex))]
        public partial int CamIndex { get; set; } = -1;

        [ObservableProperty]

        [JsonProperty(nameof(RotationAngle))]
        public partial int RotationAngle { get; set; } = 0;

        [ObservableProperty]

        [JsonProperty(nameof(NetworkBufferTime))]
        public partial int NetworkBufferTime { get; set; } = 150;

        [ObservableProperty]

        [JsonProperty(nameof(FileCachingTime))]
        public partial int FileCachingTime { get; set; } = 1000;

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

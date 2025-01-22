using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class GcodeCommandInfo : ObservableObject
    {
        #region Properties

        [ObservableProperty]

        [JsonProperty(nameof(Id))]
        public partial Guid Id { get; set; }

        [ObservableProperty]

        [JsonProperty(nameof(Command))]
        public partial string Command { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty(nameof(Sent))]
        public partial bool Sent { get; set; } = false;

        [ObservableProperty]

        [JsonProperty(nameof(Succeeded))]
        public partial bool Succeeded { get; set; } = false;

        [ObservableProperty]

        [JsonProperty(nameof(TimeStamp))]
        public partial DateTime TimeStamp { get; set; } = DateTime.Now;

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

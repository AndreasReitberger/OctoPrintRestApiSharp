using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class GcodeCommandInfo : ObservableObject
    {
        #region Properties

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(Id))]
        Guid id;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(Command))]
        string command = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(Sent))]
        bool sent = false;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(Succeeded))]
        bool succeeded = false;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty(nameof(TimeStamp))]
        DateTime timeStamp = DateTime.Now;

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

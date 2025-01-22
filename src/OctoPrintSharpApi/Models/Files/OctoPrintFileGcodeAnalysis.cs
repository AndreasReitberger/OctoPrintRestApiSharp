using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileGcodeAnalysis : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("dimensions")]
        public partial OctoPrintFileDimensions Dimensions { get; set; } = new() { Depth = 0, Height = 0, Width = 0 };

        [ObservableProperty]

        [JsonProperty("estimatedPrintTime")]
        public partial double EstimatedPrintTime { get; set; } = 0;

        [ObservableProperty]

        [NotifyPropertyChangedFor(nameof(TotalFilamentLength))]
        [NotifyPropertyChangedFor(nameof(TotalFilamentVolume))]
        [JsonProperty("filament")]
        public partial Dictionary<string, OctoPrintFilament> Filament { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("printingArea")]
        public partial Dictionary<string, double> PrintingArea { get; set; } = [];

        [JsonIgnore]
        public double TotalFilamentVolume
        {
            get
            {
                double filamentUsed = 0;
                if (Filament is not null)
                {
                    foreach (KeyValuePair<string, OctoPrintFilament> pair in Filament)
                    {
                        try
                        {
                            if (pair.Value is not null)
                                filamentUsed += pair.Value.Volume;
                        }
                        catch (Exception)
                        { continue; }
                    }
                }
                return filamentUsed;
            }
        }

        [JsonIgnore]
        public double TotalFilamentLength
        {
            get
            {
                double filamentUsed = 0;
                if (Filament is not null)
                {
                    foreach (KeyValuePair<string, OctoPrintFilament> pair in Filament)
                    {
                        try
                        {
                            if (pair.Value is not null)
                                filamentUsed += pair.Value.Length;
                        }
                        catch (Exception)
                        { continue; }
                    }
                }
                return filamentUsed;
            }
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

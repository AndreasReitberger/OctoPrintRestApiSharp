using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileGcodeAnalysis : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("dimensions")]
        OctoPrintFileDimensions dimensions = new() { Depth = 0, Height = 0, Width = 0 };

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("estimatedPrintTime")]
        double estimatedPrintTime = 0;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(TotalFilamentLength))]
        [NotifyPropertyChangedFor(nameof(TotalFilamentVolume))]
        [property: JsonProperty("filament")]
        Dictionary<string, OctoPrintFilament> filament = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printingArea")]
        Dictionary<string, double> printingArea = [];

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

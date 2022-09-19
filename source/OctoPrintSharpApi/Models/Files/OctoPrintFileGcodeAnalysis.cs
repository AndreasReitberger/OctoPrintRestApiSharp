using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileGcodeAnalysis
    {
        #region Properties
        [JsonProperty("dimensions")]
        public OctoPrintFileDimensions Dimensions { get; set; } = new OctoPrintFileDimensions() { Depth = 0, Height = 0, Width = 0 };

        [JsonProperty("estimatedPrintTime")]
        public double EstimatedPrintTime { get; set; } = 0;

        [JsonProperty("filament")]
        public Dictionary<string, OctoPrintFilament> Filament { get; set; } = new();

        [JsonProperty("printingArea")]
        public Dictionary<string, double> PrintingArea { get; set; } = new();

        [JsonIgnore]
        public double TotalFilamentVolume
        {
            get
            {
                double filamentUsed = 0;
                if(Filament != null)
                {
                    foreach(KeyValuePair<string, OctoPrintFilament> pair in Filament)
                    {
                        try
                        {
                            if (pair.Value != null)
                                filamentUsed += pair.Value.Volume;
                        }
                        catch(Exception)
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
                if(Filament != null)
                {
                    foreach(KeyValuePair<string, OctoPrintFilament> pair in Filament)
                    {
                        try
                        {
                            if (pair.Value != null)
                                filamentUsed += pair.Value.Length;
                        }
                        catch(Exception)
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

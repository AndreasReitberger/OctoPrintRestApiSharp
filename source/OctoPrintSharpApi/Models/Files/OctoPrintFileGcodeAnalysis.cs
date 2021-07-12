using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileGcodeAnalysis
    {
        [JsonProperty("dimensions")]
        public OctoPrintFileDimensions Dimensions { get; set; } = new OctoPrintFileDimensions() { Depth = 0, Height = 0, Width = 0 };

        [JsonProperty("estimatedPrintTime")]
        public double EstimatedPrintTime { get; set; } = 0;

        [JsonProperty("filament")]
        public Dictionary<string, OctoPrintPrinterFilament> Filament { get; set; }

        [JsonProperty("printingArea")]
        public Dictionary<string, double> PrintingArea { get; set; }

        [JsonIgnore]
        public double TotalFilamentVolume
        {
            get
            {
                double filamentUsed = 0;
                if(Filament != null)
                {
                    foreach(KeyValuePair<string, OctoPrintPrinterFilament> pair in Filament)
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
                    foreach(KeyValuePair<string, OctoPrintPrinterFilament> pair in Filament)
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
    }
}

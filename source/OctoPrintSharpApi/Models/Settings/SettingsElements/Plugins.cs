using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class Plugins
    {
        [JsonProperty("action_command_prompt", NullValueHandling = NullValueHandling.Ignore)]
        public ActionCommandPrompt ActionCommandPrompt { get; set; }

        [JsonProperty("announcements", NullValueHandling = NullValueHandling.Ignore)]
        public Announcements Announcements { get; set; }

        [JsonProperty("discovery", NullValueHandling = NullValueHandling.Ignore)]
        public Discovery Discovery { get; set; }

        [JsonProperty("errortracking", NullValueHandling = NullValueHandling.Ignore)]
        public Errortracking Errortracking { get; set; }

        [JsonProperty("pi_support", NullValueHandling = NullValueHandling.Ignore)]
        public PiSupport PiSupport { get; set; }

        [JsonProperty("pluginmanager", NullValueHandling = NullValueHandling.Ignore)]
        public Pluginmanager Pluginmanager { get; set; }

        [JsonProperty("slic3r", NullValueHandling = NullValueHandling.Ignore)]
        public Slic3R Slic3R { get; set; }

        [JsonProperty("softwareupdate", NullValueHandling = NullValueHandling.Ignore)]
        public Softwareupdate Softwareupdate { get; set; }

        [JsonProperty("tracking", NullValueHandling = NullValueHandling.Ignore)]
        public Tracking Tracking { get; set; }
    }
}

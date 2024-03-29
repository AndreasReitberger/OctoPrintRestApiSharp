﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintWebSocketConnectionInfo
    {
        #region Properties
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("display_version")]
        public string DisplayVersion { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("plugin_hash")]
        public string PluginHash { get; set; }

        [JsonProperty("config_hash")]
        public string ConfigHash { get; set; }

        [JsonProperty("debug")]
        public bool Debug { get; set; }

        [JsonProperty("safe_mode")]
        public object SafeMode { get; set; }

        [JsonProperty("permissions")]
        public List<OctoPrintWebSocketConnectionPermission> Permissions { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

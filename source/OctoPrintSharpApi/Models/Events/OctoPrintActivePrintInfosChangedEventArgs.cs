﻿using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.OctoPrint.Models
{
    [Obsolete("Use JobStatusChangedEventArgs instead")]
    public class OctoPrintActivePrintInfosChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public ObservableCollection<object> NewActivePrintInfos { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

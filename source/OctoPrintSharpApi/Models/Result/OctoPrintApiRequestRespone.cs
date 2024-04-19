using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    [Obsolete("Use IRestApiRequestResponse instead")]
    public class OctoPrintApiRequestResponse
    {
        #region Properties
        public string Result { get; set; } = string.Empty;
        public bool IsOnline { get; set; } = false;
        public bool Succeeded { get; set; } = false;
        public bool HasAuthenticationError { get; set; } = false;

        public RestEventArgs? EventArgs { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

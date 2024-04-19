using AndreasReitberger.API.OctoPrint.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {
        #region Static

        public static OctoPrintConnectionStates ConvertConnectionStateString(string ConnectionState)
        {
            try
            {
                OctoPrintConnectionStates state = OctoPrintConnectionStates.Unknown;
                string cropped = ConnectionState.Replace(" ", string.Empty);
                System.Enum.TryParse(cropped, out state);
                if (state == OctoPrintConnectionStates.Unknown)
                {
                    // Just for debugging
                }
                return state;
            }
            catch (Exception)
            {
                return OctoPrintConnectionStates.Unknown;
            }
        }
        #endregion
    }
}

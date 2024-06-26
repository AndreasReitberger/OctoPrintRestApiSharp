﻿using AndreasReitberger.API.OctoPrint.Enum;
using AndreasReitberger.API.Print3dServer.Core.Enums;
using System;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {
        #region Static

        public static Print3dJobState ConvertConnectionState(string ConnectionState)
        {
            try
            {
                Print3dJobState state = Print3dJobState.Unknown;
                string cropped = ConnectionState.Replace(" ", string.Empty);
                System.Enum.TryParse(cropped, out state);
                if (state == Print3dJobState.Unknown)
                {
                    // Just for debugging
                }
                return state;
            }
            catch (Exception)
            {
                return Print3dJobState.Unknown;
            }
        }
        #endregion
    }
}

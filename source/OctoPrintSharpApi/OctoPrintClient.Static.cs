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
        public static string ConvertStackToPath(Stack<string> stack, string separator)
        {
            StringBuilder sb = new();
            for (int i = stack.Count - 1; i >= 0; i--)
            {
                sb.Append(stack.ElementAt(i));
                if (i > 0)
                    sb.Append(separator);
            }
            return sb.ToString();
        }

        public static OctoPrintConnectionStates ConvertConnectionStateString(string ConnectionState)
        {
            try
            {
                OctoPrintConnectionStates state = OctoPrintConnectionStates.Unkown;
                string cropped = ConnectionState.Replace(" ", string.Empty);
                System.Enum.TryParse(cropped, out state);
                if (state == OctoPrintConnectionStates.Unkown)
                {
                    // Just for debugging
                }
                return state;
            }
            catch (Exception)
            {
                return OctoPrintConnectionStates.Unkown;
            }
        }
        #endregion
    }
}

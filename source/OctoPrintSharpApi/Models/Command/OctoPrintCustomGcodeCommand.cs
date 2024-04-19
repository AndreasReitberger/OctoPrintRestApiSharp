using AndreasReitberger.API.OctoPrint.Enum;
using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public struct OctoPrintCustomGcodeCommand
    {
        #region Properties
        public OctoPrintCommandName Name;
        public OctoPrintCommandType Type;
        public string Command;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

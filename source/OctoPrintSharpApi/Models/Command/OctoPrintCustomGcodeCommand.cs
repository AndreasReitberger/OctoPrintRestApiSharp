using AndreasReitberger.API.OctoPrint.Enum;
using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public struct OctoPrintCustomGcodeCommand
    {
        #region Properties
        public OctoPrintCommandName Name { get; set; }
        public OctoPrintCommandType Type { get; set; }
        public string Command { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}

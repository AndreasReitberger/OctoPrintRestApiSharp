using AndreasReitberger.Enum;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public class OctoPrintCustomGcodeCommands
    {
        #region Static
        public static List<OctoPrintCustomGcodeCommand> Commands = new()
        {
            new OctoPrintCustomGcodeCommand() { Type = OctoPrintCommandType.command, Name = OctoPrintCommandName.SetFanSpeed, Command = "M106 S{0}"},
            new OctoPrintCustomGcodeCommand() { Type = OctoPrintCommandType.command, Name = OctoPrintCommandName.SetFanOn, Command = "M106 S255"},
            new OctoPrintCustomGcodeCommand() { Type = OctoPrintCommandType.command, Name = OctoPrintCommandName.SetFanOff, Command = "M106 S0"},
            new OctoPrintCustomGcodeCommand() { Type = OctoPrintCommandType.command, Name = OctoPrintCommandName.MotorsOff, Command = "M18"},
        };
        #endregion
    }
}

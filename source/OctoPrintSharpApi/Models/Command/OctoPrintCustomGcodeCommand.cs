using AndreasReitberger.Enum;

namespace AndreasReitberger.Models
{
    public struct OctoPrintCustomGcodeCommand
    {
        public OctoPrintCommandName Name { get; set; }
        public OctoPrintCommandType Type { get; set; }
        public string Command { get; set; }
    }
}

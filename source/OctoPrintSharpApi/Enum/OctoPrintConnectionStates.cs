using System;

namespace AndreasReitberger.API.OctoPrint.Enum
{
    [Obsolete("Use PrintJob3dState instead")]
    public enum OctoPrintConnectionStates
    {
        Operational,
        Connecting,
        Printing,
        Closed,
        Offline,

        Unknown = 99,
    }
}

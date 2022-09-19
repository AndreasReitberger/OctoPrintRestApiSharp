namespace AndreasReitberger.API.OctoPrint.Enum
{
    public enum OctoPrintConnectionStates
    {
        Operational,
        Connecting,
        Printing,
        Closed,
        Offline,

        Unkown = 99,
    }
}

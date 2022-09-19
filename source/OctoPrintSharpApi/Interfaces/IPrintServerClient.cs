using System;
using System.ComponentModel;

namespace AndreasReitberger.API.OctoPrint.Interfaces
{
    public interface IPrintServerClient : INotifyPropertyChanged, IDisposable
    {
        #region Properties
        //public bool IsInitialized { get; set; }
        #endregion
    }
}

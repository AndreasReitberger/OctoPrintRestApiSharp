using System;
using System.ComponentModel;

namespace AndreasReitberger.Interfaces
{
    public interface IPrintServerClient : INotifyPropertyChanged, IDisposable
    {
        #region Properties
        //public bool IsInitialized { get; set; }
        #endregion
    }
}

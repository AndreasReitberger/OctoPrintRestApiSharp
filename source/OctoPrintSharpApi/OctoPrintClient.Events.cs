using AndreasReitberger.API.OctoPrint.Models;
using System;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {

        #region EventHandlers

        #region ServerStateChanges
        public event EventHandler<OctoPrintMessagesChangedEventArgs>? MessagesChanged;
        protected virtual void OnMessagesChanged(OctoPrintMessagesChangedEventArgs e)
        {
            MessagesChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintActivePrintInfosChangedEventArgs>? PrintInfosChanged;
        protected virtual void OnPrintInfosChanged(OctoPrintActivePrintInfosChangedEventArgs e)
        {
            PrintInfosChanged?.Invoke(this, e);
        }
        [Obsolete("Use the event JobStatusChanged isntead")]
        public event EventHandler<OctoPrintActivePrintInfoChangedEventArgs>? PrintInfoChanged;
        [Obsolete("Use the event JobStatusChanged isntead")]
        protected virtual void OnPrintInfoChanged(OctoPrintActivePrintInfoChangedEventArgs e)
        {
            PrintInfoChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintTempDataEventArgs>? TempDataReceived;
        protected virtual void OnTempDataReceived(OctoPrintTempDataEventArgs e)
        {
            TempDataReceived?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintPrinterConfigChangedEventArgs>? OctoPrintPrinterConfigChanged;
        protected virtual void OnOctoPrintPrinterConfigChanged(OctoPrintPrinterConfigChangedEventArgs e)
        {
            OctoPrintPrinterConfigChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintPrinterStateChangedEventArgs>? OctoPrintPrinterStateChanged;
        protected virtual void OnOctoPrintPrinterStateChanged(OctoPrintPrinterStateChangedEventArgs e)
        {
            OctoPrintPrinterStateChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintConnectionSettingsChangedEventArgs>? OctoPrintConnectionSettingsChanged;
        protected virtual void OnOctoPrintConnectionSettingsChanged(OctoPrintConnectionSettingsChangedEventArgs e)
        {
            OctoPrintConnectionSettingsChanged?.Invoke(this, e);
        }
        /*
        [Obsolete("Use OnGcodesChangedEvent instead")]
        public event EventHandler<OctoPrintModelsChangedEventArgs>? OctoPrintModelsChanged;
        [Obsolete("Use OnGcodesChangedEvent instead")]
        protected virtual void OnOctoPrintModelsChanged(OctoPrintModelsChangedEventArgs e)
        {
            OctoPrintModelsChanged?.Invoke(this, e);
        }

        [Obsolete("Use OnGcodeGroupsChangedEvent instead")]
        public event EventHandler<OctoPrintModelGroupsChangedEventArgs>? OctoPrintModelGroupsChanged;
        [Obsolete("Use OnGcodeGroupsChangedEvent instead")]
        protected virtual void OnOctoPrintModelGroupsChanged(OctoPrintModelGroupsChangedEventArgs e)
        {
            OctoPrintModelGroupsChanged?.Invoke(this, e);
        }
        */
        #endregion

        #endregion

    }
}

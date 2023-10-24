using AndreasReitberger.API.OctoPrint.Models;
using System;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {

        #region EventHandlerss

        #region ServerStateChanges
        public event EventHandler<OctoPrintMessagesChangedEventArgs> MessagesChanged;
        protected virtual void OnMessagesChanged(OctoPrintMessagesChangedEventArgs e)
        {
            MessagesChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintActivePrintInfosChangedEventArgs> PrintInfosChanged;
        protected virtual void OnPrintInfosChanged(OctoPrintActivePrintInfosChangedEventArgs e)
        {
            PrintInfosChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintActivePrintInfoChangedEventArgs> PrintInfoChanged;
        protected virtual void OnPrintInfoChanged(OctoPrintActivePrintInfoChangedEventArgs e)
        {
            PrintInfoChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintJobsChangedEventArgs> JobsChanged;
        protected virtual void OnJobsChanged(OctoPrintJobsChangedEventArgs e)
        {
            JobsChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintJobFinishedEventArgs> JobFinished;
        protected virtual void OnJobFinished(OctoPrintJobFinishedEventArgs e)
        {
            JobFinished?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintTempDataEventArgs> TempDataReceived;
        protected virtual void OnTempDataReceived(OctoPrintTempDataEventArgs e)
        {
            TempDataReceived?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintPrinterConfigChangedEventArgs> OctoPrintPrinterConfigChanged;
        protected virtual void OnOctoPrintPrinterConfigChanged(OctoPrintPrinterConfigChangedEventArgs e)
        {
            OctoPrintPrinterConfigChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintPrinterStateChangedEventArgs> OctoPrintPrinterStateChanged;
        protected virtual void OnOctoPrintPrinterStateChanged(OctoPrintPrinterStateChangedEventArgs e)
        {
            OctoPrintPrinterStateChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintConnectionSettingsChangedEventArgs> OctoPrintConnectionSettingsChanged;
        protected virtual void OnOctoPrintConnectionSettingsChanged(OctoPrintConnectionSettingsChangedEventArgs e)
        {
            OctoPrintConnectionSettingsChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintModelsChangedEventArgs> OctoPrintModelsChanged;
        protected virtual void OnOctoPrintModelsChanged(OctoPrintModelsChangedEventArgs e)
        {
            OctoPrintModelsChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintModelGroupsChangedEventArgs> OctoPrintModelGroupsChanged;
        protected virtual void OnOctoPrintModelGroupsChanged(OctoPrintModelGroupsChangedEventArgs e)
        {
            OctoPrintModelGroupsChanged?.Invoke(this, e);
        }

        public event EventHandler<OctoPrintActivePrinterChangedEventArgs> ActivePrinterChanged;
        protected virtual void OnActivePrinterChanged(OctoPrintActivePrinterChangedEventArgs e)
        {
            ActivePrinterChanged?.Invoke(this, e);
        }
        #endregion

        #endregion

    }
}

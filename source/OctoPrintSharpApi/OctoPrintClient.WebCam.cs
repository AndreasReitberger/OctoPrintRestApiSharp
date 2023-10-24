using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {

        #region Methods
        public override async Task<ObservableCollection<IWebCamConfig>> GetWebCamConfigsAsync()
        {
            // There is only support for one camera
            await Task.Delay(1);
            return new();
        }

        [Obsolete("Use GetDefaultWebCamUri instead")]
        public string GetWebCamUri()
        {
            try
            {
                string currentPrinter = GetActivePrinterSlug();
                if (string.IsNullOrEmpty(currentPrinter)) return string.Empty;
                return $"{FullWebAddress}/webcam/?action=stream?t={ApiKey}";
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return "";
            }
        }

        #endregion

    }
}

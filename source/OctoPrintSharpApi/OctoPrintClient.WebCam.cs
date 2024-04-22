using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {

        #region Methods
        public override async Task<List<IWebCamConfig>?> GetWebCamConfigsAsync()
        {
            // There is only support for one camera
            await Task.Delay(1);
            return [];
        }

        #endregion

    }
}

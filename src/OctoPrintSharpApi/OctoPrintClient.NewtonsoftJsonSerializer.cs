using AndreasReitberger.API.OctoPrint.Models;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.JSON.Newtonsoft;
using AndreasReitberger.API.REST;
using AndreasReitberger.API.REST.Interfaces;
using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {

#if DEBUG
        #region Debug

        public new static JsonSerializerSettings DefaultNewtonsoftJsonSerializerSettings = new()
        {
            // Detect if the json respone has more or less properties than the target class
            //MissingMemberHandling = MissingMemberHandling.Error,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Include,
            TypeNameHandling = TypeNameHandling.Auto,
            Converters =
            {
                // Map the converters
                new AbstractConverter<AuthenticationHeader, IAuthenticationHeader>(),
                new AbstractConverter<OctoPrintPrinter, IPrinter3d>(),
                new AbstractConverter<OctoPrintFile, IGcode>(),
                new AbstractConverter<OctoPrintGroup, IGcodeGroup>(),
                new AbstractConverter<OctoPrintPrinterStateTemperatureInfo, IHeaterComponent>(),
                new AbstractConverter<OctoPrintPrinterStateToolheadInfo, IToolhead>(),
                new AbstractConverter<OctoPrintJobInfo, IPrint3dJobStatus>(),
            }
        };
        #endregion
#else
        #region Release
        public new static JsonSerializerSettings DefaultNewtonsoftJsonSerializerSettings = new()
        {
            // Ignore if the json respone has more or less properties than the target class
            MissingMemberHandling = MissingMemberHandling.Ignore,          
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            Converters =
            {
                // Map the converters
                new AbstractConverter<AuthenticationHeader, IAuthenticationHeader>(),
                new AbstractConverter<OctoPrintPrinter, IPrinter3d>(),
                new AbstractConverter<OctoPrintFile, IGcode>(),
                new AbstractConverter<OctoPrintGroup, IGcodeGroup>(),
                new AbstractConverter<OctoPrintPrinterStateTemperatureInfo, IHeaterComponent>(),
                new AbstractConverter<OctoPrintPrinterStateToolheadInfo, IToolhead>(),
                new AbstractConverter<OctoPrintJobInfo, IPrint3dJobStatus>(),
            }
        };
        #endregion
#endif
    }
}

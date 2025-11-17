using AndreasReitberger.API.OctoPrint.Models;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.JSON.System;
using AndreasReitberger.API.REST;
using AndreasReitberger.API.REST.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {

        [ObservableProperty]
        [Newtonsoft.Json.JsonIgnore, JsonIgnore, XmlIgnore]
        public new partial JsonSerializerOptions JsonSerializerSettings { get; set; } = DefaultJsonSerializerSettings;

#if DEBUG
        #region Debug

        public new static JsonSerializerOptions DefaultJsonSerializerSettings = new()
        {
            // Detect if the json respone has more or less properties than the target class
            //MissingMemberHandling = MissingMemberHandling.Error,
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true,
            Converters =
            {                     
                // Map the converters
                new TypeMappingConverter<IAuthenticationHeader, AuthenticationHeader>(),
                new TypeMappingConverter<IPrinter3d, OctoPrintPrinter>(),
                new TypeMappingConverter<IGcode, OctoPrintFile>(),
                new TypeMappingConverter<IGcodeGroup, OctoPrintGroup>(),
                new TypeMappingConverter<IHeaterComponent, OctoPrintPrinterStateTemperatureInfo>(),
                new TypeMappingConverter<IToolhead, OctoPrintPrinterStateToolheadInfo>(),
                new TypeMappingConverter<IPrint3dJobStatus, OctoPrintJobInfo>(),
            }
        };
        #endregion
#else
        #region Release
        public new static JsonSerializerOptions DefaultJsonSerializerSettings = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true,
            Converters =
            {                     
                // Map the converters
                new TypeMappingConverter<IAuthenticationHeader, AuthenticationHeader>(),
                new TypeMappingConverter<IPrinter3d, OctoPrintPrinter>(),
                new TypeMappingConverter<IGcode, OctoPrintFile>(),
                new TypeMappingConverter<IGcodeGroup, OctoPrintGroup>(),
                new TypeMappingConverter<IHeaterComponent, OctoPrintPrinterStateTemperatureInfo>(),
                new TypeMappingConverter<IToolhead, OctoPrintPrinterStateToolheadInfo>(),
            }
        };
        #endregion
#endif
    }
}

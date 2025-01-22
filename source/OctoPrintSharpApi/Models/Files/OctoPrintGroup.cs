using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintGroup : ObservableObject, IGcodeGroup
    {
        #region Properties

        [ObservableProperty]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string DirectoryName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Path { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Root { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Permissions { get; set; } = string.Empty;

        [ObservableProperty]
        public partial long Size { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ModifiedGeneralized))]
        public partial double? Modified { get; set; }
        partial void OnModifiedChanged(double? value)
        {
            if (value is not null)
                ModifiedGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty]
        public partial DateTime? ModifiedGeneralized { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            // Ordinarily, we release unmanaged resources here;
            // but all are wrapped by safe handles.

            // Release disposable objects.
            if (disposing)
            {
                // Nothing to do here
            }
        }
        #endregion

        #region Clone

        public object Clone() => MemberwiseClone();

        #endregion

    }
}

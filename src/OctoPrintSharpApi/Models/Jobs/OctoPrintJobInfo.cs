using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintJobInfo : ObservableObject, IPrint3dJobStatus
    {
        #region Properties
        [ObservableProperty]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        [JsonProperty("job")]
        public partial OctoPrintJobInfoJob? Job { get; set; }
        partial void OnJobChanged(OctoPrintJobInfoJob? value)
        {
            if (value is not null)
            {
                FileName = value.File?.Name ?? string.Empty;
                FileExists = value.File is not null;
                FilamentUsed = value.Filament?.Length * value.Filament?.Volume;
                RemainingPrintTime = value.EstimatedPrintTime;
            }
        }

        [ObservableProperty]
        [JsonProperty("progress")]
        public partial OctoPrintJobInfoProgress? Progress { get; set; }
        partial void OnProgressChanged(OctoPrintJobInfoProgress? value)
        {
            if (value is not null)
            {
                Done = value.Completion;
                TotalPrintDuration = value.PrintTime;
                PrintDuration = value.PrintTime - value.PrintTimeLeft;
            }
        }

        [ObservableProperty]
        [JsonProperty("state")]
        public partial Print3dJobState? State { get; set; }

        //string state = string.Empty;
        #endregion

        #region Interface


        [ObservableProperty]
        public partial string FileName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial double? Done { get; set; }
        partial void OnDoneChanged(double? value)
        {
            if (value is not null)
                DonePercentage = value / 100;
            else
                DonePercentage = 0;
        }

        [ObservableProperty]
        public partial double? PrintDurationTimeComp { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StartTimeGeneralized))]
        public partial double? StartTime { get; set; }
        partial void OnStartTimeChanged(double? value)
        {
            if (value is not null)
                StartTimeGeneralized = TimeBaseConvertHelper.FromDouble(value);
        }

        [ObservableProperty]
        public partial DateTime? StartTimeGeneralized { get; set; }

        [ObservableProperty]
        public partial double? EndTime { get; set; }
        partial void OnEndTimeChanged(double? value)
        {
            if (value is not null)
                EndTimeGeneralized = TimeBaseConvertHelper.FromDouble(value);
        }

        [ObservableProperty]
        public partial DateTime? EndTimeGeneralized { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PrintDurationGeneralized))]
        public partial double? PrintDuration { get; set; }
        partial void OnPrintDurationChanged(double? value)
        {
            if (value is not null)
                PrintDurationGeneralized = TimeBaseConvertHelper.FromDoubleHours(value);
        }

        [ObservableProperty]
        public partial TimeSpan? PrintDurationGeneralized { get; set; }

        [ObservableProperty]
        public partial double? TotalPrintDuration { get; set; }

        [ObservableProperty]
        public partial TimeSpan? TotalPrintDurationGeneralized { get; set; }

        [ObservableProperty]
        public partial string JobId { get; set; } = string.Empty;

        [ObservableProperty]
        public partial double? FilamentUsed { get; set; }

        [ObservableProperty]
        public partial double? DonePercentage { get; set; }

        [ObservableProperty]
        public partial bool FileExists { get; set; }

        [ObservableProperty]
        public partial IGcodeMeta? Meta { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(RemainingPrintTimeGeneralized))]
        public partial double? RemainingPrintTime { get; set; }
        partial void OnRemainingPrintTimeChanged(double? value)
        {
            if (value is not null)
                RemainingPrintTimeGeneralized = TimeBaseConvertHelper.FromDoubleSeconds(value);
        }

        [ObservableProperty]
        public partial TimeSpan? RemainingPrintTimeGeneralized { get; set; }

        [ObservableProperty]
        public partial long? Repeat { get; set; }

        #endregion

        #region Static
        public static OctoPrintJobInfo Default = new()
        {
            //State = OctoPrintConnectionStates.Operational.ToString(),
            State = Print3dJobState.Operational,
            Progress = new OctoPrintJobInfoProgress() { Completion = 0 },
            Job = new OctoPrintJobInfoJob(),
        };
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
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

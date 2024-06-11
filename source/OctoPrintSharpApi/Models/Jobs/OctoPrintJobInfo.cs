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
        [ObservableProperty, JsonIgnore]
        Guid id;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("job")]
        OctoPrintJobInfoJob? job;
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

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("progress")]
        OctoPrintJobInfoProgress? progress;
        partial void OnProgressChanged(OctoPrintJobInfoProgress? value)
        {
            if (value is not null)
            {
                Done = value.Completion;
                TotalPrintDuration = value.PrintTime;
                PrintDuration = value.PrintTime - value.PrintTimeLeft;
            }
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state")]
        Print3dJobState? state;
        //string state = string.Empty;
        #endregion

        #region Interface


        [ObservableProperty, JsonIgnore]
        string fileName = string.Empty;

        [ObservableProperty, JsonIgnore]
        double? done;
        partial void OnDoneChanged(double? value)
        {
            if (value is not null)
                DonePercentage = value / 100;
            else
                DonePercentage = 0;
        }

        [ObservableProperty, JsonIgnore]
        double? printDurationTimeComp;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(StartTimeGeneralized))]
        double? startTime;
        partial void OnStartTimeChanged(double? value)
        {
            if (value is not null)
                StartTimeGeneralized = TimeBaseConvertHelper.FromDouble(value);
        }

        [ObservableProperty, JsonIgnore]
        DateTime? startTimeGeneralized;

        [ObservableProperty, JsonIgnore]
        double? endTime;
        partial void OnEndTimeChanged(double? value)
        {
            if (value is not null)
                EndTimeGeneralized = TimeBaseConvertHelper.FromDouble(value);
        }

        [ObservableProperty, JsonIgnore]
        DateTime? endTimeGeneralized;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(PrintDurationGeneralized))]
        double? printDuration;
        partial void OnPrintDurationChanged(double? value)
        {
            if (value is not null)
                PrintDurationGeneralized = TimeBaseConvertHelper.FromDoubleHours(value);
        }

        [ObservableProperty, JsonIgnore]
        TimeSpan? printDurationGeneralized;

        [ObservableProperty, JsonIgnore]
        double? totalPrintDuration;

        [ObservableProperty, JsonIgnore]
        TimeSpan? totalPrintDurationGeneralized;

        [ObservableProperty, JsonIgnore]
        string jobId = string.Empty;

        [ObservableProperty, JsonIgnore]
        double? filamentUsed;

        [ObservableProperty, JsonIgnore]
        double? donePercentage;

        [ObservableProperty, JsonIgnore]
        bool fileExists;

        [ObservableProperty, JsonIgnore]
        IGcodeMeta? meta;

        [ObservableProperty, JsonIgnore]
        double? remainingPrintTime;
        /*
        [JsonIgnore]
        public double? RemainingPrintTime => PrintDuration > 0 ? PrintDuration - PrintDurationTimeComp : 0;
        */
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

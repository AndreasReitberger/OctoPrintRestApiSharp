using System.Collections.ObjectModel;

namespace AndreasReitberger.Models
{
    public class OctoPrintActivePrintInfosChangedEventArgs : OctoPrintEventArgs
    {
        public ObservableCollection<object> NewActivePrintInfos { get; set; } = new ObservableCollection<object>();
    }
}

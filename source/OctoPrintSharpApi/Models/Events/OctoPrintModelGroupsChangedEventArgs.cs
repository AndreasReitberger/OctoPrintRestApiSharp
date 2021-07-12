using System.Collections.ObjectModel;

namespace AndreasReitberger.Models
{
    public class OctoPrintModelGroupsChangedEventArgs : OctoPrintEventArgs
    {
        public ObservableCollection<string> NewModelGroups { get; set; } = new ObservableCollection<string>();
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzAvalonia.ViewModels
{
    public class JavaListViewModel : ViewModelBase
    {
        public JavaListViewModel(IEnumerable<string> items) 
        {
            Items = new ObservableCollection<string>(items);
        }

        public ObservableCollection<string> Items { get; }
    }
}

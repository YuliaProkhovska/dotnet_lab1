using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMWPFApp.ViewModel
{

	public class MainViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
		private object _selectedVM;
		public object SelectedVM
		{
			get => _selectedVM;
			set { _selectedVM = value; OnPropertyChanged("SelectedVM"); }
		}
		public MainViewModel()
        {
            SelectedVM = new AuthorisationViewModel(this);
		}

    }
}

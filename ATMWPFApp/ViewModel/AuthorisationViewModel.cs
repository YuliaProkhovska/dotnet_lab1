using ATMClassLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ATMWPFApp.ViewModel
{


	public class AuthorisationViewModel : INotifyPropertyChanged
	{
		private RelayCommand _auth;
		private MainViewModel _mainViewModel;
		private string _inputText = string.Empty;
		private string _inputPin = string.Empty;
		private HomeViewModel home;



		public ICommand AuthCommand
		{
			get
			{
				if (_auth == null)
				{
					_auth = new RelayCommand(Authorisation);
				}
				return _auth;
			}
		}
		public AuthorisationViewModel()
		{
			home = new HomeViewModel(this);
		}
		public AuthorisationViewModel(MainViewModel mainViewModel)
		{
			_mainViewModel = mainViewModel;
		}

		private void Authorisation(object parameter)
		{
			_mainViewModel.SelectedVM = new HomeViewModel((AuthorisationViewModel)_mainViewModel.SelectedVM);

		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		public string InputText
		{
			get { return _inputText; }
			set
			{
				if (_inputText != value)
				{
					_inputText = value;
					OnPropertyChanged(nameof(InputText));
					
				}
			}
		}

		public string InputPin
		{
			get { return _inputPin; }
			set
			{
				if (_inputPin != value)
				{
					_inputPin = value;
					OnPropertyChanged(nameof(InputPin));
					
				}
			}
		}
	}
}

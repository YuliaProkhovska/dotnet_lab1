using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ATMClassLib
{
	public delegate void AccountHandler(string message);


	public class Account : INotifyPropertyChanged
	{
		private string _cardNumber;
		private string _gmailAddress;
		private string _pin;
		private string _name;
		private decimal _balance;

		public delegate void PropertyChangedEventHandlerDelegate(object sender, PropertyChangedEventArgs e);

		public event PropertyChangedEventHandler PropertyChanged;

		public string Pin
		{
			get { return _pin; }
			set
			{
				if (_pin != value)
				{
					_pin = value;
					OnPropertyChanged(nameof(Pin));
				}
			}
		}

        public string GmailAddress
        {
            get { return _gmailAddress; }
            set
            {
                if (_gmailAddress != value)
                {
                    _gmailAddress = value;
                    OnPropertyChanged(nameof(GmailAddress));
                }
            }
        }
        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                if (_cardNumber != value)
                {
                    _cardNumber = value;
                    OnPropertyChanged(nameof(CardNumber));
                }
            }
        }

        public string Name
		{
			get { return _name; }
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged(nameof(Name));
				}
			}
		}

		
		public decimal Balance
		{
			get { return _balance; }
			set
			{
				if (_balance != value)
				{
					_balance = value;
					OnPropertyChanged(nameof(Balance));
				}
			}
		}

		public Account(string cardNumber, string pin)
		{
			Database database = new Database();

			CardNumber = cardNumber;
			Pin = pin;
			Name = database.GetName(CardNumber);
			GmailAddress = database.GetGmail(CardNumber);
			Balance = database.GetBalance(cardNumber, pin);
		}

		protected virtual void OnPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
	}


}

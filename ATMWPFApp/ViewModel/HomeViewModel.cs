using ATMClassLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ATMWPFApp.ViewModel
{
	public class HomeViewModel : INotifyPropertyChanged
	{
		private Account _account;

		AutomatedTellerMachine _atm;
		private Bank _Bank;

		private MainViewModel _MainViewModel;
		private RelayCommand _withdrawBalance;
		private RelayCommand _topupBalance;
		private RelayCommand _transferBalance;
		private string _inputText = string.Empty;
		private string _inputAmount = string.Empty;



		public string InputCard
		{
			get { return _inputText; }
			set
			{
				if (_inputText != value)
				{

					_inputText = value;
					OnPropertyChanged(nameof(InputCard));
				}
			}
		}
		public string InputAmount
		{
			get { return _inputAmount; }
			set
			{
				if (_inputAmount != value)
				{
					_inputAmount = value;
					OnPropertyChanged(nameof(InputCard));

				}
			}
		}


		public ICommand WithdrawBalance
		{

			get
			{
				if (_withdrawBalance == null)
				{

					_withdrawBalance = new RelayCommand(WithdrawMoney);
				}
				return _withdrawBalance;
			}
		}

		public ICommand TransferBalance
		{

			get
			{
				if (_transferBalance == null)
				{

					_transferBalance = new RelayCommand(SendMoney);
				}
				return _transferBalance;
			}
		}
		public ICommand TopUpBalance
		{

			get
			{
				if (_topupBalance == null)
				{

					_topupBalance = new RelayCommand(TopUpMoney);
				}
				return _topupBalance;
			}
		}
		private void TopUpMoney(object parameter)
		{
			Account.Balance += Convert.ToDecimal(parameter);
			ATM.MoneyAmount += Convert.ToDecimal(parameter);
			Bank bank = new Bank("Privat24");
			bank.SendMessage(parameter.ToString(), "+", Account.GmailAddress);
			MessageBox.Show($"Ви поповнили рахунок на {parameter} гривень");
			bank.SuccessfulOperation += SuccessfulOperationHandler;
		}

		private static void SuccessfulOperationHandler(object sender, SuccessfulOperationEventArgs e)
		{
			MessageBox.Show($"Операція успішна: {e.Operator}{e.Parameter}.\nВідправлено на почту: {e.Gmail}");
		}
		private void WithdrawMoney(object parameter)
		{

			if (Account != null)
			{

				try
				{
					decimal balance = Account.Balance;
					decimal Amount = Convert.ToDecimal(parameter);

					if (balance >= Amount && ATM.MoneyAmount >= Amount)
					{
						Account.Balance -= Convert.ToDecimal(parameter);
						Bank bank = new Bank("Privat24");
						bank.SendMessage(parameter.ToString(), "-", Account.GmailAddress);
						MessageBox.Show($"Ви зняли {parameter} гривень");
						bank.SuccessfulOperation += SuccessfulOperationHandler;



					}
					else if (ATM.MoneyAmount < Amount)
					{
						MessageBox.Show("В терміналі недостатньо коштів.\nОперація не виконана");
					}
					else if (Amount > balance)
					{
						MessageBox.Show("Недостатньо коштів.");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Помилка при знятті грошей: {ex.Message}");
				}
			}
			else
			{
				MessageBox.Show("Account is not initialized.");
			}
		}

		private void SendMoney(object parameter)
		{

			if (Account != null)
			{

				try
				{
					decimal balance = Account.Balance;
					decimal Amount = Convert.ToDecimal(InputAmount);
					Database database = new Database();

					if (balance >= Amount && database.IsCardValid(InputCard) && InputCard != Account.CardNumber)
					{
						Account Account2 = new Account(InputCard, "");
						Account.Balance -= Amount;

						database.UpdateBalance(InputCard, Amount);

						Bank bank = new Bank("Privat24");
						bank.SendMessage(InputAmount, "Переказ на карту\n-", Account.GmailAddress);
						bank.SendMessage(InputAmount, "Поповнення карти\n+", Account2.GmailAddress);

						bank.SuccessfulOperation += SuccessfulOperationHandler;
						MessageBox.Show($"Ви переказали {InputAmount} гривень\nНа карту {Account2.Name}");




					}
					else if (!database.IsCardValid(InputCard))
					{
						MessageBox.Show("🚫 Неправильний номер карти");
					}
					else if (InputCard == Account.CardNumber)
					{
						MessageBox.Show("🚫 Це ж ваша картка!");
					}

					else if (Amount > balance)
					{
						MessageBox.Show("Недостатньо коштів.");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Помилка при знятті грошей: {ex.Message}");
				}
			}
			else
			{
				MessageBox.Show("Account is not initialized.");
			}
		}

		public HomeViewModel()
		{
			
			_MainViewModel = new MainViewModel();
			if (_account == null)
			{
				_atm = new AutomatedTellerMachine("ATM865", 10000, "Київська 68", "YANG Corp", "ATM Model 356", "v2.0");
				_account = new Account("1111111111111111", "1111");
			}

		}


		public HomeViewModel(AuthorisationViewModel authViewModel)
		{

			
			Database database = new Database();

			if (database.IsCardValid(authViewModel.InputText))
			{
				if (!database.IsValidPin(authViewModel.InputText, authViewModel.InputPin))
				{
					MessageBox.Show("🚫 Неправильний пін карти");
					_MainViewModel.SelectedVM = new AuthorisationViewModel();
				}
				else
				{
					_Bank = new Bank("Tantanbank");
					_account = new Account(authViewModel.InputText, authViewModel.InputPin);
					_atm = new AutomatedTellerMachine("ATM865", 10000, "Київська 68", "YANG Corp", "ATM Model 356", "v2.0");

                }
			}
			else
			{
				MessageBox.Show("🚫 Неправильний номер карти");
				
			}

		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		public Account Account
		{
			get { return _account; }
			set
			{
				if (_account != value)
				{
					_account = value;
					OnPropertyChanged(nameof(Account));
				}
			}
		}



		public AutomatedTellerMachine ATM
		{
			get { return _atm; }
			set
			{
				if (_atm != value)
				{
					_atm = value;
					OnPropertyChanged(nameof(ATM));
				}
			}
		}


		public Bank Bank
		{
			get { return _Bank; }
			set
			{
				if (_Bank != value)
				{
					_Bank = value;
					OnPropertyChanged(nameof(Bank));
				}
			}
		}

	}
}

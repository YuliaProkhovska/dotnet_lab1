using ATMClassLib;
using System;
using System.Text;

namespace ATMConsoleApp
{
	class Program
	{

		static void DisplayATMInfo(AutomatedTellerMachine atm)
		{
			Console.WriteLine("\nІнформація про банкомат:");
            Console.WriteLine($"Версія програмного забезпечення банкомату: {atm.SoftwareVersion}");
            Console.WriteLine($"Виробник банкомату: {atm.Manufacturer}");
            Console.WriteLine($"Адреса банкомату: {atm.Address}");
            Console.WriteLine($"Модель банкомату: {atm.Model}");
            Console.WriteLine($"Сума грошей : {atm.MoneyAmount:C}");
            Console.WriteLine($"Ідентифікатор банкомату: {atm.ATMId}");
		}

		static void Main(string[] args)
		{
            Console.OutputEncoding = Encoding.UTF8;
            
			Console.WriteLine("Ласкаво просимо до консольного додатка для банкомату!");


			Bank bank = new Bank("MyBank");
			AutomatedTellerMachine atm = new AutomatedTellerMachine("ATM865", 10000, "Zhytomirska St", "YANG Corp", "ATM Model 356", "v2.0");
			bank.ATMs.Add(atm);

			DisplayATMInfo(atm);
			bool loggedIn = false;

			while (true)
			{
				if (!loggedIn)
				{

					Console.Write("Введіть номер своєї картки: ");
					string cardNumber = Console.ReadLine();

					Console.Write("Введіть ваш PIN: ");
					string pin = Console.ReadLine();


					if (AuthorizeUser(bank, cardNumber, pin, out Account authorizedAccount))
					{
						loggedIn = true;
						Console.WriteLine($"Авторизація пройшла успішно! Ласкаво просимо, {authorizedAccount.Name}.");
						PerformTransactions(bank, authorizedAccount);
					}
					else
					{
						Console.WriteLine("Авторизація не вдалася. Будь ласка, перевірте номер картки та PIN.");
					}
				}

				Console.WriteLine("Бажаєте увійти за іншим обліковим записом? (Y/N)");
				string response = Console.ReadLine().ToUpper();

				if (response != "Y")
				{
					Console.WriteLine("Вихід...");
					Environment.Exit(0);
				}
				else
				{
					loggedIn = false;
				}
			}
		}



		static void PerformTransactions(Bank bank, Account authorizedAccount)
		{
			while (true)
			{
				Console.WriteLine("\nОберіть опцію:");
				Console.WriteLine("1. Перевірити баланс");
				Console.WriteLine("2. Зняти гроші");
				Console.WriteLine("3. Надіслати гроші");
				Console.WriteLine("4. Вийти з акаунту");
				Console.WriteLine("5. Вийти");

				int choice;
				if (int.TryParse(Console.ReadLine(), out choice))
				{
					switch (choice)
					{
						case 1:
							Console.WriteLine($"Ваш баланс: {authorizedAccount.Balance}");
							break;

						case 2:
							Console.Write("Введіть суму для зняття: ");
							if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
							{
								int result = bank.GetDatabase().WithdrawMoney(authorizedAccount.CardNumber, authorizedAccount.Pin, withdrawAmount);
								if (result == 1)
								{
									authorizedAccount.Balance -= withdrawAmount;
									Console.WriteLine($"Успішне зняття грошей. Новий баланс: {authorizedAccount.Balance}");

									bank.SendMessage($"{withdrawAmount} UAH", "Зняття: ", authorizedAccount.GmailAddress);

									bank.SuccessfulOperation += SuccessfulOperationHandler;
								}
								else if (result == 0)
								{
									Console.WriteLine("Невірний PIN. Транзакція не виконалася.");
								}
								else
								{
									Console.WriteLine("Недостатньо коштів. Транзакція не виконалася.");
								}
							}
							else
							{
								Console.WriteLine("Невірна сума. Будь ласка, введіть дійсне число.");
							}
							break;


						case 3:
							Console.Write("Введіть номер картки отримувача: ");
							string recipientCardNumber = Console.ReadLine();

							Console.Write("Введіть суму для відправлення: ");
							if (decimal.TryParse(Console.ReadLine(), out decimal sendAmount))
							{
								Account recipientAccount;
								if (AuthorizeUser(bank, recipientCardNumber, "", out recipientAccount))
								{
									int result = bank.GetDatabase().SendMoney(authorizedAccount.CardNumber, authorizedAccount.Pin, recipientCardNumber, sendAmount);
									if (result == 1)
									{
										Console.WriteLine($"Гроші відправлені успішно. Ваш новий баланс: {authorizedAccount.Balance}");
										bank.SuccessfulOperation += SuccessfulOperationHandler;
									}
									else if (result == 0)
									{
										Console.WriteLine("Невірний PIN. Транзакція не виконалася.");
									}
									else if (result == -1)
									{
										Console.WriteLine("Картку отримувача не знайдено. Транзакція не виконалася.");
									}
									else
									{
										Console.WriteLine("Недостатньо коштів. Транзакція не виконалася.");
									}
								}
								else
								{
									Console.WriteLine("Не вдалося авторизувати отримувача. Будь ласка, перевірте коректність номера картки.");
								}
							}
							else
							{
								Console.WriteLine("Невірна сума. Будь ласка, введіть дійсне число.");
							}
							break;


						case 4:
							Console.WriteLine("Вихід з акаунту...");
							return;

						case 5:
							Console.WriteLine("Вихід...");
							Environment.Exit(0);
							break;

						default:
							Console.WriteLine("Невірна опція. Будь ласка, оберіть дійсну опцію.");
							break;
					}
				}
				else
				{
					Console.WriteLine("Невірний ввід. Будь ласка, введіть дійсне число.");
				}
			}
		}

		static bool AuthorizeUser(Bank bank, string cardNumber, out Account authorizedAccount)
		{
			return AuthorizeUser(bank, cardNumber, "", out authorizedAccount);
		}

		private static void SuccessfulOperationHandler(object sender, SuccessfulOperationEventArgs e)
		{
			Console.WriteLine($"Операція успішна: {e.Operator}{e.Parameter}. Відправлено на почту: {e.Gmail}");
		}

        static bool AuthorizeUser(Bank bank, string cardNumber, string pin, out Account authorizedAccount)
        {
            authorizedAccount = null;

            var database = bank.GetDatabase();
            if (database != null)
            {
                if (database.IsCardValid(cardNumber))
                {
                    if (pin == "" || database.IsValidPin(cardNumber, pin))
                    {
                        authorizedAccount = new Account(cardNumber, pin);
                        return true;
                    }
                }
            }

            return false;
        }







    }
}

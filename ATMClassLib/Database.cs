using System;
using System.Collections.Generic;

namespace ATMClassLib
{
	public class Database
	{
		private readonly Dictionary<string, Account> accounts;

		public delegate void TransactionDelegate(int accountId, decimal amount, string description);

		public event TransactionDelegate TransactionEvent;


		public Database()
		{
			this.accounts = new Dictionary<string, Account>();
			accounts.Add("1234561234512345", new Account("1234561234512345", "James Conrod", "1234", 1000.0m, "jamescronrod5276@gmail.com"));
			accounts.Add("9876549876598765", new Account("9876549876598765", "Nick Williams", "4321", 500.0m, "nickwilliams6498@gmail.com"));
			accounts.Add("3333333333333333", new Account("3333333333333333", "Edward Elrick", "1111", 10279.120m, "edwardelrick1876@gmail.com"));
			accounts.Add("4444444444444444", new Account("4444444444444444", "Yulia Prokhovska", "2222", 8943.217m, "ipz223_pyuv@gmail.com"));
			accounts.Add("", new Account("", "", "", 0, "ipz223_pyuv@gmail.com));

        }

		

		public int SendMoney(string senderCardNumber, string senderPin, string recipientCardNumber, decimal amount)
		{
			if (!IsValidPin(senderCardNumber, senderPin))
			{
				return 0; 
			}

			if (!IsCardValid(recipientCardNumber))
			{
				return -1;
			}

			decimal senderBalance = GetBalance(senderCardNumber, senderPin);
			if (senderBalance < amount)
			{
				return -2;
			}


			accounts[senderCardNumber].Balance -= amount;
			accounts[recipientCardNumber].Balance += amount;


			accounts[senderCardNumber].AddTransaction(-amount, $"Sent to {recipientCardNumber}");
			accounts[recipientCardNumber].AddTransaction(amount, $"Received from {senderCardNumber}");

			OnTransactionEvent(accounts[senderCardNumber].Id, -amount, $"Sent to {recipientCardNumber}");
			OnTransactionEvent(accounts[recipientCardNumber].Id, amount, $"Received from {senderCardNumber}");

			return 1; 
		}

		public decimal GetBalance(string cardNumber, string pin)
		{
			if (!IsValidPin(cardNumber, pin))
			{
				return -1;
			}

			return accounts[cardNumber].Balance;
		}
		public bool IsCardValid(string cardNumber)
		{
			return accounts.ContainsKey(cardNumber);
		}
		public string GetCard(string cardNumber)
		{
			return accounts[cardNumber].CardImage;
		}
		public int UpdateBalance(string cardNumber, decimal amount)
		{

			accounts[cardNumber].Balance += amount;

			OnTransactionEvent(accounts[cardNumber].Id, amount, "Переказ на карту");


			accounts[cardNumber].AddTransaction(amount, "Переказ на карту");

			return 1;
		}

		public int AddTransaction(string cardNumber, decimal amount, string description)
		{
			if (accounts.ContainsKey(cardNumber))
			{
				OnTransactionEvent(accounts[cardNumber].Id, amount, description);


				accounts[cardNumber].AddTransaction(amount, description);

				return 1;
			}

			return 0;
		}

		public string GetName(string cardNumber)
		{
			return accounts[cardNumber].OwnerName;
		}
		public string GetGmail(string cardNumber)
		{
			return accounts[cardNumber].Gmail;
		}

		public string GetColor(string cardNumber)
		{
			return accounts[cardNumber].Color;
		}

		public int WithdrawMoney(string cardNumber, string pin, decimal amount)
		{
			if (!IsValidPin(cardNumber, pin))
			{
				return 0;
			}


			decimal currentBalance = GetBalance(cardNumber, pin);
			if (currentBalance < amount)
			{
				return -1;
			}

			accounts[cardNumber].Balance -= amount;

			OnTransactionEvent(accounts[cardNumber].Id, -amount, "Зняття коштів");


			accounts[cardNumber].AddTransaction(-amount, "Зняття коштів");

			return 1;
		}

		public bool IsValidPin(string cardNumber, string pin)
		{
			if (accounts.ContainsKey(cardNumber))
			{
				return accounts[cardNumber].Pin == pin.Trim();
			}

			return false;
		}

		private void OnTransactionEvent(int accountId, decimal amount, string description)
		{
			TransactionEvent?.Invoke(accountId, amount, description);
		}

		private class Account
		{
			private static int nextId = 1;

			public int Id { get; }
			public string CardNumber { get; }
			public string Gmail { get; }
			public string CardImage { get; }
			public string Color { get; }
			public string OwnerName { get; }
			public string Pin { get; }
			public decimal Balance { get; set; }
			public List<Transaction> Transactions { get; }

			public Account(string cardNumber, string ownerName, string pin, decimal balance, string gmail)
			{

				Id = nextId++;
				CardNumber = cardNumber;
				Gmail = gmail;
				OwnerName = ownerName;
				Pin = pin;
				Balance = balance;
				Transactions = new List<Transaction>();
				Gmail = gmail;
			}

			public void AddTransaction(decimal amount, string description)
			{
				Transaction newTransaction = new Transaction(amount, description);
				Transactions.Add(newTransaction);
			}
		}

		private class Transaction
		{
			public decimal Amount { get; }
			public string Description { get; }
			public DateTime Timestamp { get; }

			public Transaction(decimal amount, string description)
			{
				Amount = amount;
				Description = description;
				Timestamp = DateTime.Now;
			}
		}
	}
}


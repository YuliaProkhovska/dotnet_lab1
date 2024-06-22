using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;

namespace ATMClassLib
{
	public delegate void SuccessfulOperationEventHandler(object sender, SuccessfulOperationEventArgs e);



	public class SuccessfulOperationEventArgs : EventArgs
	{
		public string Parameter { get; }
		public string Operator { get; }
		public string Gmail { get; }

		public SuccessfulOperationEventArgs(string parameter, string _operator, string gmail)
		{
			Parameter = parameter;
			Operator = _operator;
			Gmail = gmail;
		}
	}

	public class Bank
	{
		private Database _database;
		public string Name { get; set; }
        private List<AutomatedTellerMachine> atms;

		public event SuccessfulOperationEventHandler SuccessfulOperation;

		public List<AutomatedTellerMachine> ATMs
		{
			get { return atms; }
			set { atms = value; }
		}

		public Bank(string bankName)
		{
			Name = bankName;
			this.atms = new List<AutomatedTellerMachine>();
		}

		public Database GetDatabase()
		{
			return _database;
		}

		public void SendMessage(string title, string subtitle, string gmail)
		{
			string sender = "ipz223_pyuv@student.ztu.edu.ua";
			string pass = "605845";

			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(sender, pass),
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network
			};

			smtpClient.Send(sender, gmail,title, subtitle);

			OnSuccessfull(new SuccessfulOperationEventArgs(title, subtitle, gmail));
		}

		protected virtual void OnSuccessfull(SuccessfulOperationEventArgs e)
		{
			SuccessfulOperation?.Invoke(this, e);
		}
	}
}

using System;
using System.Collections.Generic;

namespace ATMClassLib
{
	public class AutomatedTellerMachine
	{

		private decimal moneyAmount;
		private string atmId;
		private string address;
		private string manufacturer;
		private string model;
		private string softwareVersion;

		public string Address
		{
			get { return address; }
			set { address = value; }
		}

        public decimal MoneyAmount
        {
            get { return moneyAmount; }
            set { moneyAmount = value; }
        }

		public string Model
		{
			get { return model; }
			set { model = value; }
		}

		public string SoftwareVersion
		{
			get { return softwareVersion; }
			set { softwareVersion = value; }
		}
        public string Manufacturer
        {
            get { return manufacturer; }
            set { manufacturer = value; }
        }
        public string ATMId
        {
            get { return atmId; }
            set { atmId = value; }
        }
        public AutomatedTellerMachine(string atmId, decimal initialMoneyAmount, string address, string manufacturer, string model, string softwareVersion)
		{
			this.atmId = atmId;
			this.moneyAmount = initialMoneyAmount;
			this.address = address;
			this.manufacturer = manufacturer;
			this.model = model;
			this.softwareVersion = softwareVersion;
		}
	}
}


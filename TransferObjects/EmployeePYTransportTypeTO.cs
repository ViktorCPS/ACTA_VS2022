using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeePYTransportTypeTO
    {
        private int _transportTypeID = -1;
		private string _name = "";
		private decimal _priceDaily = -1;

        public int TransportTypeID
		{
            get { return _transportTypeID; }
            set { _transportTypeID = value; }
		}

		public string Name
		{
			get { return _name; }
			set {_name = value; }
		}

		public decimal PriceDaily
		{
			get { return _priceDaily; }
            set { _priceDaily = value; }
		}

		public EmployeePYTransportTypeTO()
		{
		}

        public EmployeePYTransportTypeTO(EmployeePYTransportTypeTO typeTO)
		{
			this.TransportTypeID = typeTO.TransportTypeID;
			this.Name = typeTO.Name;
			this.PriceDaily = typeTO.PriceDaily;
		}
    }
}

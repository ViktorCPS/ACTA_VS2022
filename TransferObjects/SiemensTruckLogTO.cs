using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SiemensTruckLogTO
    {
        // object for IT_kamioni record
        private int _id = -1;                       // id - autoincrement primary key
        private int _logID = -1;                    // at_id from AuditTrail
        private int _emplID = -1;                   // ID of SiPass employee record which is truck or truck driver
        private string _truckRegistration = "";     // kamion_registracija - LastName of truck SiPass employee
        private string _driverFirstName = "";       // vozac_ime - FirstName of truck driver SiPass employee
        private string _driverLastName = "";        // vozac_prezime - LastName of truck driver SiPass employee
        private string _buyerName = "";             // kupac_naziv - company custom filed of truck SiPass employee
        private string _forwarderName = "";         // spediter_naziv - company custom filed of truck SiPass employee
        private string _orderForm = "";             // porudzbenica - po custom filed of truck SiPass employee
        private DateTime _registrationTime = new DateTime();    // vreme_registracije - time of registration
        private string _registrationType = "";      // tip_registracije - pass direction (IN or OUT)
        private string _registrationLocation = "";  // lokacija_registracije - Point name from truckMapping.xml of registration point
        private string _type = "";                  // type of SiPass employee record - truck or truck driver
        private int _truckID = -1;                  // ID of SiPass employee record which is truck or truck driver
        private int _driverID = -1;                 // ID of SiPass employee record which is truck or truck driver
        private bool _fromFile = false;             // is log created from AuditTrail file

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public int LogID
        {
            get { return _logID; }
            set { _logID = value; }
        }

        public int EmplID
        {
            get { return _emplID; }
            set { _emplID = value; }
        }

        public string TruckRegistration
        {
            get { return _truckRegistration; }
            set { _truckRegistration = value; }
        }

        public string DriverFirstName
        {
            get { return _driverFirstName; }
            set { _driverFirstName = value; }
        }

        public string DriverLastName
        {
            get { return _driverLastName; }
            set { _driverLastName = value; }
        }

        public string DriverName
        {
            get { return _driverLastName + " " + _driverFirstName; }            
        }

        public string BuyerName
        {
            get { return _buyerName; }
            set { _buyerName = value; }
        }

        public string ForwarderName
        {
            get { return _forwarderName; }
            set { _forwarderName = value; }
        }

        public string OrderForm
        {
            get { return _orderForm; }
            set { _orderForm = value; }
        }

        public DateTime RegistrationTime
        {
            get { return _registrationTime; }
            set { _registrationTime = value; }
        }

        public string RegistrationType
        {
            get { return _registrationType; }
            set { _registrationType = value; }
        }

        public string RegistrationLocation
        {
            get { return _registrationLocation; }
            set { _registrationLocation = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int TruckID
        {
            get { return _truckID; }
            set { _truckID = value; }
        }

        public int DriverID
        {
            get { return _driverID; }
            set { _driverID = value; }
        }

        public bool FromFile
        {
            get { return _fromFile; }
            set { _fromFile = value; }
        }

        public SiemensTruckLogTO()
        { }

        public SiemensTruckLogTO(SiemensTruckLogTO logTO)
        {
            this.BuyerName = logTO.BuyerName;
            this.DriverFirstName = logTO.DriverFirstName;
            this.DriverLastName = logTO.DriverLastName;
            this.EmplID = logTO.EmplID;
            this.ForwarderName = logTO.ForwarderName; 
            this.FromFile = logTO.FromFile;
            this.ID = logTO.ID;
            this.LogID = logTO.LogID;
            this.OrderForm = logTO.OrderForm;
            this.RegistrationLocation = logTO.RegistrationLocation;
            this.RegistrationTime = logTO.RegistrationTime;
            this.RegistrationType = logTO.RegistrationType;
            this.TruckRegistration = logTO.TruckRegistration;
            this.Type = logTO.Type;
            this.TruckID = logTO.TruckID;
            this.DriverID = logTO.DriverID;
            this.FromFile = logTO.FromFile;
        }
    }
}

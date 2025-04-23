using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SiemensEmployeeTO
    {
        // object for IT_HRData record + SiPass employee record
        private uint _pk = 0;                   // PK - autoincrement primary key
        private string _id = "";                // ID - employee number
        private string _firstName = "";         // FirstName - employee first name (Breza + Siemens)
        private string _lastName = "";          // LastName - employee last name (Breza + Siemens)
        private int _departmentID = -1;         // IDDept - departmetnt ID (Odid from IT_DPData)and SiPass workgroupID
        private string _departmentIDOld = "";   // IDDeptOld
        private string _status = "";            // Status
        private string _idOld = "";             // IDOld
        private int _readStatus = -1;           // Read Status
        private string _typeOfCh = "";          // TypeOfCh - type of change: N-new record, C-change record, D-delete record
        private DateTime _timeSc = new DateTime();  // TimeSc
        private string _jmbg = "";              // JMBG - visitors JMBG from SiPass
        private int _cardHolderID = -1;         // CardHolderID - ID of SiPass cardholder for this employee
        private int _emplID = -1;               // ID of employee in SiPass
        private bool _visitor = false;          // is SiPass employee visitor or not
        private string _cardNumber = "";        // SiPass card number
        private string _cardNumberVisitor = "";        // SiPass card number visitor
        private string _cardStatus = "";        // SiPass card status (RETIRED if card is void, ACTIVE if card is not void)
        private string _cardType = "";          // SiPass type of visitor (IZVODJAC, KAMION, VOZAC KAMIONA, SLUZBENO VOZILO ...)
        private string _po = "";                // SiPass visitor custom field porudzbenica
        private string _company = "";           // SiPass visitor custom fieled company (kupac naziv, spediter naziv)
        private string _vechicleCategory = "";  // SiPass visitor custom field visitorVrstaVozila
        private string _vechicleBrand = "";     // SiPass visitor custom field visitorMarkaVozila
        private string _vechicleType = "";      // SiPass visitor custom field visitorTipVozila
        private string _vechicleColor = "";     // SiPass visitor custom field visitorBojaVozila
        private string _permition = "";         // SiPass visitor custom field visitorPropusnica
        private string _createdBy = "";         // created_by in employee_data
        private DateTime _createdTime = new DateTime(); // created_time in employee_data
        private string _modifiedBy = "";        // modified_by in employee_data
        private DateTime _modifiedTime = new DateTime(); // modified_time in employee_data
        
        public uint PK
        {
            get { return _pk; }
            set { _pk = value; }
        }

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }       

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string IDOld
        {
            get { return _idOld; }
            set { _idOld = value; }
        }

        public int DepartmentID
        {
            get { return _departmentID; }
            set { _departmentID = value; }
        }

        public string DepartmentIDOld
        {
            get { return _departmentIDOld; }
            set { _departmentIDOld = value; }
        }

        public string TypeOfCh
        {
            get { return _typeOfCh; }
            set { _typeOfCh = value; }
        }

        public int ReadStatus
        {
            get { return _readStatus; }
            set { _readStatus = value; }
        }

        public DateTime TimeSc
        {
            get { return _timeSc; }
            set { _timeSc = value; }
        }

        public string JMBG
        {
            get { return _jmbg; }
            set { _jmbg = value; }
        }

        public int CardHolderID
        {
            get { return _cardHolderID; }
            set { _cardHolderID = value; }
        }

        public string FirstAndLastName
        {
            get { return _lastName + " " + _firstName; }
        }

        public string IDLastNameFirstName
        {
            get { return ((_id.Trim() != "" ? _id.Trim() + " - " : "") + _lastName + " " + _firstName).Trim(); }
        }

        public string FirstNameLastNameID
        {
            get { return (_firstName + " " + _lastName + (_id.Trim() != "" ? " - " + _id.Trim() : "")).Trim(); }
        }

        public string LastNameFirstNameID
        {
            get { return (_lastName + " " + _firstName + (_id.Trim() != "" ? " - " + _id.Trim() : "")).Trim(); }
        }

        public int EmplID
        {
            get { return _emplID; }
            set { _emplID = value; }
        }

        public bool Visitor
        {
            get { return _visitor; }
            set { _visitor = value; }
        }

        public string CardNumber
        {
            get { return _cardNumber; }
            set { _cardNumber = value; }
        }

        public string CardNumberVisitor
        {
            get { return _cardNumberVisitor; }
            set { _cardNumberVisitor = value; }
        }
        
        public string CardStatus
        {
            get { return _cardStatus; }
            set { _cardStatus = value; }
        }

        public string CardType
        {
            get { return _cardType; }
            set { _cardType = value; }
        }

        public string Po
        {
            get { return _po; }
            set { _po = value; }
        }

        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public string VechicleCategory
        {
            get { return _vechicleCategory; }
            set { _vechicleCategory = value; }
        }

        public string VechicleBrand
        {
            get { return _vechicleBrand; }
            set { _vechicleBrand = value; }
        }

        public string VechicleType
        {
            get { return _vechicleType; }
            set { _vechicleType = value; }
        }

        public string VechicleColor
        {
            get { return _vechicleColor; }
            set { _vechicleColor = value; }
        }

        public string Permition
        {
            get { return _permition; }
            set { _permition = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }
        
        public SiemensEmployeeTO()
        { }

        public SiemensEmployeeTO(SiemensEmployeeTO emplTO)
        {
            this.PK = emplTO.PK;
            this.DepartmentID = emplTO.DepartmentID;
            this.DepartmentIDOld = emplTO.DepartmentIDOld;
            this.FirstName = emplTO.FirstName;
            this.ID = emplTO.ID;
            this.IDOld = emplTO.IDOld;
            this.LastName = emplTO.LastName;
            this.ReadStatus = emplTO.ReadStatus;
            this.Status = emplTO.Status;
            this.TimeSc = emplTO.TimeSc;
            this.TypeOfCh = emplTO.TypeOfCh;
            this.JMBG = emplTO.JMBG;
            this.EmplID = emplTO.EmplID;
            this.Visitor = emplTO.Visitor;
            this.CardNumber = emplTO.CardNumber;
            this.CardStatus = emplTO.CardStatus;
            this.CardType = emplTO.CardType;
            this.Company = emplTO.Company;
            this.Po = emplTO.Po;
            this.VechicleCategory = emplTO.VechicleCategory;
            this.VechicleColor = emplTO.VechicleColor;
            this.VechicleType = emplTO.VechicleType;
            this.VechicleBrand = emplTO.VechicleBrand;
            this.Permition = emplTO.Permition;
            this.CardNumberVisitor = emplTO.CardNumberVisitor;
            this.CreatedBy = emplTO.CreatedBy;
            this.CreatedTime = emplTO.CreatedTime;
            this.ModifiedBy = emplTO.ModifiedBy;
            this.ModifiedTime = emplTO.ModifiedTime;
        }
    }
}

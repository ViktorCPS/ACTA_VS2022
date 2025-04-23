using System;

using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using TransferObjects;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for ApplUsersTO.
	/// </summary>
	/// [XmlRootAttribute()]
    /// 

    // [DataContract] & [DataMemeber] attributes from System.Runtime.Serialization put here for WCF service in ACTAWeb solution purposes only
    [DataContract]
	public class ApplUserTO
	{
		private string _userID = "";
		private string _password = "";
		private string _name = "";
		private string _description = "";
		private int _privilegeLvl = -1;
		private string _status = "";
		private int _numOfTries = -1;
		private string _langCode = "";
		private ApplUserLogTO _applUserLog = new ApplUserLogTO();
		private int _exitPermissionVerificationLvl = -1;
        private int _extraHoursAdvancedAmt = -1;
        private string _createdBy = "";
        private string _modifiedBy = "";

        public int ExtraHoursAdvancedAmt
        {
            get { return _extraHoursAdvancedAmt; }
            set { _extraHoursAdvancedAmt = value; }
        }

        [DataMember]
		public string UserID
		{
			get{ return _userID; }
			set{ _userID = value; }
		}

        [DataMember]
		public string Password
		{
			get { return _password; }
			set {_password = value; }
		}

        [DataMember]
		public string Name
		{
			get { return _name; }
			set {_name = value; }
		}

        [DataMember]
		public string Description
		{
			get { return _description; }
			set {_description = value; }
		}

        [DataMember]
		public int PrivilegeLvl
		{
			get { return _privilegeLvl; }
			set {_privilegeLvl = value; }
		}

        [DataMember]
		public string Status
		{
			get { return _status; }
			set {_status = value; }
		}

        [DataMember]
		public int NumOfTries
		{
			get { return _numOfTries; }
			set {_numOfTries = value; }
		}

        [DataMember]
		public string LangCode
		{
			get { return _langCode; }
			set {_langCode = value; }
		}

		public ApplUserLogTO LogTO
		{
			get{ return _applUserLog; }
			set{ _applUserLog = value; }
		}

        [DataMember]
		public int ExitPermVerification
		{
			get { return _exitPermissionVerificationLvl; }
			set {_exitPermissionVerificationLvl = value; }
		}

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

		public ApplUserTO()
		{
		}

        public ApplUserTO(string userID, string password, string name, string description, int privilegeLvl,
            string status, int numOfTries, string langCode)
        {
            this.UserID = userID;
            this.Password = password;
            this.Name = name;
            this.Description = description;
            this.PrivilegeLvl = privilegeLvl;
            this.Status = status;
            this.NumOfTries = numOfTries;
            this.LangCode = langCode;
        }
	}
}

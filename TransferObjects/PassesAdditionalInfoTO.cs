using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class PassesAdditionalInfoTO
    {
        private uint _pass_id = 0;
        private string _gps_data = "";
        private string _cardholder_name = "";
        private int _cardholder_id = -1;
        private string _createdBy = "";
        private DateTime _created_time = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modified_time = new DateTime();


        public uint PassID
        {
            get { return _pass_id; }
            set { _pass_id = value; }
        }
        public string GpsData
        {
            get { return _gps_data; }
            set { _gps_data = value; }
        }
        public string CardholderName
        {
            get { return _cardholder_name; }
            set { _cardholder_name = value; }
        }
        public int CardholderID
        {
            get { return _cardholder_id; }
            set { _cardholder_id = value; }
        }
        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }
        public DateTime CreatedTime
        {
            get { return _created_time; }
            set { _created_time = value; }
        }
        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }
        public DateTime ModifiedTime
        {
            get { return _modified_time; }
            set { _modified_time = value; }
        }

        public PassesAdditionalInfoTO()
        {
        }

        public PassesAdditionalInfoTO (uint pass_id, string gps_data, string cardholder_name, int cardholder_id, string created_by, DateTime created_time, string modified_by, DateTime modified_time)
        {
            PassID = pass_id;
            GpsData = gps_data;
            CardholderID = cardholder_id;
            CardholderName = cardholder_name;
            CreatedBy = created_by;
            CreatedTime = created_time;
            ModifiedBy = modified_by;
            ModifiedTime = modified_time;

        }

    }


}

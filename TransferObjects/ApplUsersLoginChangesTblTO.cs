using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class ApplUsersLoginChangesTblTO
    {
        private int _recID = -1;
        private string _tableName = "";

        public int RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public ApplUsersLoginChangesTblTO()
        { }

        public ApplUsersLoginChangesTblTO(int recID, string tableName)
        {
            this.RecID = recID;
            this.TableName = tableName;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class PassTypesConfirmationTO
    {
        private int _passTypeID = -1;
        private int _confirmationPassTypeID = -1;

        public int ConfirmationPassTypeID
        {
            get { return _confirmationPassTypeID; }
            set { _confirmationPassTypeID = value; }
        }

        public int PassTypeID
        {
            get { return _passTypeID; }
            set { _passTypeID = value; }
        }

    }
}

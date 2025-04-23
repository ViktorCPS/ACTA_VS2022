using System;
using System.Collections;
using System.Text;

using TransferObjects;
using Util;


namespace Common
{
   public class MapsObjectDtl
    {
        private int _objectID;
        private string _type;
        private int _pointOrder;
        private double _X;
        private double _Y;

        DebugLog log;

        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        public double X
        {
            get { return _X; }
            set { _X = value; }
        }

        public int PointOrder
        {
            get { return _pointOrder; }
            set { _pointOrder = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }

        public MapsObjectDtl()
        { 
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            // Init properties
            ObjectID = -1;
            Type = "";
            PointOrder = -1;
            X = -1;
            Y = -1;
        }

        public void ReceiveTransferObject(MapsObjectDtlTO mapsObjectTO)
        {
            try
            {
                this.ObjectID = mapsObjectTO.ObjectID;
                this.Type = mapsObjectTO.Type;
                this.PointOrder = mapsObjectTO.PointOrder;
                this.X = mapsObjectTO.X;
                this.Y = mapsObjectTO.Y;               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapsObjectDtl.ReceiveTransferObject(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public MapsObjectDtlTO SendTransferObject()
        {
            MapsObjectDtlTO mapsObjectTO = new MapsObjectDtlTO();

            try
            {
                mapsObjectTO.ObjectID = this.ObjectID;
                mapsObjectTO.Type = this.Type;
                mapsObjectTO.PointOrder = this.PointOrder;
                mapsObjectTO.X = this.X;
                mapsObjectTO.Y = this.Y;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapsObjectDtl.SendTransfereObject(): " + ex.Message + "\n");
                throw ex;
            }

            return mapsObjectTO;
        }
    }
}

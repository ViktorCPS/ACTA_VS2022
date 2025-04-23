using System;
using System.Collections.Generic;
using System.Text;

namespace ReaderRemoteManagement
{
    public class AntennaCommunication
    {

        public enum TicketTransactionState { UNDEFINED, STARTED, FINISHED };
        //communication message state
        public State state;

        public int AntennaNum = 0;

        //transaction start time in ticks
        public long ticketTransactionStartTime = 0;

        //time of last pass
        public long lastTicketTime = 0;

        //current ticket shown on the reader
        public uint tagID = 0;

        //current employee_id
        public int employeeID = -1;

        //current transID
        public int transID = -1;

        //current mealTypeID
        public int mealTypeID = -1;

        //transaction state
        public TicketTransactionState ticketTransactionState = TicketTransactionState.UNDEFINED;

        public void StartTicketTransaction()
        {
            ticketTransactionStartTime = DateTime.Now.Ticks;
            ticketTransactionState = TicketTransactionState.STARTED;
        }

        public void FinishTicketTransaction()
        {
            tagID = 0;
            ticketTransactionState = TicketTransactionState.FINISHED;
        }

    }
}

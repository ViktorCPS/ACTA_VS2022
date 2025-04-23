using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class ReaderControlMonitorSyncFactory
    {
        private enum SyncType 
        {
            DB,
            File
        };

        private static SyncType syncType;

        static ReaderControlMonitorSyncFactory()
        {
            if (new ReaderControlMonitorDBSync("-1").IsDBSyncPossible())
            {
                syncType = SyncType.DB;
            }
            else
            {
                syncType = SyncType.File;
            }
        }

        public static ReaderControlMonitorSync GetReaderControlMonitorSync(string gate)
        {
            if (syncType == SyncType.DB)
            {
                return new ReaderControlMonitorDBSync(gate);
            }
            else
            {
                return new ReaderControlMonitorFileSync(gate);
            }
        }

        public static ReaderControlMonitorSync GetReaderControlMonitorSync(string gate, Object dbConnection)
        {
            if (syncType == SyncType.DB)
            {
                return new ReaderControlMonitorDBSync(gate, dbConnection);
            }
            else
            {
                return new ReaderControlMonitorFileSync(gate);
            }
        }

        public static void TestGateSyncDAO()
        {
            ReaderControlMonitorDBSync rcmDBSync0 = new ReaderControlMonitorDBSync("0");
            rcmDBSync0.CreateGateSync();

            ReaderControlMonitorDBSync rcmDBSync1 = new ReaderControlMonitorDBSync("1");
            rcmDBSync1.CreateGateSync();
            rcmDBSync1.WriteGateSync(new ReaderControlMonitorSync("YES", "NO"));

            ReaderControlMonitorSync rcms0 = rcmDBSync0.ReadGateSync();
            ReaderControlMonitorSync rcms1 = rcmDBSync1.ReadGateSync();

            bool cs0 = rcmDBSync0.CanSynchronize();
            bool cs1 = rcmDBSync1.CanSynchronize();

            rcmDBSync0.DeleteGateSync();
            rcmDBSync1.DeleteGateSync();

            cs0 = rcmDBSync0.CanSynchronize();
            cs1 = rcmDBSync1.CanSynchronize();
        }
    }
}

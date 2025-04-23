using System;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;

using Common;
using TransferObjects;

namespace ReaderControlService
{
    [DataContract(Namespace = "http://ReaderControlService")]
    public class ReaderInfo
    {
        public ReaderInfo(ReaderTO reader)
        {
            ReaderID = reader.ReaderID;
            Description = reader.Description;
            A0GateID = reader.A0GateID;
            A1GateID = reader.A1GateID;
            DownloadInterval = reader.DownloadInterval;
            DownloadStartTime = reader.DownloadStartTime;
        }

        [DataMember(Name = "ReaderID")]
        public int ReaderID;
        [DataMember(Name = "Description")]
        public string Description;
        [DataMember(Name = "A0GateID")]
        public int A0GateID;
        [DataMember(Name = "A1GateID")]
        public int A1GateID;
        [DataMember(Name = "DownloadInterval")]
        public int DownloadInterval;
        [DataMember(Name = "DownloadStartTime")]
        public DateTime DownloadStartTime;
    }

    [DataContract(Namespace = "http://ReaderControlService")]
    public class ReadersInfos
    {
        public ReadersInfos(List<ReaderTO> readers)
        {
            InfosData = new List<ReaderInfo>();

            foreach (ReaderTO reader in readers)
            {
                InfosData.Add(new ReaderInfo(reader));
            }
        }

        [DataMember(Name = "InfosData")]
        public List<ReaderInfo> InfosData;
    }

    [DataContract(Namespace = "http://ReaderControlService")]
    public class ReaderStatus
    {
        public ReaderStatus(int readerID)
        {
            ReaderID = readerID;
            IsDownloading = false;
            Action = "";
            NextTimeDownload = DateTime.Now;
            MemoryOccupation = 0;
            IsNetExist = false;
            IsDataExist = false;
            IsReaderActionDone = false;
            IsReaderAlertDone = false;
            IsPingAlertDone = false;
        }

        [DataMember(Name = "ReaderID")]
        public int ReaderID;
        [DataMember(Name = "IsDownloading")]
        public bool IsDownloading;
        [DataMember(Name = "Action")]
        public string Action;
        [DataMember(Name = "NextTimeDownload")]
        public DateTime NextTimeDownload;
        [DataMember(Name = "MemoryOccupation")]
        public int MemoryOccupation;
        [DataMember(Name = "IsNetExist")]
        public bool IsNetExist;
        [DataMember(Name = "IsDataExist")]
        public bool IsDataExist;
        [DataMember(Name = "IsReaderActionDone")]
        public bool IsReaderActionDone;
        [DataMember(Name = "IsReaderAlertDone")]
        public bool IsReaderAlertDone;
        [DataMember(Name = "IsPingAlertDone")]
        public bool IsPingAlertDone;
    }

    [DataContract(Namespace = "http://ReaderControlService")]
    public class ReadersStatuses
    {
        public ReadersStatuses(List<ReaderTO> readers)
        {
            StatusesData = new Dictionary<int, ReaderStatus>();

            foreach (ReaderTO reader in readers)
            {
                StatusesData.Add(reader.ReaderID, new ReaderStatus(reader.ReaderID));
            }

            ActionReaderID = 0;
            AlertReaderID = 0;
        }

        [DataMember(Name = "StatusesData")]
        public Dictionary<int, ReaderStatus> StatusesData;

        [DataMember(Name = "ActionReaderID")]
        public int ActionReaderID;

        [DataMember(Name = "AlertReaderID")]
        public int AlertReaderID;
    }
}

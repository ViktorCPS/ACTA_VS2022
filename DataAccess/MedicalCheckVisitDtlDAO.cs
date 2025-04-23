using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface MedicalCheckVisitDtlDAO
    {
        int insert(MedicalCheckVisitDtlTO dtlTO, bool doCommit);

        bool update(MedicalCheckVisitDtlTO dtlTO, bool doCommit);

        bool delete(string recIDs, bool doCommit);

        List<MedicalCheckVisitDtlTO> getMedicalCheckVisitDetails(MedicalCheckVisitDtlTO dtlTO);

        List<MedicalCheckVisitDtlTO> getMedicalCheckVisitDetails(string recIDs);        

        List<MedicalCheckVisitDtlTO> getPerformedVisits(string emplIDs, string type, DateTime fromDate, DateTime toDate);

        Dictionary<int, Dictionary<string, List<int>>> getScheduledVisits(string emplIDs, string type);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}

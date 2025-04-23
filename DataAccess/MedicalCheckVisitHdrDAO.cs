using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface MedicalCheckVisitHdrDAO
    {
        bool insert(MedicalCheckVisitHdrTO hdrTO, bool doCommit);

        bool update(MedicalCheckVisitHdrTO hdrTO, bool doCommit);

        bool delete(string visitID, bool doCommit);

        bool deleteEmptyVisits(string visitIDs, bool doCommit);

        List<MedicalCheckVisitHdrTO> getEmptyVisits(string visitIDs);

        List<MedicalCheckVisitHdrTO> getMedicalCheckVisitHeaders(MedicalCheckVisitHdrTO hdrTO);

        List<MedicalCheckVisitHdrTO> getMedicalCheckVisits(string visitIDs);

        Dictionary<uint, MedicalCheckVisitHdrTO> getMedicalCheckVisits(string emplIDs, string status, string point, string check, string type, DateTime from, DateTime to);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}

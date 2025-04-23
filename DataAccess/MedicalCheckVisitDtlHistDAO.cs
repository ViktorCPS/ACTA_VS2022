using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface MedicalCheckVisitDtlHistDAO
    {
        int insert(MedicalCheckVisitDtlHistTO dtlHistTO, bool doCommit);

        bool update(MedicalCheckVisitDtlHistTO dtlHistTO, bool doCommit);

        bool delete(uint recIDHist, bool doCommit);

        List<MedicalCheckVisitDtlHistTO> getMedicalCheckVisitDetailsHistory(MedicalCheckVisitDtlHistTO dtlHistTO);

        DataTable getMedicalCheckVisitDetailsHistory(string recIDs);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}

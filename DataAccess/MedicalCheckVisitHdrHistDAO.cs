using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface MedicalCheckVisitHdrHistDAO
    {
        bool insert(MedicalCheckVisitHdrHistTO hdrHistTO, bool doCommit);

        bool update(MedicalCheckVisitHdrHistTO hdrHistTO, bool doCommit);

        bool delete(uint recID, bool doCommit);

        List<MedicalCheckVisitHdrHistTO> getMedicalCheckVisitHeadersHistory(MedicalCheckVisitHdrHistTO hdrHistTO);

        DataTable getMedicalCheckVisitHeadersHistory(uint visitID);

        List<uint> getMedicalCheckVisitHeadersHistory(string visitIDs);        

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}

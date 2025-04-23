using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface MedicalCheckDisabilityDAO
    {
        int insert(MedicalCheckDisabilityTO disabilityTO, bool doCommit);

        bool update(MedicalCheckDisabilityTO disabilityTO, bool doCommit);

        bool delete(int disabilityID, bool doCommit);

        List<MedicalCheckDisabilityTO> getMedicalCheckDisabilities(MedicalCheckDisabilityTO disabilityTO);

        Dictionary<int, MedicalCheckDisabilityTO> getMedicalCheckDisabilitiesDictionary(MedicalCheckDisabilityTO disabilityTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}

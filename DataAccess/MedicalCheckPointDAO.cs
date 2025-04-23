using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface MedicalCheckPointDAO
    {
        int insert(MedicalCheckPointTO pointTO, bool doCommit);

        bool update(MedicalCheckPointTO pointTO, bool doCommit);

        bool delete(int pointID, bool doCommit);

        List<MedicalCheckPointTO> getMedicalCheckPoints(MedicalCheckPointTO pointTO);

        Dictionary<int, MedicalCheckPointTO> getMedicalCheckPointsDictionary(MedicalCheckPointTO pointTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}

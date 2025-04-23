using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;
using System.Data;

namespace DataAccess
{
    /// <summary>
    /// PassTypesDAO interface is implemented by 
    /// database specific PassTypes DAO classes
    /// </summary>
    public interface PassTypeDAO
    {
        int insert(PassTypeTO ptTO);

        bool delete(int passTypeID);

        bool update(PassTypeTO ptTO, int oldButton);

        PassTypeTO find(int passTypeID);
        
        Dictionary<int,PassTypeTO> find(string passTypeID,int company);

        int findMAXPassTypeID();

        List<PassTypeTO> getPassTypes(PassTypeTO ptTO);

        List<PassTypeTO> getPassTypes(PassTypeTO ptTO, List<int> isPass);

        Dictionary<int, PassTypeTO> findByPaymentCode(string payment_code, int company);

        List<PassTypeTO> getPassTypesForCompany(int company, bool isAlternativeLang);

        List<PassTypeTO> getPassTypesMassiveInputForCompany(int company, string ptIDs, bool isAlternativeLang);

        void serialize(List<PassTypeTO> ptList);

        Dictionary<int, PassTypeTO> getPassTypesDictionary(PassTypeTO passTypeTO);

        Dictionary<int, PassTypeTO> getPassTypesDictionaryCodeSorted(PassTypeTO passTypeTO);

        Dictionary<int, PassTypeTO> getPassTypesForCompanyDictionary(int company, bool isAlternativeLang);

        List<string> getPassTypesDistinctField(string field);

        List<PassTypeTO> getConformationTypes(int ptID, string ptIDs, bool isAltLang);

        int insert(PassTypeTO passTypeTO, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        bool update(PassTypeTO ptTO, int oldButton, bool doCommit);

        bool delete(int passTypeID, bool doCommit);

        Dictionary<int, PassTypeTO> getPassTypesDictionary(PassTypeTO passTypeTO, IDbTransaction trans);
    }
}

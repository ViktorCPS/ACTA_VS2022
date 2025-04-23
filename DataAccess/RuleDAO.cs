using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface RuleDAO
    {
        int insert(RuleTO rule, bool doCommit);

        bool update(RuleTO rule, bool doCommit);

        bool update(int cmpany, string type, int value, bool doCommit);

        bool delete(int recID);

        List<RuleTO> search(RuleTO rule);

        List<int> getRules(string type);

        List<int> getRulesExact(string type);

        List<string> getRuleTypes();

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> SearchWUEmplTypeDictionary();

        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> getTypeAllRules(string type);

        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> SearchWUEmplTypeDictionary(IDbTransaction trans);

        int searchReaderForRestaurant(string ruleType);
    }
}

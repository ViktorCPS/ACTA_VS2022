using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public interface ApplUsersLoginChangesTblDAO
    {
        List<string> getTablesNames();

        List<string> getAllTablesNames();

        int insert(List<string> listToSaveToDB);

        bool delete();

        int insert(string table);
    }
}

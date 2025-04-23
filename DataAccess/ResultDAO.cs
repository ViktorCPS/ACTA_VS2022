using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface ResultDAO
    {
        DataTable getResultTable(string fields, string tables, string filter, string sortCol, string sortDir, int firstRow, int lastRow);

        int getResultCount(string tables, string filter);

        DataTable getResultTable(string fields, string tables, string filter, string sortCol, string sortDir);
    }
}

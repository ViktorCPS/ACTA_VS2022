using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class Result
    {
        DAOFactory daoFactory = null;
        ResultDAO dao = null;

        public Result()
		{
			daoFactory = DAOFactory.getDAOFactory();
			dao = daoFactory.getResultDAO(null);			
		}
        public Result(object dbConnection)
        {
            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getResultDAO(dbConnection);
        }

        public DataTable SearchResultTable(string fields, string tables, string filter, string sortCol, string sortDir, int firstRow, int lastRow)
        {
            try
            {
                return dao.getResultTable(fields, tables, filter, sortCol, sortDir, firstRow, lastRow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SearchResultTable(string fields, string tables, string filter, string sortCol, string sortDir)
        {
            try
            {
                return dao.getResultTable(fields, tables, filter, sortCol, sortDir);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SearchResultCount(string tables, string filter)
        {
            try
            {
                return dao.getResultCount(tables, filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}

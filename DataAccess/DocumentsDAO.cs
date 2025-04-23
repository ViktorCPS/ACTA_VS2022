using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// DocumentsDAO interface is implemented by 
    /// database specific PassTypes DAO classes
    /// </summary>
    public interface DocumentsDAO
    {

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        void SetDBConnection(Object dbConnection);

        int insert(int documentID, String firstName, String lastName, String documentName,
            String documentDesc, byte[] document, String extension, bool doCommit);

        List<DocumentsTO> getDocuments(DocumentsTO docTO);

        ArrayList getDocumentsArray(string documentsID);
    }
}

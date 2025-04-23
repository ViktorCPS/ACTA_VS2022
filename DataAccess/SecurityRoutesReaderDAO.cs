using System;
using System.Collections;
using System.Text;

namespace DataAccess
{
    public interface SecurityRoutesReaderDAO
    {
        int insert(string name, string description);

        bool update(int readerID, string name, string description);

        ArrayList getReaders(int readerID, string name, string desc);

        bool delete(int readerID);
    }
}

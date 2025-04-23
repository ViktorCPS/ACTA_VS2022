using System;
using System.Collections;
using System.Text;

namespace DataAccess
{
    public interface SecurityRoutesPointDAO
    {
        int insert(int controlPointID, string name, string description, string tagID);

        bool update(int controlPointID, string name, string description, string tagID);

        ArrayList getPoints(int pointID, string name, string desc, string tagID);

        bool delete(int controlPointID);

        int getMaxID();
    }
}

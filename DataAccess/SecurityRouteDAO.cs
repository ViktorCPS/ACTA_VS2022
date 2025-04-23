using System;
using System.Collections;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface SecurityRouteDAO
    {
        int insert(SecurityRouteHdrTO secRouteTO);

        bool delete(int securityRouteID);

        ArrayList getRoutes(string name, string desc);

        ArrayList getRoutesDetailsTag(int securityRouteID);

        ArrayList getRoutesDetailsTerminal(int securityRouteID);
    }
}

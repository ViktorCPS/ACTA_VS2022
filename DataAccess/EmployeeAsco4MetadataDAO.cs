using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeAsco4MetadataDAO
    {
        Dictionary<string, string> getEmployeeAsco4MetadataValues(string lang);

        Dictionary<string, string> getEmployeeAsco4MetadataWebValues(string lang);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeXMachineTO
    {
        public MachineTO MachineTO { get; set; }
        public EmployeeTO EmployeeTO { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;
namespace DataAccess
{
    public interface ACSEventsDAO
    {

        List<ACSEventTO> findAll();
    }
}

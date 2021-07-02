using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Enums
{
    public enum StatusType
    {
        
        X,                              // 0
        UnknownStatus,                  // 1
        NewProject,                     // 2
        DataConfirmationInProgres,      // 3
        ReadyForAcceptance,             // 4
        ProjectIsAccepted,              // 5
        ProjectIsRefused                // 6
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Enums
{
    public enum StatusType
    {
        
        X,                              // 0
        NewProject,                     // 1
        DataConfirmationInProgres,     // 2
        RequestIsConfirmed,            //  3
        ProjectIsConfirmed,            //  4
        ProjectIsAccepted,             //  6
        ProjectIsRefused               //  7
    }
}

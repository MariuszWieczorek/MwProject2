﻿using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class TechnicalPropertiesViewModel
    {
        public IEnumerable<TechnicalProperty> TechnicalProperties { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}

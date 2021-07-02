﻿using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectGroupViewModel
    {
        public string Heading { get; set; }
        public ProjectGroup ProjectGroup { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}

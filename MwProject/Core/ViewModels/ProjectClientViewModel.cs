﻿using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectClientViewModel
    {
        public string Heading { get; set; }
        public ProjectClient ProjectClient { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}

﻿using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public ProjectsFilter ProjectsFilter { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
﻿using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface IProjectStatusRepository
    {
        IEnumerable<ProjectStatus> GetProjectStatuses();
        void AddProjectStatus(ProjectStatus projectStatus);
        ProjectStatus GetProjectStatus(int id);
        void UpdateProjectStatus(ProjectStatus projectStatus);
        void DeleteProjectStatus(int id);

    }
}

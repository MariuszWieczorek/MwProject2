using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface IRequirementRepository
    {
        IEnumerable<Requirement> GetRequirements();
        void AddRequirement(Requirement requirement);
        Requirement GetRequirement(int id);
        void UpdateRequirement(Requirement requirement);
        void DeleteRequirement(int id);
    }
}

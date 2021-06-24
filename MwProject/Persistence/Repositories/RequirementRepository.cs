using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Repositories
{
    public class RequirementRepository : IRequirementRepository
    {
        
        private readonly IApplicationDbContext _context;
        public RequirementRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void AddRequirement(Requirement requirement)
        {
            _context.Requirements.Add(requirement);
        }

        public void DeleteRequirement(int id)
        {
            var requirementToDelete = _context.Requirements.Single(x => x.Id == id);
            _context.Requirements.Remove(requirementToDelete);
        }

        public Requirement GetRequirement(int id)
        {
            return _context.Requirements
                .Single(x => x.Id == id);
        }

        public Requirement NewRequirement()
        {
            return new Requirement
            {
                OrdinalNumber = _context.Requirements.Max(x=>x.OrdinalNumber)+1
            };
        }

        public IEnumerable<Requirement> GetRequirements()
        {
            return _context.Requirements
                .OrderBy(x=>x.OrdinalNumber).ThenBy(x=>x.Name).ToList();
        }

        public void UpdateRequirement(Requirement requirement)
        {
            var requirementToUpdate = _context.Requirements.Single(x => x.Id == requirement.Id);
            requirementToUpdate.Name = requirement.Name;
            requirementToUpdate.Type = requirement.Type;
            requirementToUpdate.OrdinalNumber = requirement.OrdinalNumber;
        }
    }
}

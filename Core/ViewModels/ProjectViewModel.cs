using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class ProjectViewModel
    {
        public string Heading { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ProductGroup> ProductGroups { get; set; }
        public Project Project { get; set; }
        public ApplicationUser AcceptedBy { get; set; }
        public ApplicationUser ConfirmedBy { get; set; }
        public ApplicationUser CalculationConfirmedBy { get; set; }
        public ApplicationUser EstimatedSalesConfirmedBy { get; set; }
        public ApplicationUser QualityRequirementsConfirmedBy { get; set; }
        public ApplicationUser EconomicRequirementsConfirmedBy { get; set; }
        public ApplicationUser TechnicalPropertiesConfirmedBy { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        
        // 0 - wszystkie dla okna projektu
        // 1 - ekonomiczne, 2 - jakościowe - dla osobnego okna
        public int TypeOfRequirement { get; set; }
    }
}

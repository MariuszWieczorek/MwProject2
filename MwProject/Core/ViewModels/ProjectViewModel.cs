﻿using MwProject.Core.Models.Domains;
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
        public IEnumerable<RankingCategory> RankingCategories { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public IEnumerable<ProjectStatus> ProjectStatuses { get; set; }
        public IEnumerable<ProjectGroup> ProjectGroups { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
        public IEnumerable<ProjectRequirement> ProjectRequirements { get; set; }
        public IEnumerable<ProjectTechnicalProperty> ProjectTechnicalProperties { get; set; }
        public Project Project { get; set; }
        public ApplicationUser AcceptedBy { get; set; }
        public ApplicationUser ConfirmedBy { get; set; }
        public ApplicationUser CalculationConfirmedBy { get; set; }
        public ApplicationUser EstimatedSalesConfirmedBy { get; set; }
        public ApplicationUser GeneralRequirementsConfirmedBy { get; set; }
        public ApplicationUser QualityRequirementsConfirmedBy { get; set; }
        public ApplicationUser EconomicRequirementsConfirmedBy { get; set; }
        public ApplicationUser TechnicalPropertiesConfirmedBy { get; set; }
        public ApplicationUser RequestConfirmedBy { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public ApplicationUser ProjectManager { get; set; }
        public ApplicationUser ProjectManagerSetBy { get; set; }
        public ApplicationUser FinancialNotificationBy { get; set; }
                

        // 0 - wszystkie dla okna projektu
        // 1 - ekonomiczne, 2 - jakościowe - dla osobnego okna
        public int TypeOfRequirement { get; set; }
    }
}

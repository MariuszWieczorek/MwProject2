using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class RankingCategoryRepository : IRankingCategoryRepository
    {
        private readonly IApplicationDbContext _context;
        public RankingCategoryRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<RankingCategory> GetRankingCategories()
        {
            return _context.RankingCategories
                .Include(x => x.RankingElements)
               .ToList();
        }

        public void AddRankingCategory(RankingCategory rankingCategory)
        {
            _context.RankingCategories.Add(rankingCategory);
        }

        public RankingCategory GetRankingCategory(int id)
        {
            return _context.RankingCategories
                .Include(x=>x.RankingElements)
                .Single(x => x.Id == id);
        }

        public RankingCategory NewRankingCategory()
        {
            return new RankingCategory();
        }

        public void UpdateRankingCategory(RankingCategory rankingCategory)
        {
            var rankingCategoryToUpdate = _context.RankingCategories.Single(x => x.Id == rankingCategory.Id);
            rankingCategoryToUpdate.Name = rankingCategory.Name;
            rankingCategoryToUpdate.Abbrev = rankingCategory.Abbrev;
        }

        public void DeleteRankingCategory(int id)
        {
            var rankingcategoryToDelete = _context.RankingCategories.Single(x => x.Id == id);
            _context.RankingCategories.Remove(rankingcategoryToDelete);
        }
    }
}

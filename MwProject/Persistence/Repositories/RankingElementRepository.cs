using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class RankingElementRepository : IRankingElementRepository
    {
        private readonly IApplicationDbContext _context;
        public RankingElementRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public RankingElement GetRankingElement(int rankingCategoryId, int id, string userId)
        {
            return _context.RankingElements
                .Single(x => x.RankingCategoryId == rankingCategoryId && x.Id == id);
        }

        public RankingElement NewRankingElement(int rankingCategoryId, string userId)
        {
            return new RankingElement()
            { 
                RankingCategoryId = rankingCategoryId
            };

        }

        public void AddRankingElement(RankingElement rankingElement, string userId)
        {
            _context.RankingElements.Add(rankingElement);
        }

        public void UpdateRankingElement(RankingElement rankingElement, string userId)
        {
            var rankingElementToUpdate = _context.RankingElements
                 .Single(x => x.RankingCategoryId == rankingElement.RankingCategoryId && x.Id == rankingElement.Id);
            
            rankingElementToUpdate.Name = rankingElement.Name;
            rankingElementToUpdate.RangeFrom = rankingElement.RangeFrom;
            rankingElementToUpdate.RangeTo = rankingElement.RangeTo;
            rankingElementToUpdate.Index = rankingElement.Index;
            rankingElementToUpdate.Description = rankingElement.Description;
        }

        public void DeleteRankingElement(int rankingCategoryId, int id, string userId)
        {
            var rankingElementToDelete = _context.RankingElements.Single(x => x.RankingCategoryId == rankingCategoryId && x.Id == id);
            _context.RankingElements.Remove(rankingElementToDelete);
        }

    }
}

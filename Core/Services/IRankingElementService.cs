using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface IRankingElementService
    {
        RankingElement GetRankingElement(int rankingCategoryId, int id, string userId);
        RankingElement NewRankingElement(int rankingCategoryId, string userId);
        void AddRankingElement(RankingElement rankingElement, string userId);
        void UpdateRankingElement(RankingElement rankingElement, string userId);
        void DeleteRankingElement(int rankingCategoryId, int id, string userId);
    }
}

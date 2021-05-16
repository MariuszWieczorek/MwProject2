using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface IRankingCategoryService
    {
        IEnumerable<RankingCategory> GetRankingCategories();
        void AddRankingCategory(RankingCategory rankingCategory);
        RankingCategory GetRankingCategory(int id);
        RankingCategory NewRankingCategory();
        void UpdateRankingCategory(RankingCategory rankingCategory);
        void DeleteRankingCategory(int id);
    }
}

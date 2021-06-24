using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class RankingCategoryService : IRankingCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RankingCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<RankingCategory> GetRankingCategories()
        {
            var rankingCategories = _unitOfWork.RankingCategoryRepository.GetRankingCategories();
            return rankingCategories;
        }

        public void AddRankingCategory(RankingCategory rankingCategory)
        {
            _unitOfWork.RankingCategoryRepository.AddRankingCategory(rankingCategory);
            _unitOfWork.Complete();
        }

        public RankingCategory GetRankingCategory(int id)
        {
            return _unitOfWork.RankingCategoryRepository.GetRankingCategory(id);
        }

        public RankingCategory NewRankingCategory()
        {
            return _unitOfWork.RankingCategoryRepository.NewRankingCategory();
        }

        public void UpdateRankingCategory(RankingCategory rankingCategory)
        {
            _unitOfWork.RankingCategoryRepository.UpdateRankingCategory(rankingCategory);
            _unitOfWork.Complete();
        }

        public void DeleteRankingCategory(int id)
        {
            _unitOfWork.RankingCategoryRepository.DeleteRankingCategory(id);
            _unitOfWork.Complete();
        }
    }
}

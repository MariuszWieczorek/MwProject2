using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class RankingElementService : IRankingElementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RankingElementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public RankingElement GetRankingElement(int rankingCategoryId, int id, string userId)
        {
            return _unitOfWork.RankingElementRepository.GetRankingElement(rankingCategoryId, id, userId); 
        }

        public RankingElement NewRankingElement(int rankingCategoryId, string userId)
        {
            return _unitOfWork.RankingElementRepository.NewRankingElement(rankingCategoryId, userId);
        }

        public void AddRankingElement(RankingElement rankingElement, string userId)
        {
            _unitOfWork.RankingElementRepository.AddRankingElement(rankingElement, userId);
            _unitOfWork.Complete();
        }

        public void UpdateRankingElement(RankingElement rankingElement, string userId)
        {
            _unitOfWork.RankingElementRepository.UpdateRankingElement(rankingElement, userId);
            _unitOfWork.Complete();
        }

        public void DeleteRankingElement(int rankingCategoryId, int id, string userId)
        {
            _unitOfWork.RankingElementRepository.DeleteRankingElement(rankingCategoryId, id, userId);
            _unitOfWork.Complete();
        }
    }
}

using aDealerEDVMS.Repository.ToanHH;
using aDealerEDVMS.Repository.ToanHH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDealerEDVMS.Service.ToanHH
{
    public class DealerContractsHhtService 
    {
        private readonly DealerContractsHhtRepository _repository;
        public DealerContractsHhtService()
        {
            _repository = new DealerContractsHhtRepository();
        }
        public async Task<List<DealerContractsHht>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                // Log hoặc xử lý lỗi nếu cần
                return new List<DealerContractsHht>();
            }
        }
    }
}

using aDealerEDVMS.Repository.ToanHH;
using aDealerEDVMS.Repository.ToanHH.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aDealerEDVMS.Service.ToanHH
{
    public class DearlerHhtService : IDealerHhtService
    {
        public readonly DealersHhtRepository _repository;
        public DearlerHhtService()
        {
            _repository = new DealersHhtRepository();
        }

        public async Task<List<DealersHht>> GetAllAsync()
        {
            try
            {
                var items = await _repository.GetAllAsync();
                return items;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return new List<DealersHht>();
            }
        }

        public async Task<DealersHht> GetByIdAsync(int dealerId)
        {
            try
            {
                var item = await _repository.GetByIdAsync(dealerId);
                return item;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return null;
            }
        }

        public async Task<int> CreateAsync(DealersHht dealer)
        {
            try
            {
                return await _repository.CreateAsync(dealer);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
            }
            return 0;
        }

        public async Task<bool> DeleteAsync(int dealerId)
        {
            try
            {
                var dealer = await _repository.GetByIdAsync(dealerId);
                if (dealer != null)
                {
                    return await _repository.RemoveAsync(dealer);
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
            }
            return false;
        }

        public async Task<List<DealersHht>> SearchAsync(string DealerName, decimal Rating, string Address)
        {
            try
            {
                var items = await _repository.SearchAsync(DealerName, Rating, Address);
                return items;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return new List<DealersHht>();
            }
        }

        public async Task<int> UpdateAsync(DealersHht dealer)
        {
            try
            {
                return await _repository.UpdateAsync(dealer);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}

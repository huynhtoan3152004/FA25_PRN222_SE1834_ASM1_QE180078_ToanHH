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
                return item; // Now returns null if not found
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine($"Error getting dealer {dealerId}: {ex.Message}");
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
                // Log chi tiết lỗi
                Console.WriteLine($"Error creating dealer: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
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
                    var result = await _repository.RemoveAsync(dealer);
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine($"Error deleting dealer {dealerId}: {ex.Message}");
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

        public async Task<(List<DealersHht> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _repository.GetPagedAsync(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return (new List<DealersHht>(), 0);
            }
        }

        public async Task<(List<DealersHht> Items, int TotalCount)> SearchPagedAsync(string DealerName, decimal Rating, string Address, int pageNumber, int pageSize)
        {
            try
            {
                return await _repository.SearchPagedAsync(DealerName, Rating, Address, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return (new List<DealersHht>(), 0);
            }
        }

    }
}

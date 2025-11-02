using aDealerEDVMS.Repository.ToanHH.Basic;
using aDealerEDVMS.Repository.ToanHH.DBcontext;
using aDealerEDVMS.Repository.ToanHH.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aDealerEDVMS.Repository.ToanHH
{
    public class DealersHhtRepository : GenericRepository<DealersHht>
    {
        public DealersHhtRepository() : base() { }

        public DealersHhtRepository(FA25_PRN221_SE1834_G5_EVDMSContext context) : base(context) { }

        public async Task<List<DealersHht>> GetAllAsync()
        {
            var items = await _context.DealersHhts.ToListAsync();
            return items ?? new List<DealersHht>();
        }

        // Lấy đại lý theo DealerId
        public async Task<DealersHht> GetByIdAsync(int dealerId)
        {
            var dealer = await _context.DealersHhts.FirstOrDefaultAsync(d => d.ToandealerId == dealerId);
            return dealer ?? new DealersHht();
        }

        // Tìm kiếm đại lý theo tên
        public async Task<List<DealersHht>> SearchAsync(string dealerName, decimal rating, string address)
        {
            var items = await _context.DealersHhts
                .Where(d => d.DealerName.Contains(dealerName) || string.IsNullOrEmpty(dealerName))
                .ToListAsync();

            return items ?? new List<DealersHht>();
        }

        // Get paged dealers
        public async Task<(List<DealersHht> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.DealersHhts.AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(d => d.ToandealerId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items ?? new List<DealersHht>(), totalCount);
        }

        // Search with pagination
        public async Task<(List<DealersHht> Items, int TotalCount)> SearchPagedAsync(string dealerName, decimal rating, string address, int pageNumber, int pageSize)
        {
            var query = _context.DealersHhts.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(dealerName))
            {
                query = query.Where(d => d.DealerName.Contains(dealerName));
            }
            if (rating > 0)
            {
                query = query.Where(d => d.Rating >= rating);
            }
            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(d => d.Address.Contains(address));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(d => d.ToandealerId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items ?? new List<DealersHht>(), totalCount);
        }
    }
}

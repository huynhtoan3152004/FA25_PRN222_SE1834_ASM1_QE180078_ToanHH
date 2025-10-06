using aDealerEDVMS.Service.ToanHH;
using Microsoft.AspNetCore.SignalR;
using aDealerEDVMS.Repository.ToanHH.Models;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Hubs
{
    public class aDealer : Hub
    {
        private readonly IDealerHhtService _dealerHhtService;
        public aDealer(IDealerHhtService dealerHhtService) => _dealerHhtService = dealerHhtService;

        public async Task DeleteDealerRealtime(int dealerId)
        {
            if (await _dealerHhtService.DeleteAsync(dealerId))
                await Clients.All.SendAsync("DealerDeleted", dealerId);
        }

        // SIMPLE: nhận tất cả là primitive / string -> ít lỗi binding
        public async Task CreateDealerRealtimeSimple(
            string dealerName,
            string dealerCode,
            string? address,
            string? phone,
            string? email,
            string? establishedDate,  // đổi sang string
            bool isActive,
            int? totalStaff,
            double? rating)
        {
            if (string.IsNullOrWhiteSpace(dealerName) || string.IsNullOrWhiteSpace(dealerCode))
            {
                await Clients.Caller.SendAsync("CreateResult", new { success = false, message = "Name & Code required" });
                return;
            }

            DateOnly? estDate = null;
            if (!string.IsNullOrWhiteSpace(establishedDate) && DateTime.TryParse(establishedDate, out var dt))
                estDate = DateOnly.FromDateTime(dt);

            var entity = new DealersHht
            {
                DealerName = dealerName,
                DealerCode = dealerCode,
                Address = address,
                Phone = phone,
                Email = email,
                EstablishedDate = estDate,
                IsActive = isActive,
                TotalStaff = totalStaff,
                Rating = (decimal?)(rating ?? 0),
                CreatedBy = 1,
                LastAudit = DateTime.Now
            };

            var newId = await _dealerHhtService.CreateAsync(entity);
            if (newId <= 0)
            {
                await Clients.Caller.SendAsync("CreateResult", new { success = false, message = "Create failed" });
                return;
            }

            entity.DealerId = newId;

            // Broadcast – camelCase
            await Clients.All.SendAsync("DealerCreated", new
            {
                dealerId = entity.DealerId,
                dealerName = entity.DealerName,
                dealerCode = entity.DealerCode,
                address = entity.Address,
                phone = entity.Phone,
                email = entity.Email,
                establishedDate = entity.EstablishedDate, // DateOnly? -> JSON ra "YYYY-MM-DD"
                isActive = entity.IsActive,
                totalStaff = entity.TotalStaff,
                rating = entity.Rating,
                lastAudit = entity.LastAudit,
                createdBy = entity.CreatedBy
            });
            await Clients.Caller.SendAsync("CreateResult", new { success = true, id = entity.DealerId });
        }
    }
}
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
            try
            {
                var dealer = await _dealerHhtService.GetByIdAsync(dealerId);
                if (dealer == null)
                {
                    await Clients.Caller.SendAsync("DeleteResult", new { success = false, message = "Dealer not found" });
                    return;
                }

                var success = await _dealerHhtService.DeleteAsync(dealerId);
                if (success)
                {
                    // Broadcast to ALL clients (including the caller)
                    await Clients.All.SendAsync("DealerDeleted", new
                    {
                        dealerId = dealerId,
                        dealerName = dealer.DealerName,
                        dealerCode = dealer.DealerCode,
                        deletedBy = "SignalR User",
                        deletedAt = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"),
                        message = $"Dealer '{dealer.DealerName}' deleted via SignalR",
                        deleteType = "signalr"
                    });

                    // Send success result to caller
                    await Clients.Caller.SendAsync("DeleteResult", new { success = true, dealerId = dealerId });
                }
                else
                {
                    await Clients.Caller.SendAsync("DeleteResult", new { success = false, message = "Delete failed" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteDealerRealtime: {ex.Message}");
                await Clients.Caller.SendAsync("DeleteResult", new { success = false, message = "Internal server error" });
            }
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

            entity.ToandealerId = newId;

            // Broadcast – camelCase
            await Clients.All.SendAsync("DealerCreated", new
            {
                dealerId = entity.ToandealerId,
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
            await Clients.Caller.SendAsync("CreateResult", new { success = true, id = entity.ToandealerId });
        }
        
        // Thêm phương thức UpdateDealerRealtime
        public async Task UpdateDealerRealtime(
            int dealerId,
            string dealerName,
            string dealerCode,
            string? address,
            string? phone,
            string? email,
            string? establishedDate,
            bool isActive,
            int? totalStaff,
            double? rating)
        {
            if (string.IsNullOrWhiteSpace(dealerName) || string.IsNullOrWhiteSpace(dealerCode))
            {
                await Clients.Caller.SendAsync("UpdateResult", new { success = false, message = "Name & Code required" });
                return;
            }

            // Kiểm tra xem dealer có tồn tại không
            var existingDealer = await _dealerHhtService.GetByIdAsync(dealerId);
            if (existingDealer == null)
            {
                await Clients.Caller.SendAsync("UpdateResult", new { success = false, message = "Dealer not found" });
                return;
            }

            DateOnly? estDate = null;
            if (!string.IsNullOrWhiteSpace(establishedDate) && DateTime.TryParse(establishedDate, out var dt))
                estDate = DateOnly.FromDateTime(dt);

            // Cập nhật thông tin dealer
            existingDealer.DealerName = dealerName;
            existingDealer.DealerCode = dealerCode;
            existingDealer.Address = address;
            existingDealer.Phone = phone;
            existingDealer.Email = email;
            existingDealer.EstablishedDate = estDate;
            existingDealer.IsActive = isActive;
            existingDealer.TotalStaff = totalStaff;
            existingDealer.Rating = (decimal?)(rating ?? 0);
            existingDealer.LastAudit = DateTime.Now;

            // Cập nhật vào database
            var result = await _dealerHhtService.UpdateAsync(existingDealer);
            var success = result > 0; // Chuyển đổi int thành bool
            
            if (!success)
            {
                await Clients.Caller.SendAsync("UpdateResult", new { success = false, message = "Update failed" });
                return;
            }

            // Broadcast thông tin đã cập nhật đến tất cả clients
            await Clients.All.SendAsync("DealerUpdated", new
            {
                dealerId = existingDealer.ToandealerId,
                dealerName = existingDealer.DealerName,
                dealerCode = existingDealer.DealerCode,
                address = existingDealer.Address,
                phone = existingDealer.Phone,
                email = existingDealer.Email,
                establishedDate = existingDealer.EstablishedDate,
                isActive = existingDealer.IsActive,
                totalStaff = existingDealer.TotalStaff,
                rating = existingDealer.Rating,
                lastAudit = existingDealer.LastAudit,
                createdBy = existingDealer.CreatedBy
            });
            
            // Gửi kết quả thành công cho caller
            await Clients.Caller.SendAsync("UpdateResult", new { success = true, id = existingDealer.ToandealerId });
        }
    }
}
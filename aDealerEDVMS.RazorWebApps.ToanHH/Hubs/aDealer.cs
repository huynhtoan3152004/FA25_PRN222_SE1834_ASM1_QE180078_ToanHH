using aDealerEDVMS.Service.ToanHH;
using Microsoft.AspNetCore.SignalR;
using aDealerEDVMS.Repository.ToanHH.Models;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Hubs
{
    public class aDealer : Hub
    {
        private readonly IDealerHhtService _dealerHhtService;
        
        public aDealer(IDealerHhtService dealerHhtService)
        {
            _dealerHhtService = dealerHhtService;
        }

        // Delete dealer qua SignalR (đã có)
        public async Task DeleteDealerRealtime(int dealerId)
        {
            Console.WriteLine($"🎯 Hub: Delete dealer ID: {dealerId}");
            
            var success = await _dealerHhtService.DeleteAsync(dealerId);
            
            if (success)
            {
                await Clients.All.SendAsync("DealerDeleted", dealerId);
                Console.WriteLine("✅ Broadcasted delete notification");
            }
        }

        // ✅ Thêm method Create mới (đơn giản)
        public async Task CreateDealerRealtime(DealersHht dealer)
        {
            Console.WriteLine($"🎯 Hub: Create dealer: {dealer.DealerName}");
            
            var createdDealer = await _dealerHhtService.CreateAsync(dealer);
            
            if (createdDealer != null)
            {
                await Clients.All.SendAsync("DealerCreated", createdDealer);
                Console.WriteLine($"✅ Broadcasted create: Dealer ID {createdDealer}");
            }
        }
    }
}
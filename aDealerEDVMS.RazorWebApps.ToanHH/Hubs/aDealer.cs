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
                Console.WriteLine("Broadcasted delete notification");
            }
        }

        // ✅ SỬA: Create method đơn giản
        public async Task CreateDealerRealtime(DealersHht dealer)
        {
            Console.WriteLine($"🎯 Hub: Create dealer: {dealer.DealerName}");
            
            var newDealerId = await _dealerHhtService.CreateAsync(dealer);
            
            if (newDealerId > 0)
            {
                await Clients.All.SendAsync("DealerCreated", new
                {
                    DealerId = newDealerId,
                    DealerName = dealer.DealerName,
                    DealerCode = dealer.DealerCode
                });

                Console.WriteLine($"✅ Broadcasted create: {dealer.DealerName} with ID {newDealerId}");
            }
        }
    }
}
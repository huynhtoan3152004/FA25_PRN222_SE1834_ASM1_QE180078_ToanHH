using aDealerEDVMS.Service.ToanHH;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Hubs
{
    public class aDealer : Hub
    {
        //Khai báo interface 
        private readonly IDealerHhtService _dealerHhtService;
        public aDealer(IDealerHhtService dealerHhtService)
        {
            _dealerHhtService = dealerHhtService;
        }
        public async Task Hubdelete_aDealer(string DealerId)
        { await Clients.All.SendAsync("Receiver DealerHhtService", DealerId); }
    }
}

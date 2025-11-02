using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aDealerEDVMS.Repository.ToanHH.Models;
using aDealerEDVMS.Service.ToanHH;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using aDealerEDVMS.RazorWebApps.ToanHH.Hubs;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Pages.Dealers
{
    [Authorize(Roles = "1")]
    public class DeleteModel : PageModel
    {
        private readonly IDealerHhtService _dealerHhtService;
        private readonly IHubContext<aDealer> _hubContext;

        public DeleteModel(IDealerHhtService dealerHhtService, IHubContext<aDealer> hubContext)
        {
            _dealerHhtService = dealerHhtService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public DealersHht DealersHht { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var dealer = await _dealerHhtService.GetByIdAsync(id.Value);
            if (dealer == null) return NotFound();
            
            DealersHht = dealer;
            return Page();
        }

        // Delete thông thường (nút Delete đỏ)
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var dealerToDelete = await _dealerHhtService.GetByIdAsync(id.Value);
            if (dealerToDelete == null) return NotFound();

            var success = await _dealerHhtService.DeleteAsync(id.Value);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to delete dealer. Please try again.");
                return Page();
            }

            // Gửi notification qua SignalR cho delete thông thường - broadcast to all clients
            await _hubContext.Clients.All.SendAsync("DealerDeleted", id.Value);

            return RedirectToPage("./Index");
        }
    }
}
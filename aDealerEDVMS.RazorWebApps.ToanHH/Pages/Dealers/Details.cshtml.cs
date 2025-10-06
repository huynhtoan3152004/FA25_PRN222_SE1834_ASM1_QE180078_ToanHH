using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aDealerEDVMS.Repository.ToanHH.Models;
using Microsoft.AspNetCore.Authorization;
using aDealerEDVMS.Service.ToanHH;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Pages.Dealers
{
    [Authorize(Roles = "1,2")]
    public class DetailsModel : PageModel
    {
        private readonly IDealerHhtService _dealerHhtService;

        public DetailsModel(IDealerHhtService dealerHhtService)
        {
            _dealerHhtService = dealerHhtService;
        }

        public DealersHht DealersHht { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealershht = await _dealerHhtService.GetByIdAsync(id.Value);
            if (dealershht == null)
            {
                return NotFound();
            }
            
            DealersHht = dealershht;
            return Page();
        }
    }
}

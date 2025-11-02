using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aDealerEDVMS.Repository.ToanHH.Models;
using aDealerEDVMS.Service.ToanHH;
using Microsoft.AspNetCore.Authorization;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Pages.Dealers
{
    [Authorize(Roles = "1,2")]
    public class EditModel : PageModel
    {
        private readonly IDealerHhtService _dealerHhtService;

        public EditModel(IDealerHhtService dealerHhtService)
        {
            _dealerHhtService = dealerHhtService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _dealerHhtService.UpdateAsync(DealersHht);
            }
            catch (Exception)
            {
                if (await _dealerHhtService.GetByIdAsync(DealersHht.ToandealerId) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}

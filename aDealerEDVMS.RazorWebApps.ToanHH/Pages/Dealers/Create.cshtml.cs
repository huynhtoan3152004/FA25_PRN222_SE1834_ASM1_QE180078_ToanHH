using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using aDealerEDVMS.Repository.ToanHH.DBcontext;
using aDealerEDVMS.Repository.ToanHH.Models;
using aDealerEDVMS.Service.ToanHH;
using Microsoft.AspNetCore.Authorization;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Pages.Dealers
{
    [Authorize(Roles = "1,2")]
    public class CreateModel : PageModel
    {
        private readonly IDealerHhtService _dealerHhtService;
        
        public CreateModel(IDealerHhtService dealerHhtService)
        {
            _dealerHhtService = dealerHhtService;
        }

        public async Task<IActionResult> OnGet()
        {
            // Initialize new dealer object
            var DealersHht = await _dealerHhtService.GetAllAsync();
            
            // Load any reference data if needed (e.g., for dropdowns)
            // You can load other related data here using the service
            
            return Page();
        }

        [BindProperty]
        public DealersHht DealersHht { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Thêm các giá trị bắt buộc trước khi lưu
            DealersHht.CreatedBy = 1; // Hoặc lấy từ User.Identity.Name nếu có authentication
            DealersHht.LastAudit = DateTime.Now;
            
            // Use the correct method name from your service
            await _dealerHhtService.CreateAsync(DealersHht);
            return RedirectToPage("./Index");
        }
    }
}

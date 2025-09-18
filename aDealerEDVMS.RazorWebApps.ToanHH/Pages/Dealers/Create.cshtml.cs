using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using aDealerEDVMS.Repository.ToanHH.DBcontext;
using aDealerEDVMS.Repository.ToanHH.Models;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Pages.Dealers
{
    public class CreateModel : PageModel
    {
        private readonly aDealerEDVMS.Repository.ToanHH.DBcontext.FA25_PRN221_SE1834_G5_EVDMSContext _context;

        public CreateModel(aDealerEDVMS.Repository.ToanHH.DBcontext.FA25_PRN221_SE1834_G5_EVDMSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DealersHht DealersHht { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.DealersHhts.Add(DealersHht);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aDealerEDVMS.Repository.ToanHH.DBcontext;
using aDealerEDVMS.Repository.ToanHH.Models;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Pages.Dealers
{
    public class DeleteModel : PageModel
    {
        private readonly aDealerEDVMS.Repository.ToanHH.DBcontext.FA25_PRN221_SE1834_G5_EVDMSContext _context;

        public DeleteModel(aDealerEDVMS.Repository.ToanHH.DBcontext.FA25_PRN221_SE1834_G5_EVDMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DealersHht DealersHht { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealershht = await _context.DealersHhts.FirstOrDefaultAsync(m => m.DealerId == id);

            if (dealershht == null)
            {
                return NotFound();
            }
            else
            {
                DealersHht = dealershht;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealershht = await _context.DealersHhts.FindAsync(id);
            if (dealershht != null)
            {
                DealersHht = dealershht;
                _context.DealersHhts.Remove(DealersHht);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

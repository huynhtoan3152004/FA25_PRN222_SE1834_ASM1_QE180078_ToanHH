using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aDealerEDVMS.Repository.ToanHH.DBcontext;
using aDealerEDVMS.Repository.ToanHH.Models;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Pages.Dealers
{
    public class EditModel : PageModel
    {
        private readonly aDealerEDVMS.Repository.ToanHH.DBcontext.FA25_PRN221_SE1834_G5_EVDMSContext _context;

        public EditModel(aDealerEDVMS.Repository.ToanHH.DBcontext.FA25_PRN221_SE1834_G5_EVDMSContext context)
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

            var dealershht =  await _context.DealersHhts.FirstOrDefaultAsync(m => m.DealerId == id);
            if (dealershht == null)
            {
                return NotFound();
            }
            DealersHht = dealershht;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(DealersHht).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DealersHhtExists(DealersHht.DealerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DealersHhtExists(int id)
        {
            return _context.DealersHhts.Any(e => e.DealerId == id);
        }
    }
}

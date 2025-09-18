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
    public class IndexModel : PageModel
    {
        private readonly aDealerEDVMS.Repository.ToanHH.DBcontext.FA25_PRN221_SE1834_G5_EVDMSContext _context;

        public IndexModel(aDealerEDVMS.Repository.ToanHH.DBcontext.FA25_PRN221_SE1834_G5_EVDMSContext context)
        {
            _context = context;
        }

        public IList<DealersHht> DealersHht { get;set; } = default!;

        public async Task OnGetAsync()
        {
            DealersHht = await _context.DealersHhts
                .Include(d => d.DealerContractsHhts) // Lấy luôn các hợp đồng của từng dealer
                .ToListAsync();
        }
    }
}

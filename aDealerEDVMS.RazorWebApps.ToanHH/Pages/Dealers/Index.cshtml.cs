using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aDealerEDVMS.Repository.ToanHH.DBcontext;
using aDealerEDVMS.Repository.ToanHH.Models;
using aDealerEDVMS.Service.ToanHH;

namespace aDealerEDVMS.RazorWebApps.ToanHH.Pages.Dealers
{
    public class IndexModel : PageModel
    {
        private readonly IDealerHhtService _dealerHhtService;

        public IndexModel(IDealerHhtService dealerHhtService)
        {
            _dealerHhtService = dealerHhtService;
        }

        public IList<DealersHht> DealersHht { get; set; } = default!;

        public async Task OnGetAsync()
        {
            DealersHht = await _dealerHhtService.GetAllAsync();
        }
    }
}

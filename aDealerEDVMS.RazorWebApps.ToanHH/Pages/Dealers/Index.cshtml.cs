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
    public class IndexModel : PageModel
    {
        private readonly IDealerHhtService _dealerHhtService;

        public IndexModel(IDealerHhtService dealerHhtService)
        {
            _dealerHhtService = dealerHhtService;
        }

        public IList<DealersHht> DealersHht { get; set; } = new List<DealersHht>();

        // Search parameters
        [BindProperty(SupportsGet = true)]
        public string SearchDealerName { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public decimal? SearchRating { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchAddress { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string StatusFilter { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            try
            {
                // Load dealers based on search criteria
                if (!string.IsNullOrWhiteSpace(SearchDealerName) || 
                    SearchRating.HasValue || 
                    !string.IsNullOrWhiteSpace(SearchAddress))
                {
                    // Use search method
                    DealersHht = await _dealerHhtService.SearchAsync(
                        SearchDealerName?.Trim() ?? string.Empty, 
                        SearchRating ?? 0, 
                        SearchAddress?.Trim() ?? string.Empty);
                }
                else
                {
                    // Load all dealers
                    DealersHht = await _dealerHhtService.GetAllAsync();
                }

                // Ensure we have a list (not null)
                if (DealersHht == null)
                {
                    DealersHht = new List<DealersHht>();
                }

                // Apply status filter
                if (!string.IsNullOrWhiteSpace(StatusFilter) && bool.TryParse(StatusFilter, out bool isActive))
                {
                    DealersHht = DealersHht.Where(d => d.IsActive == isActive).ToList();
                }

                // Apply sorting
                DealersHht = SortBy?.ToLower() switch
                {
                    "name" => DealersHht.OrderBy(d => d.DealerName ?? string.Empty).ToList(),
                    "rating" => DealersHht.OrderByDescending(d => d.Rating ?? 0).ToList(),
                    
                    _ => DealersHht.OrderBy(d => d.DealerId).ToList()
                };

                // Store current filter values for display
                ViewData["CurrentDealerNameFilter"] = SearchDealerName?.Trim();
                ViewData["CurrentRatingFilter"] = SearchRating;
                ViewData["CurrentAddressFilter"] = SearchAddress?.Trim();
                ViewData["CurrentStatusFilter"] = StatusFilter;
                ViewData["CurrentSortBy"] = SortBy;
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                DealersHht = new List<DealersHht>();
                
                // Log error (you can add logging here if needed)
                ViewData["ErrorMessage"] = "An error occurred while loading dealers data.";
                
                // Clear filter values on error
                ViewData["CurrentDealerNameFilter"] = string.Empty;
                ViewData["CurrentRatingFilter"] = null;
                ViewData["CurrentAddressFilter"] = string.Empty;
                ViewData["CurrentStatusFilter"] = string.Empty;
                ViewData["CurrentSortBy"] = string.Empty;
            }
        }
    }
}

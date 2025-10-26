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

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalPages { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                int totalCount = 0;

                // Load dealers based on search criteria with pagination
                if (!string.IsNullOrWhiteSpace(SearchDealerName) ||
                    SearchRating.HasValue ||
                    !string.IsNullOrWhiteSpace(SearchAddress))
                {
                    // Use search with pagination
                    var result = await _dealerHhtService.SearchPagedAsync(
                        SearchDealerName?.Trim() ?? string.Empty,
                        SearchRating ?? 0,
                        SearchAddress?.Trim() ?? string.Empty,
                        PageNumber,
                        PageSize);
                    DealersHht = result.Items;
                    totalCount = result.TotalCount;
                }
                else
                {
                    // Load all dealers with pagination
                    var result = await _dealerHhtService.GetPagedAsync(PageNumber, PageSize);
                    DealersHht = result.Items;
                    totalCount = result.TotalCount;
                }

                // Ensure we have a list (not null)
                if (DealersHht == null)
                {
                    DealersHht = new List<DealersHht>();
                }

                // Apply status filter (client-side since we already have paged data)
                if (!string.IsNullOrWhiteSpace(StatusFilter) && bool.TryParse(StatusFilter, out bool isActive))
                {
                    DealersHht = DealersHht.Where(d => d.IsActive == isActive).ToList();
                }

                // Apply sorting (client-side since we already have paged data)
                DealersHht = SortBy?.ToLower() switch
                {
                    "name" => DealersHht.OrderBy(d => d.DealerName ?? string.Empty).ToList(),
                    "rating" => DealersHht.OrderByDescending(d => d.Rating ?? 0).ToList(),
                    "date" => DealersHht.OrderByDescending(d => d.LastAudit ?? DateTime.MinValue).ToList(),
                    _ => DealersHht.OrderBy(d => d.DealerId).ToList()
                };

                // Calculate total pages
                TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);

                // Ensure page number is valid
                if (PageNumber < 1) PageNumber = 1;
                if (PageNumber > TotalPages && TotalPages > 0) PageNumber = TotalPages;

                // Store current filter values for display
                ViewData["CurrentDealerNameFilter"] = SearchDealerName?.Trim();
                ViewData["CurrentRatingFilter"] = SearchRating;
                ViewData["CurrentAddressFilter"] = SearchAddress?.Trim();
                ViewData["CurrentStatusFilter"] = StatusFilter;
                ViewData["CurrentSortBy"] = SortBy;
                ViewData["CurrentPageNumber"] = PageNumber;
                ViewData["CurrentPageSize"] = PageSize;
                ViewData["TotalCount"] = totalCount;
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                DealersHht = new List<DealersHht>();
                TotalPages = 0;

                // Log error (you can add logging here if needed)
                ViewData["ErrorMessage"] = "An error occurred while loading dealers data.";

                // Clear filter values on error
                ViewData["CurrentDealerNameFilter"] = string.Empty;
                ViewData["CurrentRatingFilter"] = null;
                ViewData["CurrentAddressFilter"] = string.Empty;
                ViewData["CurrentStatusFilter"] = string.Empty;
                ViewData["CurrentSortBy"] = string.Empty;
                ViewData["CurrentPageNumber"] = 1;
                ViewData["CurrentPageSize"] = PageSize;
                ViewData["TotalCount"] = 0;
            }
        }
    }
}

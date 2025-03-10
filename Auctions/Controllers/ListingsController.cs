using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auctions.Data;
using Auctions.Models;
using Auctions.Data.Services;
using Auctions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Auctions.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingService _listingService;
        private readonly IRepositoryService<Bid> _bidRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepositoryService<Comment> _commentRepo;

        public ListingsController(IListingService listingService,IRepositoryService<Bid> bidRepo,
            UserManager<IdentityUser> userManager, IRepositoryService<Comment> commentRepo)
        {
            _listingService = listingService;
            _bidRepo = bidRepo;
            _userManager = userManager;
            _commentRepo = commentRepo;
        }

        // GET: Listings
        public async Task<IActionResult> Index(int? pageNumber,string searchValue)
        {
            var listings = _listingService.GetAll();
            int pageSize = 3;
            if (!string.IsNullOrEmpty(searchValue))
            {
                listings = listings.Where(l => l.Title.Contains(searchValue));
                return View(await PaginatedList<Listing>.CreateAsync(listings.Where(s => s.IsSold == false).AsNoTracking(), pageNumber ?? 1, pageSize));
            }

            return View(await PaginatedList<Listing>.CreateAsync(listings.Where(s => s.IsSold == false).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: MyListings
        public async Task<IActionResult> MyListings(int? pageNumber)
        {
            var listings = _listingService.GetAll();
            int pageSize = 3;
           
            return View(nameof(Index),await PaginatedList<Listing>.CreateAsync(listings
                   .Where(s => s.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: MyBids
        public async Task<IActionResult> MyBids(int? pageNumber)
        {
            var bids = _bidRepo.GetAll();
            int pageSize = 3;

            return View(await PaginatedList<Bid>.CreateAsync(bids
                   .Where(s => s.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _listingService.GetByIdAsync(id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listings/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingViewModel listingVM)
        {
            if (ModelState.IsValid)
            {
               var imageName =  DocumentSettings.UploadFile(listingVM.Image, "images");
                var mappedListing = new Listing()
                {
                    Title = listingVM.Title,
                    Description = listingVM.Description,
                    Price = listingVM.Price,
                    ImagePath = imageName,
                    IdentityUserId = listingVM.IdentityUserId,
                };
               await _listingService.AddAsync(mappedListing);
                var result = await _listingService.CompleteAsync();
                if (result > 0)
                    TempData["Message"] = "New Listing is Added!";
                return RedirectToAction("Index");
            }
           return View(listingVM);
            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBid(BidViewModel bidVM)
        {
            if(!ModelState.IsValid)
            {
                return View(nameof(Details), bidVM);
            }
            
            bidVM.IdentityUserId = _userManager.GetUserId(User);

            var bid = new Bid()
            {
                IdentityUserId = bidVM.IdentityUserId,
                Price = bidVM.Price,
                ListingId = bidVM.ListingId,
            };

            await _bidRepo.AddAsync(bid);
            await _bidRepo.CompleteAsync();

            var listing = await _listingService.GetByIdAsync(bid.ListingId);
            listing.Price = bid.Price;
            var result = await _listingService.CompleteAsync();

            return View(nameof(Details), listing);
        }
        [HttpPost]
        public async Task<IActionResult> CloseBidding(int id)
        {
            var listing = await _listingService.GetByIdAsync(id);
            listing.IsSold = true;
            await _listingService.CompleteAsync();
            return RedirectToAction(nameof(Details),listing);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel commentVM)
        {
            commentVM.IdentityUserId = _userManager.GetUserId(User);
            if (!ModelState.IsValid)
            {
                return View(nameof(Details), commentVM);
            }

            var comment = new Comment()
            {
                Content = commentVM.Content,
                ListingId= commentVM.ListingId,
                IdentityUserId = commentVM.IdentityUserId
            };

            await _commentRepo.AddAsync(comment);
            await _commentRepo.CompleteAsync();
            var listing = await _listingService.GetByIdAsync(commentVM.ListingId);

            return View(nameof(Details), listing);
        }


        //// GET: Listings/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Listings == null)
        //    {
        //        return NotFound();
        //    }

        //    var listing = await _context.Listings.FindAsync(id);
        //    if (listing == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", listing.IdentityUserId);
        //    return View(listing);
        //}

        //// POST: Listings/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,ImageUrl,IsSold,IdentityUserId")] Listing listing)
        //{
        //    if (id != listing.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(listing);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ListingExists(listing.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", listing.IdentityUserId);
        //    return View(listing);
        //}

        //// GET: Listings/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Listings == null)
        //    {
        //        return NotFound();
        //    }

        //    var listing = await _context.Listings
        //        .Include(l => l.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (listing == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(listing);
        //}

        //// POST: Listings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Listings == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Listings'  is null.");
        //    }
        //    var listing = await _context.Listings.FindAsync(id);
        //    if (listing != null)
        //    {
        //        _context.Listings.Remove(listing);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ListingExists(int id)
        //{
        //  return (_context.Listings?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}

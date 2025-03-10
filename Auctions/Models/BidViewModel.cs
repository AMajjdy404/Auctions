using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Auctions.Models
{
    public class BidViewModel
    {
        public double Price { get; set; }
        public string? IdentityUserId { get; set; } = null!; 

        public int? ListingId { get; set; }
       
    }
}

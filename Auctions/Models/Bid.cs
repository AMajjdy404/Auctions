using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Auctions.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public double Price { get; set; }
        [Required]
        public string? IdentityUserId { get; set; } = null!; // FK

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser? User { get; set; }

        public int? ListingId { get; set; }
        [ForeignKey(nameof(ListingId))]
        public Listing? Listing { get; set; } = null!;
    }
}
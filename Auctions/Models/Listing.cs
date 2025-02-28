using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auctions.Models
{
    public class Listing
    {
        public int Id { get; set; }
        public string? Title { get; set; }    
        public string? Description { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; } = null!;
        public bool IsSold { get; set; } = false;
        [Required]
        public string? IdentityUserId { get; set; } = null!; // FK 

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser? User { get; set; } 

        public List<Bid>? Bids { get; set; } = new List<Bid>();
        public List<Comment>? Comments { get; set; } = new List<Comment>();

    }
}

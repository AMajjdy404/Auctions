using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auctions.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public string? IdentityUserId { get; set; } = null!; // FK

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser? User { get; set; }

        public int? ListingId { get; set; }
        [ForeignKey(nameof(ListingId))]
        public Listing? Listing { get; set; } = null!;

        public DateTime CommentDate { get; set; } = DateTime.Now;
    }
}
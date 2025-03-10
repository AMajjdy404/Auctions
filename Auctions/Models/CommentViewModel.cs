using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auctions.Models
{
    public class CommentViewModel
    {
        public string Content { get; set; } = null!;
        public string? IdentityUserId { get; set; } = null!; 
        public int? ListingId { get; set; }

    }
}

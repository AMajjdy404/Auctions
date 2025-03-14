﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Auctions.Models
{
    public class ListingViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; } = null!;
        public bool IsSold { get; set; } = false;
        [Required]
        public string? IdentityUserId { get; set; } = null!; 

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser? User { get; set; }
    }
}

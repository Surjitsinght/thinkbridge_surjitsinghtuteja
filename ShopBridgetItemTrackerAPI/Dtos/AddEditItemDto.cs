using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using static ShopBridgeItemTrackerAPI.Helper;

namespace ShopBridgeItemTrackerAPI.Dtos
{    
    public class AddEditItemDto
    {
        [Required]
        [RegularExpression(VHelper.AlphaNumRegex, ErrorMessage = VHelper.InvalidMessage)]
        public string Name { get; set; }

        [Required, Range(0, 100000)]
        [RegularExpression(VHelper.AmtRegex, ErrorMessage = VHelper.InvalidMessage)]
        public decimal Price { get; set; }

        [MaxLength(500)]
        [RegularExpression(VHelper.StringRegex, ErrorMessage = VHelper.InvalidMessage)]
        public string Description { get; set; }
        public IFormFile ProductImg { get; set; }
    }
}

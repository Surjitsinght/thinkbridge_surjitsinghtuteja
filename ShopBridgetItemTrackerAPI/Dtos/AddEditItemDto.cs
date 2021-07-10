using System;
using System.ComponentModel.DataAnnotations;

namespace ShopBridgeItemTrackerAPI.Dtos
{
    public class AddEditItemDto
    {
        [Required]
        public string Name { get; set; }

        [Required, Range(0, 100000)]
        public decimal Price { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}

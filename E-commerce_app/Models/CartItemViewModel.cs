using System.ComponentModel.DataAnnotations;

namespace E_commerce_app.Models
{
    public class CartItemViewModel
    {
        [Required]
        public string CartItemID { get; set; }
        [Required]
        public string ProductID { get; set; }
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int StockQuantity { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]

        public decimal UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Discount { get; set; } //The ammount of money saved for one item

    }
}

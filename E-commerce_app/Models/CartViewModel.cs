using System.ComponentModel.DataAnnotations;

namespace E_commerce_app.Models
{
    public class CartViewModel
    {
        [Required]
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        [Required]
        public decimal TotalSavings { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
    }
}

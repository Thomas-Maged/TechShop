using Microsoft.Build.Framework;

namespace E_commerce_app.Models
{
    public class OrderItemViewModel
    {
        [Required]
        public string ProductID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Image { get; set; }
    }
}

using E_commerce_entities.Models;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_app.Models
{
    public class OrderViewModel
    {
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        [Required]
        public decimal TotalSavings { get; set; } = 0;
        [Required]
        public decimal TotalPrice { get; set; } = 0;
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string ZIP { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}

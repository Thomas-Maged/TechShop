using E_commerce_entities.Models;

namespace E_commerce_app.Models
{
    public class ProductViewModel
    {
        public string? ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }
        public string CategoryID { get; set; }
    }
}

using E_commerce_entities.Models;

namespace E_commerce_app.Models
{
    public class ProductsViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public int TotalNumberOfProducts { get; set; }
        public int FromProduct { get; set; }
        public int ToProduct { get; set; }
        public string CategoryID { get; set; }
        public int Page { get; set; }
        public int MaxPrice { get; set; }
        public int MinPrice { get; set; }

    }
}

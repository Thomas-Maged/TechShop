using E_commerce_entities.Enums;
namespace E_commerce_app.Models
{
    public class OrderDetailsViewModel
    {
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        public string OrderID { get; set; }
        public int OrderNumber { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

    }
}

using E_commerce_entities.Enums;
namespace E_commerce_app.Models
{
    
    public class OrderSummaryViewModel
    {
        public int OrderNumber { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal TotalAmount { get; set; }
    }
}

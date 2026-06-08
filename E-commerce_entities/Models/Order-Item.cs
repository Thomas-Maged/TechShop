using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace E_commerce_entities.Models
{
    public class Order_Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderItemID { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal LineTotal { get; set; }
        [Required]
        public string OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }
        [Required]
        public string ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }

    }
}

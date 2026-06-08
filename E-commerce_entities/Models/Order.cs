using E_commerce_entities.Data;
using E_commerce_entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace E_commerce_entities.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNumber { get; set; }
        [Required]
        public  StatusEnum Status { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public Decimal TotalAmount { get; set; }
        [Required]
        public decimal TotalDiscount { get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        [ForeignKey("Address")]
        public string ShippingAddressID { get; set; }
        public Address Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required]
        public string Fullname { get; set; }
        public ICollection<Order_Item> order_Items { get; set; }
    }
}

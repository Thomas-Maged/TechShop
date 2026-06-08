using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace E_commerce_entities.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CategoryID { get; set; }
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

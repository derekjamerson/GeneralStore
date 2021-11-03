using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralStore.Models
{
    public class Product
    {
        [Key]
        public int SKU { get; set; }

        [Required]
        public string Name { get; set; }

        private double _cost;
        [Required]
        public double Cost 
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = Math.Round(value, 2);
            }
        }

        [Required]
        public int NumberInInventory { get; set; }
        public bool IsInStock => NumberInInventory > 0;
    }
}

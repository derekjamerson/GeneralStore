﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralStore.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Customer)), Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(Product)), Required]
        public virtual int ProductSKU { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int ItemCount { get; set; }
        [Required]
        public DateTime DateOfTransaction { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace is_lab6.Models
{
    public partial class ShopItem
    {
        public int ItemId { get; set; }  //Нет поля requred т.к. выставляется ядром СУБД
        [Required]
        public string ItemName { get; set; }
        public string ItemStats { get; set; }
        [Required]
        public long ItemPrice { get; set; }

        [Column(TypeName="varchar(30)")]
        public string ItemColor { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string ItemWeight { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string ItemLifetime { get; set; } //Срок службы

        public int? TypeCode { get; set; }
        public Types Type { get; set; }
    }
}

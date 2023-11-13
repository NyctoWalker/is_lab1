using System;
using System.Collections.Generic;

#nullable disable

namespace is_lab6.Models
{
    public partial class ShopItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemStats { get; set; }
        public long ItemPrice { get; set; }
    }
}

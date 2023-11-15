using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace is_lab6.Models
{
    public partial class Types
    {
        [Key]
        [Required]
        public int Code { get; set; }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string TypeName { get; set; }

        public ICollection<ShopItem> ShopItems { get; set; }
        public Types()
        {
            ShopItems = new List<ShopItem>();
        }
    }
}

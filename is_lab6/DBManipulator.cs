using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using is_lab6.Models;

namespace is_lab6
{
    public class DBManipulator
    {
        public void DBAdd(string name, long price, string stats)
        {
            ShopItem item = new ShopItem
            {
                ItemName = name,
                ItemPrice = price,
                ItemStats = stats,
            };

            using (is_arch6Context db = new())
            {
                db.ShopItems.Add(item);
                db.SaveChanges();
            }
        }

        public List<string> DBReadByNumber()
        {
            List<string> output = new();
            int r = 1;

            using (is_arch6Context db = new())
            {
                var items = db.ShopItems;
                foreach (ShopItem i in items)
                {
                    output.Add($"{r}. {i.ItemName} ({i.ItemPrice} руб.)\nХарактеристики: {i.ItemStats}\n");
                    r++;
                }
            }

            return output;
        }
            
    }
}

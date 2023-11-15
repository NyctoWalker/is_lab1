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
            string[] values = stats.Split("; ");
            string color ="";
            string weight="";
            string lifetime="";

            string[] validValues = 
            { 
                "Цвет: ",
                "Вес: ",
                "Срок службы: "
            };

            foreach (string s in values)
            {
                if (validValues.Contains(s.Substring(0, s.IndexOf(':'))))
                {
                    switch (s.Substring(0, s.IndexOf(':')))
                    {
                        case "Цвет: ":
                            color = s.Substring(s.IndexOf(':') + 1);
                            break;

                        case "Вес: ":
                            weight = s.Substring(s.IndexOf(':') + 1);
                            break;

                        case "Срок службы: ":
                            lifetime = s.Substring(s.IndexOf(':') + 1);
                            break;
                    }
                }
            }


            ShopItem item = new ShopItem
            {
                ItemName = name,
                ItemPrice = price,
                ItemColor = color,
                ItemLifetime = lifetime,
                ItemWeight = weight,
                //ItemStats = stats,
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
                    output.Add($"{r}. {i.ItemName} ({i.ItemPrice} руб.)\n"); //\nХарактеристики: {i.ItemStats}
                    r++;
                }
            }

            return output;
        }
            
    }
}

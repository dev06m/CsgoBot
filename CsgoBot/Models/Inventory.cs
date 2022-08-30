using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Models
{
    public class Inventory
    {
        public List<InventoryItem> data { get; set; }
        public Metadata metadata { get; set; }
        public string status { get; set; }
    }

    public class InventoryItem
    {
        public int id { get; set; }
        public string project { get; set; }
        public string steam_market_hash_name { get; set; }
        public string exterior { get; set; }
        public string type { get; set; }
        public string subcategory { get; set; }
        public string collection { get; set; }
        public object phase { get; set; }
        public double suggested_price { get; set; }
        public bool is_stattrak { get; set; }
        public string icon { get; set; }
        public string rarity { get; set; }
        public string asset_id { get; set; }
        public bool tradable { get; set; }
        public string link { get; set; }
        public string skip_reason { get; set; }
        public double min_price { get; set; }
        public double max_price { get; set; }
        public List<object> stickers { get; set; }
    }

    public class Metadata
    {
        public int total { get; set; }
    }

}

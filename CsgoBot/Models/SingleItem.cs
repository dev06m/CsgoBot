using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public Item item { get; set; }
        public bool is_seller_online { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public double price { get; set; }
        public double floatvalue { get; set; }
        public int paintindex { get; set; }
        public int paintseed { get; set; }
        public string link { get; set; }
        public string time_created { get; set; }
        public SteamItem steam_item { get; set; }
        public List<object> stickers { get; set; }
    }

    public class SingleItem
    {
        public Data data { get; set; }
        public string status { get; set; }
    }

}

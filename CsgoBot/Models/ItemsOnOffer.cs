using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DatumOffer
    {
        public int id { get; set; }
        public double price { get; set; }
        public double floatvalue { get; set; }
        public object paintindex { get; set; }
        public object paintseed { get; set; }
        public string link { get; set; }
        public string time_created { get; set; }
        public SteamItem steam_item { get; set; }
        public List<object> stickers { get; set; }
        public string asset_id { get; set; }
        public string state { get; set; }
        public double price_with_fee { get; set; }
        public string steamid { get; set; }
        public object custom_id { get; set; }
        public Settings settings { get; set; }
    }

    public class MetadataOffer
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int total { get; set; }
    }

    public class ItemsOnOffer
    {
        public List<DatumOffer> data { get; set; }
        public MetadataOffer metadata { get; set; }
        public string status { get; set; }
    }

    public class Settings
    {
        public object min_price { get; set; }
        public object max_price { get; set; }
        public object manual_lock { get; set; }
    }

    //public class SteamItem
    //{
    //    public int id { get; set; }
    //    public string project { get; set; }
    //    public string steam_market_hash_name { get; set; }
    //    public string exterior { get; set; }
    //    public string type { get; set; }
    //    public string subcategory { get; set; }
    //    public string collection { get; set; }
    //    public object phase { get; set; }
    //    public double suggested_price { get; set; }
    //    public bool is_stattrak { get; set; }
    //    public string icon { get; set; }
    //    public string rarity { get; set; }
    //}




}

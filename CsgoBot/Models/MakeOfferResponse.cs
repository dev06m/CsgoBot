﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Models
{
    public class Datum
    {
        public int id { get; set; }
        public double price { get; set; }
        public int floatvalue { get; set; }
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
    }

    public class MakeOfferResponse
    {
        public List<Datum> data { get; set; }
        public MetadataCancel metadata { get; set; }
        public string status { get; set; }
    }

    public class SteamItem
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
    }

}
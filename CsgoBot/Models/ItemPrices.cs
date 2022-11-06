using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Models
{
    public class PriceDatum
    {
        public string? steam_market_hash_name { get; set; }
        public string? price { get; set; }
        public string? volume { get; set; }
    }

    public class PriceRoot
    {
        public List<PriceDatum>? data { get; set; }
    }

}

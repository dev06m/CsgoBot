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
        public float? price { get; set; }
        public int? volume { get; set; }
        public float? liquidity { get; set; }
        public String? phase { get; set; }
    }

    public class PriceRoot
    {
        public List<PriceDatum>? data { get; set; }
        public string? status { get; set; }
    }

}

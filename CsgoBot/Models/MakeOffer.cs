using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Models
{
    public class Offer
    {
        public string id { get; set; }
        public string price { get; set; }
        public string project { get; set; }
        public string currency { get; set; }
    }

    public class MakeOffer
    {
        public List<Offer> offers { get; set; }
    }
}

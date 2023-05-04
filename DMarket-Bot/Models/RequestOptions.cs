using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMarket_Bot.Models
{
    class RequestOptions
    {
        public string host { get; set; }
        public string path { get; set; }
        public string method { get; set; }
        public Headers headers { get; set; }
    }

    class Headers
    {
        public string XApiKey { get; set; }
        public string XRequestSign { get; set; }
        public string XSignDate { get; set; }
        public string ContentType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Models
{
    public class CancelledItem
    {
        public int id { get; set; }
        public int price { get; set; }
        public int floatvalue { get; set; }
        public object? paintindex { get; set; }
        public object? paintseed { get; set; }
        public string? link { get; set; }
        public string? time_created { get; set; }
        public SteamItem? steam_item { get; set; }
        public List<Sticker>? stickers { get; set; }
        public string? asset_id { get; set; }
        public string? state { get; set; }
        public double? price_with_fee { get; set; }
        public string? steamid { get; set; }
        public object? custom_id { get; set; }
    }

    public class MetadataCancel
    {
        public int total_cancelled_items { get; set; }
        public int total_not_cancelled_items { get; set; }
    }

    public class Root
    {
        public List<CancelledItem>? cancelled_items { get; set; }
        public List<object>? not_cancelled_items { get; set; }
        public List<object>? errors { get; set; }
        public Metadata? metadata { get; set; }
        public string? status { get; set; }
    }

}

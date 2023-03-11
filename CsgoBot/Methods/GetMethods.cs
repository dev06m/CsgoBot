using CsgoBot.Assets;
using CsgoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Methods
{
    static class GetMethods
    {

        private static readonly HttpClient client = new HttpClient();

        public static Inventory GetInventory()
        {
            string accessToken = Isimlendirmeler.ACCESS_TOKEN;
            string path = "https://api.shadowpay.com/api/v2/user/inventory";
            string itemListPath = path + "?token=" + accessToken;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
            request.ContentType = "application/json";

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (response == null)
                return null;
            var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //JObject json = JObject.Parse(content);
            var items = System.Text.Json.JsonSerializer.Deserialize<Inventory>(content);

            return items;
        }



        public static MakeOfferResponse GetItemsOnOffers()
        {
            string itemListPath = "https://api.shadowpay.com/api/v2/user/offers" + "?"  + Isimlendirmeler.ACCESS_TOKEN + "\"";
            MakeOfferResponse inventory = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
                request.ContentType = "application/json";

            
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                inventory = System.Text.Json.JsonSerializer.Deserialize<MakeOfferResponse>(content);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //throw;
            }
            if (inventory != null)
            {
                return inventory;
            }
            return new MakeOfferResponse();
        }

        public static async Task<ItemsOnOffer> SatisListesi()
        {
            string path = "https://api.shadowpay.com/api/v2/user/offers";
            var accessToken = Isimlendirmeler.ACCESS_TOKEN;
            string itemListPath = path + "?token=" + accessToken;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //JObject json = JObject.Parse(content);
            var items = System.Text.Json.JsonSerializer.Deserialize<ItemsOnOffer>(content);

            return items ?? new ItemsOnOffer();
        }


        public static async Task<double> ItemFiyatGetir(String itemName)
        {
            List<PriceDatum> prices = null;
            double lowestPrice = 0;
            try
            {
                var accessToken = Isimlendirmeler.ACCESS_TOKEN;
                string itemListPath = "https://api.shadowpay.com/api/v2/user/items/prices" + "?token=" + accessToken;
                //string itemListPath = "https://api.shadowpay.com/api/v2/user/items?search=" + itemName + "&token=" + accessToken;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //JObject json = JObject.Parse(content);
                prices = System.Text.Json.JsonSerializer.Deserialize<PriceRoot>(content).data;
                PriceDatum item = prices.FirstOrDefault(x => x.steam_market_hash_name == itemName);
                if (item == null)
                    return 0;
                lowestPrice = Convert.ToDouble(item.price, System.Globalization.CultureInfo.InvariantCulture);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //throw;
            }

            return lowestPrice;
        }

        public static List<PriceDatum> TumItemFiyatlariniGetir()
        {
            List<PriceDatum> prices = null;
            double lowestPrice = 0;
            try
            {
                var accessToken = Isimlendirmeler.ACCESS_TOKEN;
                string itemListPath = "https://api.shadowpay.com/api/v2/user/items/prices" + "?token=" + accessToken;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                prices = System.Text.Json.JsonSerializer.Deserialize<PriceRoot>(content).data;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //throw;
            }

            return prices;
        }

        public static object SatistakiItemFiyatiGetir(string itemName)
        {
            MakeOfferResponse satistakiItemler = GetItemsOnOffers();
            Datum itemObject = null;
            try
            {
                itemObject =  satistakiItemler.data.FirstOrDefault(x => x.steam_item.steam_market_hash_name == itemName);

            }
            catch (Exception exp)
            {
                Console.WriteLine("\n{0}\n", exp.Message);
            }
            if(itemObject != null)
                return itemObject.price;

            return null;
        }

        public static string FindNameById(string id)
        {
            var accessToken = Isimlendirmeler.ACCESS_TOKEN;
            string itemListPath = "https://api.shadowpay.com/api/v2/user/items/" + id + "?token=" + accessToken;
            
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //JObject json = JObject.Parse(content);
                var item = System.Text.Json.JsonSerializer.Deserialize<SingleItem>(content);
                if (item.data == null)
                {
                    return "";
                }
                return item.data.item.steam_item.steam_market_hash_name;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool FiyatDegisikligiCheck()
        {
            var accessToken = Isimlendirmeler.ACCESS_TOKEN;
            string itemListPath = "https://api.shadowpay.com/api/v2/user/offers" + "?token=" + accessToken;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //JObject json = JObject.Parse(content);
                var item = System.Text.Json.JsonSerializer.Deserialize<Offers>(content);
                if (item?.data.Count == 0)
                {
                    return;
                }
                // yoksa null cevirir ve uygulama patlar
                return item.data[0].id.ToString(); // degisecek, isme gore bulunabilir olabir
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

    }
}

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
            string accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
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
                return new Inventory();
            var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //JObject json = JObject.Parse(content);
            var items = System.Text.Json.JsonSerializer.Deserialize<Inventory>(content);

            return items;
        }



        public static MakeOfferResponse GetItemsOnOffers()
        {
            string itemListPath = "https://api.shadowpay.com/api/v2/user/offers" + "?token=5694e257ec0dc1ca476024eb5f15ded7";
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
            var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
            string itemListPath = path + "?token=" + accessToken;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //JObject json = JObject.Parse(content);
            var items = System.Text.Json.JsonSerializer.Deserialize<ItemsOnOffer>(content);

            return items ?? new ItemsOnOffer();
        }


        public static async Task<PriceDatum> ItemFiyatGetir(string itemName)
        {
            List<PriceDatum> lowestPriceObject = null;

            try
            {
                var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
                string itemListPath = "https://api.shadowpay.com/api/v2/user/items/prices" + "?token=" + accessToken;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //JObject json = JObject.Parse(content);
                var items = System.Text.Json.JsonSerializer.Deserialize<PriceRoot>(content);

                lowestPriceObject = items.data.Where(x => x.steam_market_hash_name == itemName).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //throw;
            }

            if (lowestPriceObject == null || lowestPriceObject?.Count == 0)
                return new PriceDatum();

            return lowestPriceObject[0];
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
            var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
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

        //public static string FindOfferItemId()
        //{
        //    var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
        //    string itemListPath = "https://api.shadowpay.com/api/v2/user/offers" + "?token=" + accessToken;

        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
        //        request.ContentType = "application/json";

        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //        //JObject json = JObject.Parse(content);
        //        var item = System.Text.Json.JsonSerializer.Deserialize<Offers>(content);
        //        if (item?.data.Count == 0)
        //        {
        //            return;
        //        }
        //        // yoksa null cevirir ve uygulama patlar
        //        return item.data[0].id.ToString(); // degisecek, isme gore bulunabilir olabir
        //    }
        //    catch (Exception exp)
        //    {

        //        throw exp;
        //    }
        //}

    }
}

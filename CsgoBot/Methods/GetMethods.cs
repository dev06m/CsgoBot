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
        private static int bir_saat_bekle = 0;

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
                Console.WriteLine($"Line 35(GetMethids.cs) {e.Message}\n");
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
            string itemListPath = "https://api.shadowpay.com/api/v2/user/offers" + "?token="  + Isimlendirmeler.ACCESS_TOKEN;
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
                Console.WriteLine($"Line 66(GetMethids.cs) {e.Message}\n");
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
                //JObject json = JObject.Parse(content);s
                prices = System.Text.Json.JsonSerializer.Deserialize<PriceRoot>(content).data;
                PriceDatum item = prices.FirstOrDefault(x => x.steam_market_hash_name == itemName);
                if (item == null)
                    return 0;
                lowestPrice = Convert.ToDouble(item.price, System.Globalization.CultureInfo.InvariantCulture);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Line 118(GetMethods.cs) {e.Message}\n");
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
                Console.WriteLine($"Line 144(GetMethods.cs) {e.Message}\n");
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
                //Console.WriteLine(@"\n{0}\n", exp.Message);
                Console.WriteLine($"Line163 {exp.Message}\n");
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

        public static bool FiyatDegisikligiCheck(Datum item)
        {
            bool dongu = true;
            int sabitlenecek_zaman = Isimlendirmeler.SABITLENECEK_ZAMAN;

            var shadowEnDusukFiyat = GetMethods.ItemFiyatGetir(item.steam_item.steam_market_hash_name).Result;
            double itemFiyati = Convert.ToDouble(GetMethods.SatistakiItemFiyatiGetir(item.steam_item.steam_market_hash_name));
            if (itemFiyati == null || itemFiyati == 0)
            {
                Console.WriteLine("İtem fiyatı null ya da 0");
                //dongu = false;
            }

            shadowEnDusukFiyat = shadowEnDusukFiyat != null ? shadowEnDusukFiyat : item.steam_item.suggested_price;
            if (shadowEnDusukFiyat < itemFiyati)
            {
                Console.WriteLine($"DÜŞÜK FİYATLI item tespit edildi, fiyat güncleleniyor. ({item.steam_item.steam_market_hash_name})\n");
                dongu = false;
                item.bir_saat_bekle = 0;
            }
            else if (shadowEnDusukFiyat == itemFiyati)
            {
                //if(item.bir_saat_bekle + 1 % 5 == 0)
                    Console.WriteLine($"İtem fiyatı sitedeki en düşük fiyata eşit - {item.bir_saat_bekle+1}. deneme. __{item.steam_item.steam_market_hash_name}__\n");
                item.bir_saat_bekle = item.bir_saat_bekle + 1;
            }
            if(item.bir_saat_bekle == 100)
            {
                Console.WriteLine($"100 kez fiyat kontrolü yapıldı, fiyat {sabitlenecek_zaman / (1000 * 60)} dk süresince sabitlenecek .");
                Thread.Sleep(sabitlenecek_zaman); // bir süre sabit fiyatla bekle
                Console.WriteLine($"{sabitlenecek_zaman / (1000*60)} dk beklendi, fiyat {item.baslangic_fiyati} $'a setlenecek, sonra güncellenecek.");
                PostMethods.MakeOffer(item, item.baslangic_fiyati.ToString(), item.interval_time); // bir süre bekledikten sonra başlangıc fiyatına setle ve donguden çıkarak fiyatı tekrar setlgüncelle.
                dongu = false;
                item.bir_saat_bekle = 0;
            }
            return dongu;
        }

        

    }
}

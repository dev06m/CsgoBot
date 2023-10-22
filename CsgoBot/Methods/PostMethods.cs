using CsgoBot.Assets;
using CsgoBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CsgoBot.Methods
{
    static class PostMethods
    {
        private static readonly HttpClient client = new HttpClient();

        public static MakeOfferResponse MakeOffer(Datum item, string price, int miliseconds) // update yaparken
        {

            string path = "https://api.shadowpay.com/api/v2/user/offers";
            var accessToken = Isimlendirmeler.ACCESS_TOKEN;
            price = price.Replace(",", ".");
            price = price == "0" ? item.steam_item.suggested_price.ToString() : price;  

            var offersList = new List<OfferUpdate>
            {
                new OfferUpdate
                {
                    id = item.id, // update yaptigimiz icin id olmasi laizm
                    price = double.Parse(price, System.Globalization.CultureInfo.InvariantCulture),
                    project = "csgo",
                    currency = "USD"
                }
            };
            var offerData = new Dictionary<string, List<OfferUpdate>>();
            offerData.Add("offers", offersList);

            var json = JsonConvert.SerializeObject(offerData);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var retry = true;
            
            while (retry)
            {
                var response = new HttpResponseMessage();
                try
                {
                    response = client.PatchAsync(path, stringContent).Result;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"hata var!!!!! {e.Message}\n");
                }

                if (response.IsSuccessStatusCode)
                {
                    retry = false;
                    var contentStream = response.Content.ReadAsStream();

                    using var streamReader = new StreamReader(contentStream);
                    try
                    {
                        var result = streamReader.ReadToEnd();
                        var data = JsonConvert.DeserializeObject<MakeOfferResponse>(result);
                        return data != null ? data : new MakeOfferResponse();
                    }
                    catch (JsonReaderException)
                    {
                        Console.WriteLine("Gecersiz JSON.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }else
                {
                    Console.WriteLine("Fiyat seetleme basarisiz oldu, tekrar denenecek.");
                }
            }
            return null;
        }

        public static String IlkFiyatSetleme(string asset_id, string price, string item_name) // "E" "H" "satildi"
        {
            Console.WriteLine($"İlk fiyat setleme başlıyor {item_name}");
            string path = "https://api.shadowpay.com/api/v2/user/offers";
            var accessToken = Isimlendirmeler.ACCESS_TOKEN;

            Datum item = null;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            StringContent content = ContentProvider(asset_id, price, item);

            var retry = true;
            //while (retry)
            //{
                int count = 0;
                HttpResponseMessage response = null;
                response = client.PostAsync(path, content).Result;
                response.EnsureSuccessStatusCode();

                //if (response.ReasonPhrase == "Unprocessable Entity")
                //{
                //    Console.WriteLine("Unprocessable Entity hatasi olustu, post istegi tekrar gonderiliyor... \n");
                //    //if (count < 5)
                //    //{
                //    //    Console.WriteLine($"{count + 1}. deneme. \n");
                //    //    count++;
                //    //    //continue;
                //    //}
                //    retry = false;
                //    Console.WriteLine($"Post denemeleri basarisiz oldu... \n");
                //}

                if (response.IsSuccessStatusCode)
                {
                    retry = false;
                    var contentStream = response.Content.ReadAsStream();

                    using var streamReader = new StreamReader(contentStream);
                    try
                    {
                        var result = streamReader.ReadToEnd();
                        var data = JsonConvert.DeserializeObject<MakeOfferResponse>(result);
                        if (data.error_message == "inventory_items_mismatch")
                        {
                            Console.WriteLine("İtme bulunamadı, satıldı ya da başka hata var.");
                            return "satildi";
                        }
                        if (data.status == "error" && data.error_message == "user_got_autoban")
                        {
                            Console.WriteLine($"Fiyat setleme başarısız!! error : {result}\n");
                            if (data.error_message == "user_got_autoban")
                                Thread.Sleep(3600000);
                            return "H";
                        }
                        //Worker.itemId = data.data[0].id;
                        Console.WriteLine($"Fiyat setleme başarılı... \n");
                    }
                    catch (JsonReaderException)
                    {
                        Console.WriteLine("Geçersiz JSON.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Fiyat setleme basarisiz oldu, tekrar denenecek.");
                }
            //}
            return "E";
        }



        public static Task<HttpResponseMessage> CancelOffer(string itemId)
        {
            string path = "https://api.shadowpay.com/api/v2/user/offers";
            var accessToken = Isimlendirmeler.ACCESS_TOKEN;
            //var response = string.Empty;
            var cancelItemIds = new List<int>();
             

            if (itemId != null)
            {
                cancelItemIds.Add(Convert.ToInt32(itemId));

            }// null degilse ??

            var cancelList = new Dictionary<string, List<int>>();
            cancelList.Add("item_ids", cancelItemIds);


            
            var stringContent = new StringContent(JsonConvert.SerializeObject(cancelList), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, path + "?token=" + accessToken);
            request.Content = stringContent;
            var response = client.SendAsync(request);

            //var threadId = Thread.CurrentThread.ManagedThreadId;
            ////int threadId = 0xFF;

            //Process currentProcess = Process.GetCurrentProcess();

            //foreach (System.Diagnostics.ProcessThread thread in currentProcess.Threads)
            //{
            //    var a = thread.Id;
            //    if (thread.Id == threadId)
            //    {
            //        // We found you thread! And abort it.
            //        //thread. Abort();
            //    }
            //}

            return response;
            
        }


        //public static bool SetLowestPrice(double? minFiyat, Datum item, int miliseconds)
        //{
        //    double suggestedPrice = item.steam_item.suggested_price;
        //    double? altLimit = 0;

        //    altLimit = minFiyat; // ALT LIMIT AYARLANDI

        //    string itemId = item.asset_id.ToString();

        //    string itemName = item.steam_item.steam_market_hash_name;
        //    if (itemName != null)
        //    {
        //        double lowestPriceObject = GetMethods.ItemFiyatGetir(item.steam_item.steam_market_hash_name).Result;
        //        string lowestPrice = lowestPriceObject != null ? lowestPriceObject.ToString() : Convert.ToString(suggestedPrice);

        //        double doubleLowestPrice = lowestPrice != null ?lowestPriceObject : suggestedPrice;
          
        //        var myItemPrice = item.price;

        //        if ((myItemPrice > doubleLowestPrice || myItemPrice == 0) && doubleLowestPrice >= altLimit) // (item fiyati en dusuk fiyattan fazlaysa ya da item fiyati sifirsa) ve en dusuk fiyat alt limitin altinda degilse
        //        {
        //            Console.WriteLine($"--Fiyat degisikligi '{item.steam_item.steam_market_hash_name}'-- Item fiyatim: ${myItemPrice}, En dusuk fiyat: ${doubleLowestPrice}");
        //            if (myItemPrice > altLimit)
        //            {
        //                var newPrice = doubleLowestPrice - 0.01;
        //                string newPriceString = newPrice.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                        
        //                var result = MakeOffer(item, newPriceString, miliseconds);
        //                Console.WriteLine($"Item fiyati degisti, yeni fiyat: {newPriceString} ---------- {result} \n");
                        
        //            }
        //            else
        //            {
        //                string suggestedPriceString = suggestedPrice.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        //                MakeOffer(item, suggestedPriceString, miliseconds);
        //                Console.WriteLine($"Alt limiti astigi icin tavsiye fiyat ayarlandi.");
        //            }
        //            return true;
        //        }else
        //        {
        //            Console.WriteLine($"Fiyat degisikligi olmadi. -- {itemName} -- \n");
        //        }
        //    }
        //    return false;
        //}

 
        private static StringContent ContentProvider(string asset_id, string price, Datum item)
        {

            List<Offer> offersList = null;
            if(price.Contains(","))
                price = price.Replace(",", ".");
            //double double_price = double.Parse(price, System.Globalization.CultureInfo.InvariantCulture);

            if (item == null)
            {
                offersList = new List<Offer>
                    {
                        new Offer
                        {
                            id = asset_id, // asset id dinamik gelecek
                            price = double.Parse(price, System.Globalization.CultureInfo.InvariantCulture),
                            project = "csgo" ,
                            currency = "USD"
                        }
                    };
            }
            else
            {
                offersList = new List<Offer>
                    {
                        new Offer
                        {
                            id = asset_id, // asset id dinamik gelecek
                            price = double.Parse(price, System.Globalization.CultureInfo.InvariantCulture),
                            currency = "USD"
                        }
                    };

            }
            var offerData = new Dictionary<string, List<Offer>>();
            offerData.Add("offers", offersList);

            using StringContent contentX = new(
               System.Text.Json.JsonSerializer.Serialize(offerData),
               Encoding.UTF8,
               "application/json");


            var stringPayload = JsonConvert.SerializeObject(offerData);

            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            return content;
        }
    }

}

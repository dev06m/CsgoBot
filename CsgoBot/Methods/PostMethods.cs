using CsgoBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Methods
{
    static class PostMethods
    {
        private static readonly HttpClient client = new HttpClient();

        public static MakeOfferResponse MakeOffer(Datum item, string price, int miliseconds)
        {

            string path = "https://api.shadowpay.com/api/v2/user/offers";
            var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";

            var offersList = new List<Offer>
            {
                new Offer
                {
                    id = item.asset_id, // asset id dinamik gelecek
                    price = price,
                    project = "csgo" ,
                    currency = "USD"
                }
            };
            var offerData = new Dictionary<string, List<Offer>>();
            offerData.Add("offers", offersList);

            var json = JsonConvert.SerializeObject(offerData);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            //Eger varsa item satisini iptal et
            Thread.Sleep(miliseconds);
            var cancelResult = CancelOffer(item.id.ToString()).Result;

            if (cancelResult.IsSuccessStatusCode != true)
            {
                Thread.Sleep(1000);
            }

            var retry = true;
            
            while (retry)
            {
                Thread.Sleep(miliseconds);
                var response = client.PostAsync(path, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    retry = false;
                    var contentStream = response.Content.ReadAsStreamAsync().Result;

                    using var streamReader = new StreamReader(contentStream);
                    try
                    {
                        var result = streamReader.ReadToEnd();
                        var data = JsonConvert.DeserializeObject<MakeOfferResponse>(result);
                        return data;
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


        public async static Task<HttpResponseMessage> CancelOffer(string itemId)
        {
            string path = "https://api.shadowpay.com/api/v2/user/offers";
            var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
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
            var response = await client.SendAsync(request);
            return response;
            

            /*
            var request = new HttpRequestMessage(HttpMethod.Delete, path + "?token=" + accessToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(cancelList), Encoding.UTF8, "application/json");
            await client.SendAsync(request);
            */

            /*
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            request.Method = "DELETE";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(cancelList);
            request.ContentLength = data.Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(data);
            requestWriter.Close();

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();

                responseReader.Close();
            }
            catch (Exception ex)
            {

            }

            return response.ToString();
            */
        }


        public async static void SetLowestPrice(Datum item, int miliseconds)
        {
            double suggestedPrice = item.steam_item.suggested_price;
            double altLimit = suggestedPrice - suggestedPrice*0.15;
            string itemId = item.asset_id.ToString();

            //string itemName = GetMethods.FindNameById(itemId);
            string itemName = item.steam_item.steam_market_hash_name;
            if (itemName != null)
            {
                PriceDatum lowestPriceObject = GetMethods.ItemFiyatGetir(itemName).Result;
                string lowestPrice = lowestPriceObject != null ? lowestPriceObject.price : Convert.ToString(suggestedPrice);

                double doubleLowestPrice = lowestPrice != null ? double.Parse(lowestPrice, System.Globalization.CultureInfo.InvariantCulture) : suggestedPrice;
          
                var myItemPrice = item.price;

                if ((myItemPrice > doubleLowestPrice || myItemPrice == 0) && doubleLowestPrice >= altLimit) // (item fiyati en dusuk fiyattan fazlaysa ya da item fiyati sifirsa) ve en dusuk fiyat alt limitin altinda degilse
                {
                    Console.WriteLine($"--Fiyat degisikligi '{item.steam_item.steam_market_hash_name}'-- Item fiyatim: ${myItemPrice}, En dusuk fiyat: ${doubleLowestPrice}");
                    if (myItemPrice > altLimit)
                    {
                        var newPrice = doubleLowestPrice - 0.01;
                        string newPriceString = newPrice.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                        
                        var result = MakeOffer(item, newPriceString, miliseconds);
                        Console.WriteLine($"Item fiyati degisti, yeni fiyat: {newPriceString} ---------- {result}");
                        
                    }
                    else
                    {
                        string suggestedPriceString = suggestedPrice.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                        MakeOffer(item, suggestedPriceString, miliseconds);
                        Console.WriteLine($"Alt limiti astigi icin tavsiye fiyat ayarlandi.");
                    }
                }else
                {
                    Console.WriteLine($"Fiyat degisikligi olmadi.");
                }
            }
        }

        public static async Task<MakeOfferResponse> MakeOffer(string asset_id, string price)
        {
            MakeOfferResponse itemsResult = GetMethods.GetItemsOnOffers();
            if (itemsResult == null)
                return null;

            var item = itemsResult != null ? itemsResult.data.FirstOrDefault(x => x.asset_id == asset_id) : null;
            string path = "https://api.shadowpay.com/api/v2/user/offers";
            var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";

            var offersList = new List<Offer>
            {
                new Offer
                {
                    id = asset_id, // asset id dinamik gelecek
                    price = price,
                    project = "csgo" ,
                    currency = "USD"
                }
            };
            var offerData = new Dictionary<string, List<Offer>>();
            offerData.Add("offers", offersList);

            var content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)offerData);

            //var json = JsonConvert.SerializeObject(offerData);
            //var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            if (item != null)
            {
                var cancelResult = await CancelOffer(item.id.ToString());
            }
            //});
            //if (cancelResult.IsSuccessStatusCode != true)
            //{
            //    Thread.Sleep(1000);
            //}

            var retry = true;
            while (retry)
            {
                int count = 0;
                var response = await client.PostAsync(path, content);
                if (response.ReasonPhrase == "Unprocessable Entity")
                {
                    Console.WriteLine("Unprocessable Entity hatasi olustu, post istegi tekrar gonderiliyor... \n");
                    if (count < 5)
                    {
                        Console.WriteLine($"{count + 1}. deneme. \n");
                        count++;
                        continue;
                    }
                    retry = false;
                    Console.WriteLine($"Post denemeleri basarisiz oldu... \n");
                } 
                if (response.IsSuccessStatusCode)
                {
                    retry = false;
                    var contentStream = await response.Content.ReadAsStreamAsync();

                    using var streamReader = new StreamReader(contentStream);
                    try
                    {
                        var result = streamReader.ReadToEnd();
                        var data = JsonConvert.DeserializeObject<MakeOfferResponse>(result);
                        return data;
                    }
                    catch (JsonReaderException)
                    {
                        Console.WriteLine("Gecersiz JSON.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Fiyat setleme basarisiz oldu, tekrar denenmeyecek.");
                }
            }
            return null;
        }

    }
}

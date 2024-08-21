using CsgoBot.Assets;
using CsgoBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static CsgoBot.Form1;

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

        public static string FiyatDegisikligiCheck(Datum item)
        {
            bool dongu = true;
            int sabitlenecek_zaman = Isimlendirmeler.SABITLENECEK_ZAMAN;

            var shadowEnDusukFiyat = GetMethods.ItemFiyatGetir(item.steam_item.steam_market_hash_name).Result;
            double itemFiyati = 0;
            int count = 0;
            bool dongu_dene = true;
            while (dongu_dene) // eger item fiyati null gelirse(satis olma durumu ya da too may request hatasi) 3 saniyede bir 3 defa tekrar deneyip 5 dk bekleyip tekrar deneyecek ve sonra basatisiz olursa thread kapanacak
            {
                itemFiyati = Convert.ToDouble(GetMethods.SatistakiItemFiyatiGetir(item.steam_item.steam_market_hash_name));
                if (itemFiyati == null || itemFiyati == 0)
                {
                    Console.WriteLine($"İtem fiyatı null ya da 0({item.steam_item.steam_market_hash_name}), 3 saniye sonra tekrar denenecek\n");
                    Thread.Sleep(3000);
                } else if (item != null)
                {
                    dongu_dene = false;
                }
                count++;
                if(count == 3)
                {
                    Console.WriteLine($"İtem fiyatı null ya da 0, 3 saniye sonra tekrar denenecek ({item.steam_item.steam_market_hash_name})\n");
                    Thread.Sleep(30000);
                }else if (count == 4)
                {
                    dongu_dene = false;
                    return item.asset_id;
                }

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
            return "";
        }

        public async static void GetAllDotaItems_Buff163()
        {
            var nesneler = new List<Nesne>();
            string itemListPath = "https://buff.163.com/api/market/goods?game=dota2&page_num=3";
            string dosyaYolu = @"C:\Users\mdeveci\Desktop\BOT-DMarket\DMarket\CsgoBot\Methods\data.json";
            
            for (int j=1; j <= 1220; j = j + 20)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        for (int i = j; i < j+20; i++)
                        {
                            string path = "https://buff.163.com/api/market/goods?game=dota2&page_num=" + i;
                            client.DefaultRequestHeaders.Add("Cookie", "Device-Id=89rhBjowQwPGegiuYxZS; P_INFO=90-5399138096|1704202553|1|netease_buff|00&99|null&null&null#TR&null#10#0|&0||90-5399138096; Locale-Supported=en; session=1-VBNljroky1VN_XHbg63n4vTdHabn_PYgV2qlGWQt9kdc2034331581; game=dota2; csrf_token=IjFkNTY2MmY4YzM1MzI2ZTQ3Njc0ODRmYTM2M2EwYjgxM2I3NGRmYTUi.GQvTOw.GGgl7zdkBHRH9xz6b0Yo_CG9Le4");
                            using (HttpResponseMessage response = await client.GetAsync(path))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    string jsonResult = await response.Content.ReadAsStringAsync();


                                    dynamic veri = JsonConvert.DeserializeObject(jsonResult);
                                    foreach (var item in veri.data.items)
                                    {
                                        int id = item.id;
                                        string name = item.market_hash_name;
                                        int page_num = veri.data.page_num;
                                        nesneler.Add(new Nesne { ID = id, Name = name, PageNum = page_num });
                                    }


                                    //MessageBox.Show(jsonNesneler, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    Console.WriteLine("İstek başarısız. HTTP durum kodu: " + response.StatusCode);
                                    //MessageBox.Show("İstek başarısız. HTTP durum kodu: " + response.StatusCode, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            if (i % 20 == 0)
                                Console.WriteLine(i);
                                
                            Thread.Sleep(7000);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            string jsonNesneler = JsonConvert.SerializeObject(nesneler, Formatting.Indented);
            File.WriteAllText(dosyaYolu, jsonNesneler);
        }
        /*
        public async static void GetAllLisskinHistroy()
        {
            string itemListPath = "https://lis-skins.ru/market/cart/";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Add("Cookie", "Device-Id=89rhBjowQwPGegiuYxZS; P_INFO=90-5399138096|1704202553|1|netease_buff|00&99|null&null&null#TR&null#10#0|&0||90-5399138096; Locale-Supported=en; session=1-VBNljroky1VN_XHbg63n4vTdHabn_PYgV2qlGWQt9kdc2034331581; game=dota2; csrf_token=IjFkNTY2MmY4YzM1MzI2ZTQ3Njc0ODRmYTM2M2EwYjgxM2I3NGRmYTUi.GQvTOw.GGgl7zdkBHRH9xz6b0Yo_CG9Le4");
                    using (HttpResponseMessage response = await client.GetAsync(itemListPath))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResult = await response.Content.ReadAsStringAsync();

                            var nesneler = new List<Nesne>();

                            dynamic veri = JsonConvert.DeserializeObject(jsonResult);
                            foreach (var item in veri.data.items)
                            {
                                int id = item.id;
                                string name = item.market_hash_name;
                                nesneler.Add(new Nesne { ID = id, Name = name });
                            }

                            string dosyaYolu = "data.json";
                            string jsonNesneler = JsonConvert.SerializeObject(nesneler, Formatting.Indented);
                            File.WriteAllText(dosyaYolu, jsonResult);

                            MessageBox.Show(jsonNesneler, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("İstek başarısız. HTTP durum kodu: " + response.StatusCode, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        */
        public async static void GetAllLisskinHistroy()
        {
            string url = "https://lis-skins.ru/profile/?page=1&ajax=1";
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            try
            {
                using (HttpClient client = new HttpClient(clientHandler))
                {
                    client.DefaultRequestHeaders.Add("Cookie", "_ga=GA1.1.1827035189.1714943037; _ym_uid=1714943043877081707; _ym_d=1714943043; _ym_isad=2; remember_web_59ba36addc2b2f9401580f014c7f58ea4e30989d=eyJpdiI6IlBjWTJCSkdFdEd2bTNRM1B4ZytUUFE9PSIsInZhbHVlIjoiTFRTZFdndURiSW95NUh6MkgzbHFMU2tDY1lCbm0yS1h1bnFIYU95THNIMkNCeXNvTFVzV2FVbDJTK1NhUTVpL0t5Mkkwc3h0dll0bEtEZFgweHM5TCsrMGo2TmF5UEFMQ1AxbFpVRE1SZW9Kd3VIZTNYUVdPUlVCQUF6Vitvc29XL2FZYnQ3Kzg3cncvd245T3hkaThnPT0iLCJtYWMiOiI0MzJmYTAyZjJiNzkzODJiNmVmNWUxMzgwZWE0MDc0MGU3Njk4MTYyN2RmMTFlMTU2NjNiOTcwYjgzOGViMzNjIiwidGFnIjoiIn0%3D; lsuserid=1281646; firstLogin=20230412; user_saved_game=dota2; view_obtained_skin_id=98214433; __cf_bm=OTUxkoiM2_35N29kIkgFEOqi3g5VfUp9F52bkghsCz4-1714949038-1.0.1.1-cbTjF6muN9I4DPFHLrUWo56_J3H25SM59uQxTwPMqoFLfGuU17ViidCQjOfNPSkDkdfMLXPHFPJxaZly1XEetQ; _ym_visorc=b; XSRF-TOKEN=eyJpdiI6ImZ6ditqUE5MU2tyVE51NTd0cURUYVE9PSIsInZhbHVlIjoiZjg3U1BnWjErZVhqQ1BmRzJPeDBMUVVyUjJYZ1FHVUt2dU9qMHREQy9mQ2oyZC91c0duL05aNU9hWXpUTStudzlBYW8waGZMWUVaNkFLMzkzbTMrNEtMd2JlaWd1bnQ2L0FjcHdxWGs1SnBVQit4eDJ6R1NtdnFVOSthTmxaM20iLCJtYWMiOiJlNDk4MGRlYjcwMTEwOGI4NjM0YjdkMjY5MTJlZjQzMWZmZWJmZGIyYzhiYjRjNDhkZGUxMzIzMGZhNDhlZmJmIiwidGFnIjoiIn0%3D; lis_skins_session=eyJpdiI6IjAxK3c5VG5zZzJocEZVTVBsZDVkT0E9PSIsInZhbHVlIjoiU2ZYdjBXZU1UbE9SYlU1ZHIzN0NLdCtiZUsvdmFrY1JUbjI5QytjdDNvNDdETTlyaTgxYXlWdVZibkx5QXR0MlZZVUxZd3BHMUJZenB1N3JDZWVUQUoyelRIL3g5Zit0eXZKeGxWOVU3NkdJMnFBTVYwYmhnd1NBWkxxSXVVczAiLCJtYWMiOiI5YjZhMGNlM2Q5OTVkMzg5MjMxNTlhZTRkYjRhNWE3N2YyOTBjNmM0ZmQ4OTZhZDRhY2M1NmNkNDNiNDM3MTk5IiwidGFnIjoiIn0%3D; _ga_GS4REXZLTV=GS1.1.1714949075.2.1.1714949340.48.0.0 ");
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    //client.DefaultRequestHeaders.Add("Cookie", "_ga=GA1.1.1827035189.1714943037; _ym_uid=1714943043877081707; _ym_d=1714943043; _ym_isad=2; remember_web_59ba36addc2b2f9401580f014c7f58ea4e30989d=eyJpdiI6IlBjWTJCSkdFdEd2bTNRM1B4ZytUUFE9PSIsInZhbHVlIjoiTFRTZFdndURiSW95NUh6MkgzbHFMU2tDY1lCbm0yS1h1bnFIYU95THNIMkNCeXNvTFVzV2FVbDJTK1NhUTVpL0t5Mkkwc3h0dll0bEtEZFgweHM5TCsrMGo2TmF5UEFMQ1AxbFpVRE1SZW9Kd3VIZTNYUVdPUlVCQUF6Vitvc29XL2FZYnQ3Kzg3cncvd245T3hkaThnPT0iLCJtYWMiOiI0MzJmYTAyZjJiNzkzODJiNmVmNWUxMzgwZWE0MDc0MGU3Njk4MTYyN2RmMTFlMTU2NjNiOTcwYjgzOGViMzNjIiwidGFnIjoiIn0%3D; lsuserid=1281646; firstLogin=20230412; user_saved_game=dota2; view_obtained_skin_id=98214433; __cf_bm=OTUxkoiM2_35N29kIkgFEOqi3g5VfUp9F52bkghsCz4-1714949038-1.0.1.1-cbTjF6muN9I4DPFHLrUWo56_J3H25SM59uQxTwPMqoFLfGuU17ViidCQjOfNPSkDkdfMLXPHFPJxaZly1XEetQ; _ym_visorc=b; XSRF-TOKEN=eyJpdiI6ImZ6ditqUE5MU2tyVE51NTd0cURUYVE9PSIsInZhbHVlIjoiZjg3U1BnWjErZVhqQ1BmRzJPeDBMUVVyUjJYZ1FHVUt2dU9qMHREQy9mQ2oyZC91c0duL05aNU9hWXpUTStudzlBYW8waGZMWUVaNkFLMzkzbTMrNEtMd2JlaWd1bnQ2L0FjcHdxWGs1SnBVQit4eDJ6R1NtdnFVOSthTmxaM20iLCJtYWMiOiJlNDk4MGRlYjcwMTEwOGI4NjM0YjdkMjY5MTJlZjQzMWZmZWJmZGIyYzhiYjRjNDhkZGUxMzIzMGZhNDhlZmJmIiwidGFnIjoiIn0%3D; lis_skins_session=eyJpdiI6IjAxK3c5VG5zZzJocEZVTVBsZDVkT0E9PSIsInZhbHVlIjoiU2ZYdjBXZU1UbE9SYlU1ZHIzN0NLdCtiZUsvdmFrY1JUbjI5QytjdDNvNDdETTlyaTgxYXlWdVZibkx5QXR0MlZZVUxZd3BHMUJZenB1N3JDZWVUQUoyelRIL3g5Zit0eXZKeGxWOVU3NkdJMnFBTVYwYmhnd1NBWkxxSXVVczAiLCJtYWMiOiI5YjZhMGNlM2Q5OTVkMzg5MjMxNTlhZTRkYjRhNWE3N2YyOTBjNmM0ZmQ4OTZhZDRhY2M1NmNkNDNiNDM3MTk5IiwidGFnIjoiIn0%3D; _ga_GS4REXZLTV=GS1.1.1714949075.2.1.1714949340.48.0.0 ");
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
                    client.DefaultRequestHeaders.Add("Origin", "https://lis-skins.ru");
                    client.DefaultRequestHeaders.Add("Priority", "u=1, i");
                    client.DefaultRequestHeaders.Add("Referer", "https://lis-skins.ru/profile/inventory/");
                    client.DefaultRequestHeaders.Add("Sec-Ch-Ua", "\"Chromium\";v=\"124\", \"Microsoft Edge\";v=\"124\", \"Not-A.Brand\";v=\"99\"");
                    client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Mobile", "?0");
                    client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
                    client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
                    client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
                    client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36 Edg/124.0.0.0");
                    client.DefaultRequestHeaders.Add("X-Csrf-Token", "ewbl5dOMF9rnkmpLX1Wi4H2ZaIBOgQ5UhRO2ysSr");
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResult = await response.Content.ReadAsStringAsync();

                            var nesneler = new List<Nesne>();

                            dynamic veri = JsonConvert.DeserializeObject(jsonResult);
                            //foreach (var item in veri.data.items)
                            //{
                            //    int id = item.id;
                            //    string name = item.market_hash_name;
                            //    nesneler.Add(new Nesne { ID = id, Name = name });
                            //}

                            //string dosyaYolu = "data.json";
                            string jsonNesneler = JsonConvert.SerializeObject(nesneler, Formatting.Indented);
                            //File.WriteAllText(dosyaYolu, jsonResult);

                            MessageBox.Show(jsonNesneler, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("İstek başarısız. HTTP durum kodu: " + response.StatusCode, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

    }
}

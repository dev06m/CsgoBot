using CsgoBot.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace CsgoBot
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private void GetInventoryItems_Click(object sender, EventArgs e)
        {
            //string accessToken = Environment.GetEnvironmentVariable("AccessKey");
            //string path = Environment.GetEnvironmentVariable("GetInventoryPath");
            string accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
            string path = "https://api.shadowpay.com/api/v2/user/inventory";
            Inventory inventory = GetInventory(accessToken, path);

            if (inventory == null)
                return;

            //tabloyu temizle
            CleanRows();

            dataGridView1.Visible = true;
            dataGridView1.Size = new System.Drawing.Size(1200, 1200);

            //Kullanıcıya yeni kayıt ekleme izni.
            dataGridView1.AllowUserToAddRows = true;

            //Kullanıcıya kayıt silme izni.
            dataGridView1.AllowUserToDeleteRows = true;

            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //DataGridView sütun oluşturma
            dataGridView1.ColumnCount = 9;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "Name";
            dataGridView1.Columns[2].Name = "Suggested Price";
            dataGridView1.Columns[3].Name = "Type";
            dataGridView1.Columns[4].Name = "Rarity";
            dataGridView1.Columns[5].Name = "Asset Id";
            dataGridView1.Columns[6].Name = "Tradable";
            dataGridView1.Columns[7].Name = "Min Price";
            dataGridView1.Columns[8].Name = "Max Price";

            foreach (var item in inventory.data)
            {
                string[] row = new string[] { item.id.ToString(), item.steam_market_hash_name,
                                              item.suggested_price.ToString(), item.type,
                                              item.rarity, item.asset_id, item.tradable.ToString(),
                                              item.min_price.ToString(), item.max_price.ToString() };
                dataGridView1.Rows.Add(row);
            }

            //Kullanıcıya yeni kayıt ekleme izni.
            dataGridView1.AllowUserToAddRows = true;

            //Kullanıcıya kayıt silme izni.
            dataGridView1.AllowUserToDeleteRows = true;

            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


        private async void FiyatAyarlaButton(object sender, EventArgs e)
        {
            MakeOfferResponse res = null;
            await Task.Run(() =>
            {

                res = MakeOffer2("https://api.shadowpay.com/api/v2/user/offers").Result;
            });

            //var res = MakeOffer("https://api.shadowpay.com/api/v2/user/offers");

            //tabloyu temizle
            CleanRows();

            dataGridView1.Visible = true;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.Size = new System.Drawing.Size(2000, 1200);


            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //DataGridView sütun oluşturma
            dataGridView1.ColumnCount = 8;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "Name";
            dataGridView1.Columns[2].Name = "Price";
            dataGridView1.Columns[3].Name = "Time Created";
            dataGridView1.Columns[4].Name = "Asset Id";
            dataGridView1.Columns[5].Name = "State";
            dataGridView1.Columns[6].Name = "Price with Fee";
            dataGridView1.Columns[7].Name = "Steam Id";

            foreach (var item in res.data)
            {
                string[] row = new string[] { item.id.ToString(), item.steam_item.steam_market_hash_name,
                                              item.price.ToString(), item.time_created,
                                              item.asset_id, item.state,
                                              item.price_with_fee.ToString(), item.steamid };
                dataGridView1.Rows.Add(row);
            }

            //Kullanıcıya yeni kayıt ekleme izni.
            dataGridView1.AllowUserToAddRows = true;

            //Kullanıcıya kayıt silme izni.
            dataGridView1.AllowUserToDeleteRows = true;

            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


        private async void CancelOfferButton(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                var cancelResult = CancelOffer("https://api.shadowpay.com/api/v2/user/offers");
            });
            //return "bang bang";
            var something = 5;
        }

        private async void SatisListesiButton(object sender, EventArgs e)
        {
            ItemsOnOffer res = null;
            await Task.Run(() =>
            {

                res = SatisListesi("https://api.shadowpay.com/api/v2/user/offers").Result;
            });

            //var res = MakeOffer("https://api.shadowpay.com/api/v2/user/offers");

            //tabloyu temizle
            CleanRows();

            dataGridView1.Visible = true;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.Size = new System.Drawing.Size(1200, 800);


            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //DataGridView sütun oluşturma
            dataGridView1.ColumnCount = 10;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "Name";
            dataGridView1.Columns[2].Name = "Price";
            dataGridView1.Columns[3].Name = "Time Created";
            dataGridView1.Columns[4].Name = "Asset Id";
            dataGridView1.Columns[5].Name = "State";
            dataGridView1.Columns[6].Name = "Price with Fee";
            dataGridView1.Columns[7].Name = "Steam Id";
            dataGridView1.Columns[8].Name = "Min Price";
            dataGridView1.Columns[9].Name = "Max Price";

            foreach (var item in res.data)
            {
                string[] row = new string[] { item.id.ToString(), item.steam_item.steam_market_hash_name,
                                              item.price.ToString(), item.time_created,
                                              item.asset_id, item.state,
                                              item.price_with_fee.ToString(), item.steamid, 
                                              //item.settings.min_price.ToString(), item.settings.max_price.ToString()
                                            };
                dataGridView1.Rows.Add(row);
            }

            //Kullanıcıya yeni kayıt ekleme izni.
            dataGridView1.AllowUserToAddRows = true;

            //Kullanıcıya kayıt silme izni.
            dataGridView1.AllowUserToDeleteRows = true;

            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private async void itemlerinFiyatiniGetirButton(object sender, EventArgs e)
        {
            ItemsOnOffer res = null;
            await Task.Run(() =>
            {

                res = itemlerinFiyatiniGetirGetir("https://api.shadowpay.com/api/v2/user/items/prices").Result;
            });

        }


        public Inventory GetInventory(string accessToken, string path)
        {
            string itemListPath = path + "?token=" + accessToken;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //JObject json = JObject.Parse(content);
            var items = System.Text.Json.JsonSerializer.Deserialize<Inventory>(content);

            return items;
        }

        private async Task<MakeOfferResponse> MakeOffer2(string path)
        {

            var itemPrice = textBox2.Text;
            var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
            //var itemId = GetItemId();
            var offersList = new List<Offer>
            {
                new Offer
                {
                    id = "26762748734",
                    price = itemPrice.ToString() ,
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

            // Eger varsa item satisini iptal et
            await Task.Run(() =>
            {
                var cancelResult = CancelOffer("https://api.shadowpay.com/api/v2/user/offers");
            });


            var response = await client.PostAsync(path, stringContent);

            if (response.IsSuccessStatusCode)
            {
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
            return null;
        }

        private string CancelOffer(string path)
        {
            var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
            var response = string.Empty;
            var cancelItemIds = new List<int>();
            int itemId = 0;
            var resultItem = GetItemsOnOffers().data.Count;
            if (resultItem > 0)
                itemId = GetItemsOnOffers().data[0].id;
            else
                return null;
            
            cancelItemIds.Add(itemId);

            // 
            var cancelList = new Dictionary<string, List<int>>();
            cancelList.Add("item_ids", cancelItemIds);

            string URL = "http://localhost:xxxxx/api/values";
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
            // response string seklinde CanceledItem a cevirmen lazim
            return response.ToString();
        }

        private MakeOfferResponse GetItemsOnOffers()
        {
            //if (textBox1.Text != "")
            //    itemName = textBox1.Text;

            string itemListPath = "https://api.shadowpay.com/api/v2/user/offers" + "?token=5694e257ec0dc1ca476024eb5f15ded7";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
            
            MakeOfferResponse inventory = System.Text.Json.JsonSerializer.Deserialize<MakeOfferResponse>(content);
            //Inventory pricesByName = itemPrices.data.FindAll(x => x.steam_market_hash_name == itemName).ToList();

            //if (inventory.data.Count > 0)
            //{
            //    return inventory;
            //}
            //var newInventory = new Inventory();
            return inventory;

        }

        private async Task<ItemsOnOffer> SatisListesi(string path)
        {
            var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
            string itemListPath = path + "?token=" + accessToken;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //JObject json = JObject.Parse(content);
            var items = System.Text.Json.JsonSerializer.Deserialize<ItemsOnOffer>(content);

            return items;
        }

        private void itemlerinFiyatiniGetirGetir(string path)
        {
            var accessToken = "5694e257ec0dc1ca476024eb5f15ded7";
            string itemListPath = path + "?token=" + accessToken;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@itemListPath);
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //JObject json = JObject.Parse(content);
            var items = System.Text.Json.JsonSerializer.Deserialize<ItemsOnOffer>(content);

            return items;
        }

        private void CleanRows()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

    }
}
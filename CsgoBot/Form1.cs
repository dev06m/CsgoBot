using CsgoBot.Methods;
using CsgoBot.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using CsgoBot.Methods;

namespace CsgoBot
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Selam Dunyali");
            //Thread th = new Thread(() =>
            //{
            //    int count = 0;
            //    while (true)
            //    {
            //        SetLowestPrice();
            //        Console.WriteLine("{0} Selam Dunya", count);
            //        count++;
            //    }
            //});
            //th.Start();
        }

        private void GetInventoryItems_Click(object sender, EventArgs e)
        {
            
            //string accessToken = Environment.GetEnvironmentVariable("AccessKey");
            //string path = Environment.GetEnvironmentVariable("GetInventoryPath");
            string path = "https://api.shadowpay.com/api/v2/user/inventory";
            Inventory inventory = CsgoBot.Methods.GetMethods.GetInventory();

            if (inventory == null)
                return;

            //tabloyu temizle
            CleanRows();

            dataGridView1.Visible = true;
            dataGridView1.Size = new System.Drawing.Size(1400, 1400);

            DataGridViewButtonColumn itemOpen = new DataGridViewButtonColumn();
            itemOpen.Name = "Item";
            itemOpen.Text = "Item";

            dataGridView1.CellClick += Temp.random_click;

            //Kullanıcıya yeni kayıt ekleme izni.
            dataGridView1.AllowUserToAddRows = true;

            //Kullanıcıya kayıt silme izni.
            dataGridView1.AllowUserToDeleteRows = true;

            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //DataGridView sütun oluşturma
            dataGridView1.ColumnCount = 10;
            dataGridView1.Columns[0].Name = "Item";
            dataGridView1.Columns[1].Name = "ID";
            dataGridView1.Columns[2].Name = "Name";
            dataGridView1.Columns[3].Name = "Suggested Price";
            dataGridView1.Columns[4].Name = "Type";
            dataGridView1.Columns[5].Name = "Rarity";
            dataGridView1.Columns[6].Name = "Asset Id";
            dataGridView1.Columns[7].Name = "Tradable";
            dataGridView1.Columns[8].Name = "Min Price";
            dataGridView1.Columns[9].Name = "Max Price";

            int count = 1;
            var tradableItems = inventory.data.Where(x => x.tradable == true);
            foreach (var item in tradableItems)
            {
                if (dataGridView1.Columns["item"] == null)
                {
                    dataGridView1.Columns.Insert(count, itemOpen);
                }


                string[] row = new string[] { count.ToString(), item.id.ToString(), item.steam_market_hash_name,
                                              item.suggested_price.ToString(), item.type,
                                              item.rarity, item.asset_id, item.tradable.ToString(),
                                              item.min_price.ToString(), item.max_price.ToString() };
                dataGridView1.Rows.Add(row);

                count++;
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
            string price = "0";
            MakeOfferResponse res = new MakeOfferResponse();
            await Task.Run(() =>
            {

                //res = Methods.PostMethods.MakeOffer("0", textBox2.Text).Result;
            });


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

            if (res.data == null)
                res.data = new List<Datum>();

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
            string cancelItemId = cancelItemBox.Text;
            await Task.Run(() =>
            {
                var cancelResult = Methods.PostMethods.CancelOffer(cancelItemId);
            });
            //return "bang bang";
            var something = 5;
        }

        private async void SatisListesiButton(object sender, EventArgs e)
        {
            ItemsOnOffer res = null;
            await Task.Run(() =>
            {

                res = Methods.GetMethods.SatisListesi().Result;
            });


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
            PriceDatum item = new PriceDatum();
            await Task.Run(() =>
            {
                string itemName = Methods.GetMethods.FindNameById(textBox1.Text);
                item = Methods.GetMethods.ItemFiyatGetir(itemName).Result;
            });

            //tabloyu temizle
            CleanRows();

            dataGridView1.Visible = true;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.Size = new System.Drawing.Size(1200, 800);


            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //DataGridView sütun oluşturma
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Steam Market Hash Name";
            dataGridView1.Columns[1].Name = "Price";
            dataGridView1.Columns[2].Name = "Volume";

            if (item != null)
            {
                string[] row = new string[] { item.steam_market_hash_name, item.price,
                                              item.volume
                                            };
                dataGridView1.Rows.Add(row);
            }
        }



        private void CleanRows()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string itemId = hileTextBox.Text;
            // item id sini FindOfferItem ile buluyor
            itemId = GetMethods.FindOfferItemId();
            if (itemId != null)
            {
                //PostMethods.SetLowestPrice(itemId);

            }
        }
   

        public void MainMethod()
        {
            Console.WriteLine("I GOT A CAR!!!!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void satisIdGetir(object sender, EventArgs e)
        {
            satisIdText.Text = "empty";
            string satisId = GetMethods.FindOfferItemId();
            satisIdText.Text = satisId;
        }

        //private void random_click(object sender, DataGridViewCellEventArgs e)
        //{
        //    Console.WriteLine("I am clicked!!");
        //}
    }
}
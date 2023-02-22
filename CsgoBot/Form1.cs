using CsgoBot.Methods;
using CsgoBot.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using CsgoBot.Methods;
using System;

namespace CsgoBot
{
    public partial class Form1 : Form
    {
        public List<Datum> SeciliItemler = new List<Datum>();
        private static readonly HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Selam Dunyali");
        }

        private void GetInventoryItems_Click(object sender, EventArgs e)
        {
            //tabloyu temizle
            CleanRows();
            
            Inventory inventory = CsgoBot.Methods.GetMethods.GetInventory();

            if (inventory.data == null)
                return;

            if (inventory.data.Count < 1)
            {
                inventory.data = GenerateInventoryItems();
            }

            dataGridView1.Visible = true;
            dataGridView1.Size = new System.Drawing.Size(1200, 1400);
            dataGridView1.AutoSize = true;
            dataGridView1.ScrollBars = ScrollBars.Vertical;

            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "Sat";
            checkColumn.HeaderText = "Sat";
            checkColumn.Width = 65;
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 10; //if the datagridview is resized (on form resize) the checkbox won't take up too much; value is relative to the other columns' fill values
           
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "Asset Id";
            dataGridView1.Columns[2].Name = "Tavsiye fiyat";
            dataGridView1.Columns[3].Name = "Başlangıc Fiyatı";
            dataGridView1.Columns[4].Name = "Minimum fiyat";
            dataGridView1.Columns[5].Name = "Interval time(in ms)";

            dataGridView1.Columns[0].Width = 350;
            int count = 1;

            var tradableItems = inventory.data.Where(x => x.tradable == true);
            dataGridView1.Columns.Add(checkColumn); // bunun altına dıger butonlar eklenecek
            foreach (var item in tradableItems)
            {
                string[] row = new string[] { item.steam_market_hash_name,
                                              item.asset_id,
                                              item.suggested_price.ToString()};
                dataGridView1.Rows.Add(row);

                count++;
            }

            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }



        private async void SatisListesiButton(object sender, EventArgs e)
        {
            ItemsOnOffer res = Methods.GetMethods.SatisListesi().Result;
            
            //tabloyu temizle
            CleanRows();

            dataGridView1.Visible = true;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.Size = new System.Drawing.Size(1200, 800);


            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //DataGridView sütun oluşturma
            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Name = "İsim";
            dataGridView1.Columns[1].Name = "Fiyat";
            dataGridView1.Columns[2].Name = "Komisyon"; // *
            dataGridView1.Columns[3].Name = "Komisyon çıkınca Fiyat";
            dataGridView1.Columns[4].Name = "Baslangıç Fiyatı";
            dataGridView1.Columns[5].Name = "Minimum Fiyat";
            dataGridView1.Columns[6].Name = "Interval";

            dataGridView1.Columns[0].Width = 350;

            List<DatumOffer> satistakiItemler = res?.data;

            if (SeciliItemler != null)
                foreach (var item in SeciliItemler)
                {
                    foreach (var satisItem in satistakiItemler)
                    {
                        if (item.asset_id.Equals(satisItem.asset_id))
                        {
                            satisItem.interval_time = item.interval_time;
                            satisItem.minimum_fiyat = item.minimum_fiyat;
                            satisItem.baslangic_fiyati = item.baslangic_fiyati;
                        }
                    }
                }
           
            foreach (var item in satistakiItemler)
            {
                string[] row = new string[] { item.steam_item.steam_market_hash_name,
                                              item.price.ToString(), 
                                              (item.price - item.price_with_fee).ToString(),
                                              item.price_with_fee.ToString(),
                                              item.baslangic_fiyati.ToString(), 
                                              item.minimum_fiyat.ToString(),
                                              item.interval_time.ToString(), 
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


        private void baslat_click(object sender, EventArgs e)
        {
            string path = "https://api.shadowpay.com/api/v2/user/inventory";

            List<Datum> datumList = new List<Datum>();

            // her bir satiri rows degikenine assign ediyoryz
            var rows = dataGridView1.Rows;

            // for dongusunde her bir satiri datum objesine donusturup datumlist listesine ekliyoruz
            foreach (DataGridViewRow row in rows)
            {
                if (row.Cells[6].Value == null)
                    continue; 
                var isSuggestedNull = row.Cells[4].Value == null;
                var isTimeIntervalNull = row?.Cells[5].Value == null;

                string itemName = row.Cells[0].Value.ToString();
                string assetId = row.Cells[1].Value.ToString();
                double suggestedPrice = Convert.ToDouble(row.Cells[2].Value.ToString());
                double baslangicFiyati = row.Cells[3].Value == null ? suggestedPrice : Convert.ToDouble(row.Cells[3].Value.ToString());
                double minimumFiyat = row.Cells[4].Value == null ? baslangicFiyati - (baslangicFiyati * 0.07) : Convert.ToDouble(row.Cells[4].Value);
                int miliseconds = row.Cells[5].Value == null ? 1000 : Convert.ToInt32(row.Cells[5].Value.ToString()); // INTERVAL0 YENIDEN SETLENMELI;
                //...
                Datum datum = new Datum()
                {
                    steam_item = new SteamItem ()
                    {
                        steam_market_hash_name = itemName,
                        suggested_price = suggestedPrice,
                        
                    },
                    asset_id = assetId,
                    minimum_fiyat = minimumFiyat,
                    baslangic_fiyati = baslangicFiyati,
                    interval_time = miliseconds
                };
                datumList.Add(datum);
                SeciliItemler.AddRange(datumList);
            }
            // tek bir ya da coklu thread olarak worker_thread metoduyla calistiriyoruz
            Worker.worker_threads(datumList);

        }
        private void CleanRows()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
        }


        private List<InventoryItem> GenerateInventoryItems()
        {
            List<InventoryItem> items = new List<InventoryItem>();
            items = new List<InventoryItem>
                    {
                        new InventoryItem
                        {
                            steam_market_hash_name = "denem1",
                            suggested_price = 5.5,
                            asset_id = "12345",
                            tradable = true
                        },
                        new InventoryItem
                        {
                            steam_market_hash_name = "denem2",
                            suggested_price = 6.5,
                            asset_id = "12345",
                            tradable = true
                        },
                        new InventoryItem
                        {
                            steam_market_hash_name = "denem3",
                            suggested_price = 7.5,
                            asset_id = "12345",
                            tradable = true
                        },
                    };

            return items;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PostMethods.MakeOffer();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
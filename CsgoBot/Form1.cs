using CsgoBot.Methods;
using CsgoBot.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using CsgoBot.Methods;
using System;
using System.Security;
using System.ComponentModel;
using CsgoBot.Assets;

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

            if (inventory == null)
            {
                Console.WriteLine("Envanter bos geliyor");
                return;
            }

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

            dataGridView1.ColumnCount = 9;
            dataGridView1.Columns[KolonIsimleri.AD].Name = "İsim";
            dataGridView1.Columns[KolonIsimleri.ASSET_ID].Name = "Asset Id";
            dataGridView1.Columns[KolonIsimleri.TAVSIYE_FIYAT].Name = "Tavsiye fiyat";
            dataGridView1.Columns[KolonIsimleri.SITE_SATIS_FIYATI].Name = "Site Satış Fiyatı";
            dataGridView1.Columns[KolonIsimleri.BASLANGIC_FIYATI].Name = "Başlangıc Fiyatı";
            dataGridView1.Columns[KolonIsimleri.MINIMUM_FIYAT].Name = "Minimum fiyat";
            dataGridView1.Columns[KolonIsimleri.FIYAT_KONTROL_ARALIGI].Name = "Fiyat Kontrol Araligi(in ms)";

            dataGridView1.Columns[KolonIsimleri.AD].Width = 350;
            dataGridView1.Columns[KolonIsimleri.BASLANGIC_FIYATI].Width = 100;
            dataGridView1.Columns[KolonIsimleri.MINIMUM_FIYAT].Width = 100;
            int count = 1;

            // sort by column
            dataGridView1.Sort(dataGridView1.Columns["Tavsiye fiyat"], ListSortDirection.Descending);


            var tradableItems = inventory.data.Where(x => x.tradable == true);
            dataGridView1.Columns.Add(checkColumn); // bunun altına dıger butonlar eklenecek

            var itemFiyatlari = GetMethods.TumItemFiyatlariniGetir();

            foreach (var item in tradableItems)
            {
                //var itemName = GetMethods.ItemFiyatGetir(item.steam_market_hash_name);
                string[] row = new string[] { item.steam_market_hash_name,
                                              item.asset_id,
                                              item.suggested_price.ToString(),
                                              itemFiyatlari != null ?
                                              itemFiyatlari.FirstOrDefault(x => x.steam_market_hash_name == item.steam_market_hash_name)?.price :
                                              "İtem Fiyatı Bulunamadı"};
                dataGridView1.Rows.Add(row);

                count++;
            }

          
            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }



        private void SatisListesiButton(object sender, EventArgs e)
        {
            ItemsOnOffer res = new ItemsOnOffer();
            try
            {
                    res = Methods.GetMethods.SatisListesi().Result;

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                return;
            }
            
            //tabloyu temizle
            CleanRows();

            dataGridView1.Visible = true;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.Size = new System.Drawing.Size(1800, 1000);

            DataGridViewButtonColumn cancelItem = new DataGridViewButtonColumn();
            DataGridViewButtonColumn itemiGuncelle = new DataGridViewButtonColumn();
            
            cancelItem.Name = Isimlendirmeler.SATIS_IPTAL;
            cancelItem.Text = "X";

            itemiGuncelle.Name = "Satış Güncelle";

            int columnIndex = 2;

            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "Sat";
            checkColumn.HeaderText = "Sat";
            checkColumn.Width = 65;
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 10;

            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //DataGridView sütun oluşturma
            dataGridView1.ColumnCount = 8;

            dataGridView1.Columns[KolonIsimleri.AD].Name = "İsim";
            dataGridView1.Columns[KolonIsimleri.ASSET_ID].Name = "Asset Id";
            dataGridView1.Columns[KolonIsimleri.TAVSIYE_FIYAT].Name = "Tavsiye fiyat";
            dataGridView1.Columns[KolonIsimleri.SITE_SATIS_FIYATI].Name = "Site F. - Kendi F.";
            dataGridView1.Columns[KolonIsimleri.BASLANGIC_FIYATI].Name = "Başlangıc Fiyatı";
            dataGridView1.Columns[KolonIsimleri.MINIMUM_FIYAT].Name = "Minimum fiyat";
            dataGridView1.Columns[KolonIsimleri.FIYAT_KONTROL_ARALIGI].Name = "Kontrol Aralığı";
            dataGridView1.Columns[KolonIsimleri.ITEM_ID].Name = "ITEM ID";

            dataGridView1.Columns[KolonIsimleri.AD].Width = 350;
            dataGridView1.Columns[KolonIsimleri.TAVSIYE_FIYAT].Width = 80;
            dataGridView1.Columns[KolonIsimleri.SITE_SATIS_FIYATI].Width = 120;
            dataGridView1.Columns[KolonIsimleri.BASLANGIC_FIYATI_SATIS_LISTESI].Width = 80;
            dataGridView1.Columns[KolonIsimleri.MINIMUM_FIYAT_SATIS_LISTESI].Width = 80;
            dataGridView1.Columns[KolonIsimleri.FIYAT_KONTROL_ARALIGI].Width = 85;

            dataGridView1.Columns.Insert(KolonIsimleri.SATIS_IPTAL_ET, cancelItem);
            //dataGridView1.Columns.Insert(KolonIsimleri.SATIS_GUNCELLE, itemiGuncelle);

            // check box
            dataGridView1.Columns.Add(checkColumn);

            dataGridView1.Columns[0].Width = 350;

            List<DatumOffer> satistakiItemler = res?.data;

            if (SeciliItemler != null) // buraya bida bak
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


            var itemFiyatlari = GetMethods.TumItemFiyatlariniGetir();

            foreach (var item in satistakiItemler)
            {
                string[] row = new string[] {
                                              item.steam_item.steam_market_hash_name,
                                              item.asset_id,
                                              item.steam_item.suggested_price.ToString(),
                                               itemFiyatlari != null ?
                                              itemFiyatlari.FirstOrDefault(x => x.steam_market_hash_name == item.steam_item.steam_market_hash_name).price  + "  -  " + item.price:
                                              "İtem Fiyatı Bulunamadı",

                                              item.baslangic_fiyati.ToString(),
                                              item.minimum_fiyat.ToString(),
                                              item.interval_time.ToString(),
                                              item.id.ToString()
                //item.steam_item.steam_market_hash_name,
                //item.price.ToString(),
                //(item.price - item.price_with_fee).ToString(),
                //item.price_with_fee.ToString(),
                //item.baslangic_fiyati.ToString(),
                //item.minimum_fiyat.ToString(),
                //item.interval_time.ToString(),
                //item.id.ToString()
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
                if (row.Cells[KolonIsimleri.CHECK_BOX].Value == null)
                    continue; 


                string itemName = row.Cells[KolonIsimleri.AD].Value.ToString();
                string assetId = row.Cells[KolonIsimleri.ASSET_ID].Value.ToString();
                double tavsiyeFiyat = Convert.ToDouble(row.Cells[KolonIsimleri.TAVSIYE_FIYAT].Value.ToString());
                double baslangicFiyati = (row.Cells[KolonIsimleri.BASLANGIC_FIYATI].Value == null
                    || row.Cells[KolonIsimleri.BASLANGIC_FIYATI].Value == "") ? tavsiyeFiyat : Convert.ToDouble(row.Cells[KolonIsimleri.BASLANGIC_FIYATI].Value.ToString());
                double minimumFiyat = (row.Cells[KolonIsimleri.MINIMUM_FIYAT].Value == null 
                    || row.Cells[KolonIsimleri.MINIMUM_FIYAT].Value == "") ? baslangicFiyati - (baslangicFiyati * 0.07) : Convert.ToDouble(row.Cells[KolonIsimleri.MINIMUM_FIYAT].Value);
                int miliseconds = (row.Cells[KolonIsimleri.FIYAT_KONTROL_ARALIGI].Value == null
                    || row.Cells[KolonIsimleri.FIYAT_KONTROL_ARALIGI].Value == "0") ? 2000 : Convert.ToInt32(row.Cells[KolonIsimleri.FIYAT_KONTROL_ARALIGI].Value.ToString()); // INTERVAL0 YENIDEN SETLENMELI;
                //...
                Datum datum = new Datum()
                {
                    steam_item = new SteamItem ()
                    {
                        steam_market_hash_name = itemName,
                        suggested_price = tavsiyeFiyat,
                        
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

            GenerateInventoryItems();

        }
        private void CleanRows()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
        }

        private void CancelItem(object sender, DataGridViewCellEventArgs e)
        {
            // buton disinda bir yere tiklanirsa gec
            if (e.ColumnIndex != 8)
                return;
            // envanteri goster ekraninda tiklanirsa gec
            if (!dataGridView1.Columns.Contains(Isimlendirmeler.SATIS_IPTAL))
                return;

            String itemId = null;
            var index = dataGridView1.Columns[Isimlendirmeler.SATIS_IPTAL].Index;
            if (e.ColumnIndex == dataGridView1.Columns[Isimlendirmeler.SATIS_IPTAL].Index)
            {
                var rows = dataGridView1.Rows;
                foreach (DataGridViewRow row in rows)
                {
                    var a = row.Cells[Isimlendirmeler.SATIS_IPTAL].RowIndex;
                    var b = e.RowIndex;
                    if (row.Cells[Isimlendirmeler.SATIS_IPTAL].RowIndex == e.RowIndex) // burada itemin id sini buluyoruz bir seyi setleyerek fiayt check dongusunun bitmesini saglamamiz gerekiyor
                    {
                        Console.WriteLine("HEYYYYY");
                        itemId = row.Cells["ITEM ID"].Value.ToString();
                    }

                }
                if (itemId != null)
                    PostMethods.CancelOffer(itemId);
                //Do something with your button.
               
            }
        }

        private void ItemiGuncelle(object sender, DataGridViewCellEventArgs e)
        {
            
            // buton disinda bir yere tiklanirsa gec
            if (e.ColumnIndex != 9)
                return;
            // envanteri goster ekraninda tiklanirsa gec
            if (!dataGridView1.Columns.Contains("Satış Güncelle"))
                return;
            /*
            String itemId = null;
            var index = dataGridView1.Columns["Satis_Iptal_Et"].Index;
            if (e.ColumnIndex == dataGridView1.Columns["Satis_Iptal_Et"].Index)
            {
                var rows = dataGridView1.Rows;
                foreach (DataGridViewRow row in rows)
                {
                    var a = row.Cells["Satis_Iptal_Et"].RowIndex;
                    var b = e.RowIndex;
                    if (row.Cells["Satis_Iptal_Et"].RowIndex == e.RowIndex) // burada itemin id sini buluyoruz bir seyi setleyerek fiayt check dongusunun bitmesini saglamamiz gerekiyor
                    {
                        Console.WriteLine("HEYYYYY");
                        itemId = row.Cells["ITEM ID"].Value.ToString();
                    }

                }
                if (itemId != null)
                    PostMethods.CancelOffer(itemId);
                //Do something with your button.

            }
            */
            Console.WriteLine("uzgun surat");
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
            StringBuilder sb = new StringBuilder();

            sb.Append($"{DateTime.Now.ToString("h:mm:ss tt")}\n");

            String filePath = "C:\\Users\\mdeveci\\Desktop\\BOT\\";
            // flush every 20 seconds as you do it
            File.AppendAllText(filePath + "log.txt", sb.ToString());
            sb.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
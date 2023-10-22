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
using System.Collections.Generic;

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
            CleanRows(dataGridView1);

            Inventory inventory_model = CsgoBot.Methods.GetMethods.GetInventory();
            List<InventoryItem> inventory = DistinctList(inventory_model?.data);

            if (inventory == null)
            {
                Console.WriteLine("Envanter bos geliyor");
                return;
            }

            if (inventory.Count < 1)
            {
                Console.WriteLine("Envanter BOS GELIYORR!!!!!");
                //inventory.data = Generate();
            }

            dataGridView1.Visible = true;
            dataGridView1.Size = new System.Drawing.Size(1200, 1400);
            dataGridView1.AutoSize = true;
            dataGridView1.ScrollBars = ScrollBars.Vertical;


            DataGridViewButtonColumn baslat_click = new DataGridViewButtonColumn();

            baslat_click.Name = "Başlat";
            baslat_click.Text = "X";

            dataGridView1.ColumnCount = 9;
            dataGridView1.Columns[KolonIsimleri.AD].Name = "İsim";
            dataGridView1.Columns[KolonIsimleri.ASSET_ID].Name = "Asset Id";
            dataGridView1.Columns[KolonIsimleri.TAVSIYE_FIYAT].Name = "Tavsiye fiyat";
            dataGridView1.Columns[KolonIsimleri.SITE_SATIS_FIYATI].Name = "Site Satış Fiyatı";
            dataGridView1.Columns[KolonIsimleri.BASLANGIC_FIYATI].Name = "Başlangıc Fiyatı";
            dataGridView1.Columns[KolonIsimleri.MINIMUM_FIYAT].Name = "Minimum fiyat";
            dataGridView1.Columns[KolonIsimleri.FIYAT_KONTROL_ARALIGI].Name = "Fiyat Kontrol Araligi(in ms)";
            dataGridView1.Columns[KolonIsimleri.ITEM_ID].Name = "ITEM ID";

            dataGridView1.Columns[KolonIsimleri.AD].Width = 350;
            dataGridView1.Columns[KolonIsimleri.BASLANGIC_FIYATI].Width = 100;
            dataGridView1.Columns[KolonIsimleri.MINIMUM_FIYAT].Width = 100;
            dataGridView1.Columns[KolonIsimleri.TAVSIYE_FIYAT].Width = 60;
            dataGridView1.Columns[KolonIsimleri.SITE_SATIS_FIYATI].Width = 60;
            
            dataGridView1.Columns.Insert(9, baslat_click);

            int count = 1;

            // sort by column
            dataGridView1.Sort(dataGridView1.Columns["Tavsiye fiyat"], ListSortDirection.Descending);


            var tradableItems = inventory.Where(x => x.tradable == true);
            //dataGridView1.Columns.Add(checkColumn); // bunun altına dıger butonlar eklenecek

            var itemFiyatlari = GetMethods.TumItemFiyatlariniGetir();

            foreach (var item in tradableItems)
            {
                var seciliITem = SeciliItemler.Where(x => x.asset_id == item.asset_id).FirstOrDefault();
                //var itemName = GetMethods.ItemFiyatGetir(item.steam_market_hash_name);
                string[] row = new string[] { item.steam_market_hash_name,
                                              item.asset_id,
                                              item.suggested_price.ToString(),
                                              itemFiyatlari != null ?
                                              itemFiyatlari.FirstOrDefault(x => x.steam_market_hash_name == item.steam_market_hash_name)?.price :
                                              "İtem Fiyatı Bulunamadı",
                                                seciliITem?.baslangic_fiyati.ToString(), 
                                                seciliITem?.minimum_fiyat.ToString(), 
                                                seciliITem?.interval_time.ToString(),
                                              item.id.ToString()
                                                };
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
            CleanRows(dataGridView1);

            dataGridView1.Visible = true;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.Size = new System.Drawing.Size(1800, 1000);

            DataGridViewButtonColumn baslat_click = new DataGridViewButtonColumn();
            //DataGridViewButtonColumn itemiGuncelle = new DataGridViewButtonColumn();
            DataGridViewButtonColumn cancelItem = new DataGridViewButtonColumn();

            baslat_click.Name = "Başlat";
            baslat_click.Text = "X";

            cancelItem.Name = Isimlendirmeler.SATIS_IPTAL;
            cancelItem.Text = "X";

            //itemiGuncelle.Name = "Satış Güncelle";

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
            dataGridView1.Columns[KolonIsimleri.TAVSIYE_FIYAT].Width = 60;
            dataGridView1.Columns[KolonIsimleri.SITE_SATIS_FIYATI].Width = 120;

            dataGridView1.Columns.Insert(8, baslat_click);
            dataGridView1.Columns.Insert(KolonIsimleri.SATIS_IPTAL_ET, cancelItem);
            //dataGridView1.Columns.Add(checkColumn);
            //dataGridView1.Columns.Insert(KolonIsimleri.SATIS_GUNCELLE, itemiGuncelle);

            // check box

            dataGridView1.Columns[0].Width = 350;

            List<DatumOffer> satistakiItemler = res?.data;

            var itemFiyatlari = GetMethods.TumItemFiyatlariniGetir();

            foreach (var item in satistakiItemler)
            {
                var seciliITem = SeciliItemler.Where(x => x.asset_id == item.asset_id).FirstOrDefault();
                string[] row = new string[] {
                                              item.steam_item.steam_market_hash_name,
                                              item.asset_id,
                                              item.steam_item.suggested_price.ToString(),
                                               itemFiyatlari != null ?
                                              itemFiyatlari.FirstOrDefault(x => x.steam_market_hash_name == item.steam_item.steam_market_hash_name).price  + "  -  " + item.price:
                                              "İtem Fiyatı Bulunamadı",

                                              seciliITem?.baslangic_fiyati.ToString(),
                                              seciliITem?.minimum_fiyat.ToString(),
                                              seciliITem?.interval_time.ToString(),
                                              item.id.ToString()
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


        private void baslat_click(object sender, DataGridViewCellEventArgs e)
        {
            // buton disinda bir yere tiklanirsa gec
            if (e.ColumnIndex != 9)
                return;
            // envanteri goster ekraninda tiklanirsa gec
            if (!dataGridView1.Columns.Contains(Isimlendirmeler.BASLAT)) // Başlat
                return;

            Console.WriteLine($"Task başladı: {dataGridView1.Rows[e.RowIndex].Cells[KolonIsimleri.AD].Value.ToString()}\n");

            string path = "https://api.shadowpay.com/api/v2/user/inventory";

            List<Datum> datumList = new List<Datum>();

            var rows = dataGridView1.Rows;
            var row = rows[e.RowIndex];


            string id = row.Cells[KolonIsimleri.ITEM_ID]?.Value == null ? null : row.Cells[KolonIsimleri.ITEM_ID]?.Value.ToString();
            PostMethods.CancelOffer(id);

 



            string itemName = row.Cells[KolonIsimleri.AD].Value.ToString();
            string assetId = row.Cells[KolonIsimleri.ASSET_ID].Value.ToString();
            double tavsiyeFiyat = Convert.ToDouble(row.Cells[KolonIsimleri.TAVSIYE_FIYAT].Value.ToString());
            double baslangicFiyati = (row.Cells[KolonIsimleri.BASLANGIC_FIYATI].Value == null
                || row.Cells[KolonIsimleri.BASLANGIC_FIYATI].Value == "") ? tavsiyeFiyat : Convert.ToDouble(row.Cells[KolonIsimleri.BASLANGIC_FIYATI].Value.ToString());
            double minimumFiyat = (row.Cells[KolonIsimleri.MINIMUM_FIYAT].Value == null 
                || row.Cells[KolonIsimleri.MINIMUM_FIYAT].Value == "") ? baslangicFiyati - (baslangicFiyati * 0.12) : Convert.ToDouble(row.Cells[KolonIsimleri.MINIMUM_FIYAT].Value);
            int miliseconds = (row.Cells[KolonIsimleri.FIYAT_KONTROL_ARALIGI].Value == null
                || row.Cells[KolonIsimleri.FIYAT_KONTROL_ARALIGI].Value == "0") ? 2000 : Convert.ToInt32(row.Cells[KolonIsimleri.FIYAT_KONTROL_ARALIGI].Value.ToString()); // INTERVAL0 YENIDEN SETLENMELI;
                                                                                                                                                                           //...
            List<Datum> seciliItem = SeciliItemler.Where(x => x.steam_item.steam_market_hash_name == itemName).ToList();
            if (seciliItem != null)
            {
                foreach (var item in seciliItem)
                {
                    item.baslangic_fiyati = baslangicFiyati;
                    item.minimum_fiyat = minimumFiyat;
                    item.interval_time = miliseconds;

                }
            }

            Datum datum = new Datum()
            {
                steam_item = new SteamItem()
                {
                    steam_market_hash_name = itemName,
                    suggested_price = tavsiyeFiyat,

                },
                asset_id = assetId,
                minimum_fiyat = minimumFiyat,
                baslangic_fiyati = baslangicFiyati,
                interval_time = miliseconds,
                id = Convert.ToInt32(id)
                };
                datumList.Add(datum);
                SeciliItemler.AddRange(datumList);

            Worker.worker_threads(datumList);

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
                        itemId = row.Cells["ITEM ID"]?.Value.ToString();
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


        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{DateTime.Now.ToString("h:mm:ss tt")}\n");

            String filePath = "C:\\Users\\mdeveci\\Desktop\\BOT\\";
            // flush every 20 seconds as you do it
            File.AppendAllText(filePath + "log.txt", sb.ToString());
            sb.Clear();
        }

        public static void CleanRows(DataGridView dataGridView1)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
        }

        private List<InventoryItem> DistinctList(List<InventoryItem> list)
        {
            List<InventoryItem> newArr = new List<InventoryItem>();
            for (int i = 0; i < list.Count; i++)
            {
                if (newArr.Count == 0)
                {
                    newArr.Add(list[i]);
                }
                else
                {
                    var item = newArr.SingleOrDefault(x => x.steam_market_hash_name == list[i].steam_market_hash_name);
                    if (item == null && list[i].tradable == true)
                        newArr.Add(list[i]);
                }
            }
            
            return newArr;
        }


    }
}
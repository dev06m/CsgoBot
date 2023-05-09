using CsgoBot.Assets;
using CsgoBot.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsgoBot.Methods
{
    class EkranButonlar
    {
        public static void SatisListesiButton(DataGridView dataGridView1, List<Datum> SeciliItemler)
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

            dataGridView1.Columns.Insert(8, baslat_click);
            dataGridView1.Columns.Insert(KolonIsimleri.SATIS_IPTAL_ET, cancelItem);
            //dataGridView1.Columns.Add(checkColumn);
            //dataGridView1.Columns.Insert(KolonIsimleri.SATIS_GUNCELLE, itemiGuncelle);

            // check box

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
                Datum seciliItem = SeciliItemler.Where(x => x.steam_item.steam_market_hash_name == item.steam_item.steam_market_hash_name).FirstOrDefault();
                string[] row = new string[] {
                                              item.steam_item.steam_market_hash_name,
                                              item.asset_id,
                                              item.steam_item.suggested_price.ToString(),
                                               itemFiyatlari != null ?
                                              itemFiyatlari.FirstOrDefault(x => x.steam_market_hash_name == item.steam_item.steam_market_hash_name).price  + "  -  " + item.price:
                                              "İtem Fiyatı Bulunamadı",
                                              seciliItem?.baslangic_fiyati.ToString(),
                                              seciliItem?.minimum_fiyat.ToString(),
                                              seciliItem?.interval_time.ToString(),
                                              seciliItem?.id.ToString()
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

        public static void EnvanteriGoster(DataGridView dataGridView1, List<Datum> SeciliItemler)
        {
            EkranButonlar.CleanRows(dataGridView1);

            Inventory inventory = CsgoBot.Methods.GetMethods.GetInventory();

            if (inventory == null)
            {
                Console.WriteLine("Envanter bos geliyor");
                return;
            }

            if (inventory.data.Count < 1)
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


            /*
            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "Sat";
            checkColumn.HeaderText = "Sat";
            checkColumn.Width = 65;
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 10; //if the datagridview is resized (on form resize) the checkbox won't take up too much; value is relative to the other columns' fill values
            */

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

            dataGridView1.Columns.Insert(9, baslat_click);

            int count = 1;

            // sort by column
            dataGridView1.Sort(dataGridView1.Columns["Tavsiye fiyat"], ListSortDirection.Descending);


            var tradableItems = inventory.data.Where(x => x.tradable == true);
            //dataGridView1.Columns.Add(checkColumn); // bunun altına dıger butonlar eklenecek

            var itemFiyatlari = GetMethods.TumItemFiyatlariniGetir();

            foreach (var item in tradableItems)
            {
                var seciliITem = SeciliItemler.Where(x => x.steam_item.steam_market_hash_name == item.steam_market_hash_name).FirstOrDefault();
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

        public static void CleanRows(DataGridView dataGridView1)
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

        public static void  SatisIptal(DataGridView dataGridView1, DataGridViewCellEventArgs e)
        {
           
        }


    }
}

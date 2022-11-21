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
            dataGridView1.Size = new System.Drawing.Size(900, 1400);
            //dataGridView1.AutoSize = false;
            dataGridView1.ScrollBars = ScrollBars.Both;

            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "Sat";
            checkColumn.HeaderText = "Sat";
            checkColumn.Width = 65;
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 10; //if the datagridview is resized (on form resize) the checkbox won't take up too much; value is relative to the other columns' fill values
            //itemOpen.Name = "Item";
            //itemOpen.Text = "Sayi";

            // botu baslatan button
            //dataGridView1.CellClick += Temp.random_click;

            //Kullanıcıya yeni kayıt ekleme izni.
            dataGridView1.AllowUserToAddRows = true;

            //Kullanıcıya kayıt silme izni.
            dataGridView1.AllowUserToDeleteRows = true;

            //Veriye tıklandığında satır seçimi sağlama.
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //DataGridView sütun oluşturma
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "Suggested Price";
            dataGridView1.Columns[2].Name = "Asset Id";
            dataGridView1.Columns[3].Name = "Tradable";
            dataGridView1.Columns[4].Name = "Satis fiyati";
            dataGridView1.Columns[5].Name = "Interval time(in ms)";

            int count = 1;
            //var tradeableItems = inventory.data;
            var tradableItems = inventory.data.Where(x => x.tradable == true);
            dataGridView1.Columns.Add(checkColumn);
            foreach (var item in tradableItems)
            {
                //if (dataGridView1.Columns["X"] == null)
                //{
                //    dataGridView1.Columns.Add(checkColumn);
                //}


                string[] row = new string[] { item.steam_market_hash_name,
                                              item.suggested_price.ToString(), item.asset_id,
                                              item.tradable.ToString()};
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
           
            foreach (var item in res?.data)
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


        private void baslat_click(object sender, EventArgs e)
        {
            string path = "https://api.shadowpay.com/api/v2/user/inventory";
            //Inventory inventory = CsgoBot.Methods.GetMethods.GetInventory();
            //List<InventoryItem> inventoryItems = inventory.data.Where(x => x.tradable == true).ToList();
            List<Datum> datumList = new List<Datum>();

            //if (inventoryItems == null)
            //{
            //    inventoryItems = GenerateInventoryItems();
            //}   

            var rows = dataGridView1.Rows;
            //DataGridView datagrid = new DataGridView();

            //datagrid.ColumnCount = 6;
            //datagrid.Columns[0].Name = "Name";
            //datagrid.Columns[1].Name = "Suggested Price";
            //datagrid.Columns[2].Name = "Asset Id";
            //datagrid.Columns[3].Name = "Tradable";
            //datagrid.Columns[4].Name = "Satis fiyati";
            //datagrid.Columns[5].Name = "Interval time(in ms)";

            //int index = 0;
            //foreach (DataGridViewRow row in rows)
            //{
            //    if (row.Cells[6].Value != null)
            //    {
            //        dataGridView1.Rows.RemoveAt(index);
            //        ////DataGridViewRow newRow = row;
            //        //datagrid.Rows.Add(row.Clone());
            //    }
            //    index++;
            //}

            foreach (DataGridViewRow row in rows)
            {
                if (row.Cells[6].Value == null)
                    continue; 
                var isSuggestedNull = row.Cells[4].Value == null;
                var isTimeIntervalNull = row?.Cells[5].Value == null;

                string itemName = row.Cells[0].Value.ToString();
                double suggestedPrice = Convert.ToDouble(row.Cells[1].Value.ToString());
                string assetId = row.Cells[2].Value.ToString();
                string tradable = row.Cells[3].Value.ToString(); // kullanilmayacak
                double determined_price = isSuggestedNull ? 10.0 : Convert.ToDouble(row.Cells[4].Value.ToString()); // SUGGESTEC PRICE YENIDEN SETLENMELI
                int miliseconds = isTimeIntervalNull ? 1000 : Convert.ToInt32(row?.Cells[5]?.Value?.ToString()); // INTERVAL0 YENIDEN SETLENMELI;
                //...
                Datum datum = new Datum()
                {
                    steam_item = new SteamItem ()
                    {
                        steam_market_hash_name = itemName,
                        suggested_price = suggestedPrice,
                        
                    },
                    asset_id = assetId,
                    determined_price = determined_price,
                    interval_time = miliseconds
                };
                datumList.Add(datum);
            }
            Temp.worker_threads(datumList);

            //if (inventory == null)
            //    return;

            //if (dataGridView1.SelectedCells.Count > 0)
            //{
            //    int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
            //    var cell1 = dataGridView1.Rows[selectedrowindex];
            //    var item = inventoryItems[selectedrowindex];
            //    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
            //    //string cellValue = Convert.ToString(selectedRow.Cells["enter column name"].Value);
            //}
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
    }
}
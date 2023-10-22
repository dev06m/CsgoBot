using CsgoBot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Methods
{
    internal class Worker
    {
        public static bool dongu = true;
        public static int itemId;
        public static bool fiyat_kontrol_dongusu = false;
        static int count = 0;
        static Thread workerThread1;
        public static void worker_threads(List<Datum> datums)
        {
            ItemForm itemForm = new ItemForm();

            if (datums.Count > 0)
            {
                workerThread1 = new Thread(() => Hile(datums[0], workerThread1));

                workerThread1.Start();
            }
            else
            {
                Console.WriteLine("\nIsaretlemeyi unuttun \n");
            }


        }

        public static void Hile(Datum item, Thread thread)
        {
            Datum selectedItem = item;
            dongu = true;
            
            int intervalTime = item.interval_time;
            var asset_id = item.asset_id;
            var suggestedPriceString = item.steam_item.suggested_price.ToString(); // buraya bak
            String baslangicFiyati = item.baslangic_fiyati.ToString();
            String minimumFiyat = item.minimum_fiyat.ToString();
            int bir_saat_bekle = item.bir_saat_bekle;
            int count = 1;
            double? minFiyat = item.minimum_fiyat;

            var threadId = Thread.CurrentThread.ManagedThreadId; // silinecek eger ise yaramiyorsa
            item.thread_id = threadId;
            
            while (dongu)
            {
                fiyat_kontrol_dongusu = false;
                int getIdCount = 0;
                MakeOfferResponse result = null;

                result = GetMethods.GetItemsOnOffers();
                /* item satista mi? */
                Datum myItem = result.data != null ? result.data.FirstOrDefault(x => x.asset_id == asset_id) : null;
                double itemPrice = 0;

                if (myItem == null)
                { /* ILK BASTA FIYAT SETLEME */
                    String ilk_setleme_sonuc = FiyatSetlemeMetodlari.FirstPriceSet(item, baslangicFiyati, minimumFiyat, asset_id);
                    if (ilk_setleme_sonuc == "E")
                        fiyat_kontrol_dongusu = true;
                    else
                    {
                        fiyat_kontrol_dongusu = false;
                        Thread.Sleep(item.interval_time);
                    }
                }

                else /* FIYAT UPDATE YAPMA */
                {
                    selectedItem = myItem;
                    selectedItem.interval_time = intervalTime;
                    selectedItem.minimum_fiyat = Convert.ToDouble(minimumFiyat);
                    selectedItem.baslangic_fiyati = Convert.ToDouble(baslangicFiyati);
                    selectedItem.bir_saat_bekle = bir_saat_bekle;
                    fiyat_kontrol_dongusu = FiyatSetlemeMetodlari.FiyatGuncelle(minFiyat, myItem, intervalTime); // minimum fiyati parametre olarak gecmeliyiz cunku datum objesini sunucudan cekiyor ve onun icinde min fiyat yok
                }
                
                // FIYAT DEGISIKLIGI VAR MI?
                while(fiyat_kontrol_dongusu)
                {
                    if (!GetMethods.FiyatDegisikligiCheck(selectedItem))
                        fiyat_kontrol_dongusu = false;
                    bir_saat_bekle = selectedItem.bir_saat_bekle;
                    Thread.Sleep(item.interval_time); // 1,5 saniyede bir bak
                }


                if (count % 100 == 0)
                    Console.WriteLine($"Request number: {count}");
                count++;
            }
            /*
            StringBuilder sb = new StringBuilder();

            sb.Append($"{DateTime.Now.ToString("h:mm:ss tt")} Thread durduruldu. (${item.steam_item.steam_market_hash_name})\n");

            String filePath = "C:\\Users\\mdeveci\\Desktop\\BOT";            
            // flush every 20 seconds as you do it
            File.AppendAllText(filePath + "log.txt", sb.ToString());
            sb.Clear();
            */
            Console.WriteLine($"Thread kapatılıyor... ({item.steam_item.steam_market_hash_name})");
        }
    }
}

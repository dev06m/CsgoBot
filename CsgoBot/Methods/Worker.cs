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
        static int count = 0;
        static Thread workerThread1, workerThread2, workerThread3, workerThread4, workerThread5, workerThread6, workerThread7, workerThread8, workerThread9, workerThread10;
        public static void worker_threads(List<Datum> datums)
        {
            ItemForm itemForm = new ItemForm();

            if (datums.Count > 0)
            {
                workerThread1 = new Thread(() => Hile(datums[0]));

                workerThread1.Start();
            }
            else
            {
                Console.WriteLine("\nIsaretlemeyi unuttun \n");
            }


        }

        public static void Hile(Datum item)
        {
            //Datum myItem = item;
            bool dongu = true;
            
            int miliseconds = item.interval_time;
            var asset_id = item.asset_id;
            var suggestedPriceString = item.steam_item.suggested_price.ToString(); // buraya bak
            String baslangicFiyati = item.baslangic_fiyati.ToString();
            String minimumFiyat = item.minimum_fiyat.ToString();
            int count = 1;
            double? minFiyat = item.minimum_fiyat;

            var threadId = Thread.CurrentThread.ManagedThreadId; // silinecek eger ise yaramiyorsa
            item.thread_id = threadId;
            
            while (dongu)
            {
                bool fiyat_kontrol_dongusu = false;
                int getIdCount = 0;
                MakeOfferResponse result = null;

                result = GetMethods.GetItemsOnOffers();
                // item satista mi?
                Datum myItem = result.data != null ? result.data.FirstOrDefault(x => x.asset_id == asset_id) : null;
                double itemPrice = 0;

                if (myItem == null)  // ILK BASTA FIYAT SETLEME 
                {
                    fiyat_kontrol_dongusu = FiyatSetlemeMetodlari.FirstPriceSet(item, baslangicFiyati, minimumFiyat, asset_id);
                }
                else // FIYAT UPDATE YAPMA
                {
                    fiyat_kontrol_dongusu = FiyatSetlemeMetodlari.FiyatGuncelle(minFiyat, item, miliseconds); // minimum fiyati parametre olarak gecmeliyiz cunku datum objesini sunucudan cekiyor ve onun icinde min fiyat yok
                }

                // FIYAT DEGISIKLIGI VAR MI?
                while(fiyat_kontrol_dongusu)
                {
                    if (!GetMethods.FiyatDegisikligiCheck())
                        fiyat_kontrol_dongusu = false;

                }


                if (count % 100 == 0)
                    Console.WriteLine($"Request number: {count}");
                count++;
            }
        }
    }
}

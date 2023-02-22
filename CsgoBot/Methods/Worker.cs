using CsgoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot.Methods
{
    internal class Worker
    {
        static int count = 0;
        static Thread workerThread1, workerThread2, workerThread3, workerThread4, workerThread5, workerThread6, workerThread7, workerThread8, workerThread9, workerThread10;
        public static async void worker_threads(List<Datum> datums)
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
            bool dongu = true;

            int miliseconds = item.interval_time;
            var asset_id = item.asset_id;
            var suggestedPriceString = item.steam_item.suggested_price.ToString(); // buraya bak
            String baslangicFiyati = item.baslangic_fiyati.ToString();
            String minimumFiyat = item.minimum_fiyat.ToString();
            int count = 1;
            double? minFiyat = item.minimum_fiyat;

            while (dongu)
            {
                int getIdCount = 0;
                MakeOfferResponse result = null;

                // id null gelirse 5 defa dene hala null geliyorsa else'de make offer yapiyoruz
                //while (getIdCount < 5)
                //{
                //    result = GetMethods.GetItemsOnOffers();
                //    if (result.data != null)
                //        if (result.data.Count > 0) 
                //            getIdCount = 5;
                //    getIdCount++;
                //}
                result = GetMethods.GetItemsOnOffers();
                // item satista mi?
                item = result.data != null ? result.data.FirstOrDefault(x => x.asset_id == asset_id) : null;
                double itemPrice = 0;

                if (item != null)  // FIYAT UPDATE YAPMA
                {
                    PriceSetMethods.PriceUpdate(minFiyat, item, miliseconds); // minimum fiyati parametre olarak gecmeliyiz cunku datum objesini sunucudan cekiyor ve onun icinde min fiyat yok
                }
                else // ILK BASTA FIYAT SETLEME
                {
                    PriceSetMethods.FirstPriceSet(item, baslangicFiyati, minimumFiyat, asset_id);
                }
                if (count % 100 == 0)
                    Console.WriteLine($"Request number: {count}");
                count++;
            }
        }
    }
}

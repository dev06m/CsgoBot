using CsgoBot.Methods;
using CsgoBot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot
{
    static class Temp
    {
        static int count = 0;
        static Thread workerThread1, workerThread2, workerThread3, workerThread4, workerThread5, workerThread6, workerThread7, workerThread8, workerThread9, workerThread10;
        public static async void worker_threads(List<Datum> datums)
        {
            ItemForm itemForm = new ItemForm();

            if (datums.Count > 0)
            {
                workerThread1 = new Thread(() => Hile(datums[0]));
                //workerThread2 = new Thread(() => Hile(datums[1]));
                //workerThread3 = new Thread(() => Hile(datums[2]));
                //workerThread4 = new Thread(() => Hile(datums[3]));
                //workerThread5 = new Thread(() => Hile(datums[4]));
                //workerThread6 = new Thread(() => Hile(datums[5]));
                //workerThread7 = new Thread(() => Hile(datums[6]));
                //workerThread8 = new Thread(() => Hile(datums[7]));
                //workerThread9 = new Thread(() => Hile(datums[8]));
                //workerThread10 = new Thread(() => Hile(datums[9]));
                //workerThread1 = new Thread(() => Hile(datums[10], miliseconds));

                workerThread1.Start();
                //workerThread2.Start();
                //workerThread3.Start();
                //workerThread4.Start();
                //workerThread5.Start();
                //workerThread6.Start();
                //workerThread7.Start();
                //workerThread8.Start();
                //workerThread9.Start();
                //workerThread10.Start();
            }else
            {
                Console.WriteLine("\nIsaretlemeyi unuttun \n");
            }


        }

        public static void Hile(Datum item)
        {
            int miliseconds = item.interval_time;
            var asset_id = item.asset_id;
            var suggestedPriceString = item.steam_item.suggested_price.ToString();
            int count = 1;

            while (true)
            {
                int getIdCount = 0;
                MakeOfferResponse result = null;

                // id null gelirse 5 defa dene hala null geliyorsa else'de make offer yapiyoruz
                while (getIdCount < 5)
                {
                    result = GetMethods.GetItemsOnOffers();
                    Thread.Sleep(miliseconds);
                    if (result.data != null)
                        if (result.data.Count > 1)
                            getIdCount = 5;
                    getIdCount++;
                }

                item = result.data != null ? result.data.FirstOrDefault(x => x.asset_id == asset_id) : null;
                double itemPrice = 0;
                
                if (item != null)  // FIYAT UPDATE YAPMA
                {
                    PriceUpdate(itemPrice, item, miliseconds);
                }
                else // ILK BASTA FIYAT SETLEME
                {
                    FirstPriceSet(item, itemPrice, suggestedPriceString, asset_id);
                }
                if (count % 100 == 0)
                    Console.WriteLine($"Request number: {count}");
                count++;
            }
        }

        private static void FirstPriceSet(Datum item, double itemPrice, string suggestedPriceString,string asset_id)
        {
            Console.WriteLine($"Item bulunamadi, fiyat setleniyor... --  {item?.steam_item?.steam_market_hash_name}\n");
            itemPrice = Convert.ToDouble(suggestedPriceString);
            try
            {
                string price = (Convert.ToDouble(suggestedPriceString) * 1.2).ToString();
                var offerResult = PostMethods.MakeOffer(asset_id, price);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //throw;
            }
        }

        private static void PriceUpdate(double itemPrice, Datum item, int miliseconds)
        {
            string itemName = item.steam_item.steam_market_hash_name;
            var priceStatus = true;
            itemPrice = item.price;
            while (priceStatus)
            {
                var lowestPrice = GetMethods.ItemFiyatGetir(itemName).Result;

                itemPrice = Convert.ToDouble(GetMethods.ItemFiyatGetir(itemName).Result.price);
                double doubleLowestPrice = lowestPrice.price != null ? double.Parse(lowestPrice.price, System.Globalization.CultureInfo.InvariantCulture) : item.steam_item.suggested_price;
                if (doubleLowestPrice < itemPrice)
                    priceStatus = false;
                Console.WriteLine($"Dongu icinde Fiyat Ayni | {item.steam_item.steam_market_hash_name} - {doubleLowestPrice} | \n");
            }
            // en dusuk fiyati bul itemi 1 cent altina koy
            if (item != null)
            {
                PostMethods.SetLowestPrice(item, miliseconds);
            }
            count++;
        }
    }
}

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

                // id null gelirse 5 defa dene hala null geliyorsa else'de makeoffer yapiyoruz
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
                
                if (item != null) 
                {
                    string itemName = item.steam_item.steam_market_hash_name;
                    var priceStatus = true;
                    itemPrice = item.price;
                    while (priceStatus)
                    {
                        Thread.Sleep(miliseconds);
                        var lowestPrice = GetMethods.ItemFiyatGetir(itemName).Result;
                        Thread.Sleep(miliseconds);
                        itemPrice = Convert.ToDouble(GetMethods.ItemFiyatGetir(itemName).Result.price);
                        double doubleLowestPrice = lowestPrice.price != null ? double.Parse(lowestPrice.price, System.Globalization.CultureInfo.InvariantCulture) : item.steam_item.suggested_price;
                        if (doubleLowestPrice < itemPrice)
                            priceStatus = false;
                        Console.WriteLine($"Dongu icinde Fiyat Ayni | {item.steam_item.steam_market_hash_name} - {doubleLowestPrice} | \n");
                    }
                    // en dusuk fiyati bul itemi 1 cent altina koy
                    if (item != null)
                    {
                        itemPrice = item.price;
                        Thread.Sleep(miliseconds);
                        PostMethods.SetLowestPrice(item, miliseconds);
                    }
                    count++;
                }else
                {
                    Console.WriteLine($"Item id NULL, fiyat tekrar setleniyor... |  {item?.steam_item?.steam_market_hash_name}  | \n");
                    itemPrice = Convert.ToDouble(suggestedPriceString);
                    Thread.Sleep(miliseconds);
                    try
                    {
                        var offerResult = PostMethods.MakeOffer(asset_id, (Convert.ToDouble(suggestedPriceString)*2).ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        //throw;
                    }
                }
                if (count % 100 == 0)
                    Console.WriteLine($"Request number: {count}");
                count++;
            }
        }


        public async static void Print()
        {

            PriceDatum result = new PriceDatum();
            long start_time, elapsed_time;
            while (true)
            {
                start_time = DateTime.Now.Millisecond;
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                await Task.Run(() =>
                {
                    result = GetMethods.ItemFiyatGetir("AK-47 | Elite Build (Battle-Scarred)").Result;
                });
                Console.WriteLine($"{count} {result?.price}");
                Thread.Sleep(2000);
                count++;

                stopwatch.Stop();
                elapsed_time = stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"start time: {start_time} elapsed time: {elapsed_time}");
                //Console.WriteLine(elapsed_time - start_time);

            }
        }
    }
}

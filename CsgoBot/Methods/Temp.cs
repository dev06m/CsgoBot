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
        static Thread workerThread1, workerThread2, workerThread3, workerThread4;
        public static void worker_threads(object sender, DataGridViewCellEventArgs e)
        {
            ItemForm itemForm = new ItemForm();
            //itemForm.Show();
            var offerItems = GetMethods.GetItemsOnOffers();
            if (offerItems != null)
            {
                var itemIds = offerItems.data;
                //workerThread1 = new Thread(() => Hile(itemIds.FirstOrDefault(x => x.asset_id == "27194641919"), 700));
                workerThread1 = new Thread(() => Hile(itemIds[0], 500));
                workerThread2 = new Thread(() => Hile(itemIds[1], 700));
                workerThread3 = new Thread(() => Hile(itemIds[2], 1000));
                workerThread4 = new Thread(() => Hile(itemIds[3], 1300));

                workerThread1.Start();
                workerThread2.Start();
                workerThread3.Start();
                workerThread4.Start();

            }


        }

        public static async void Hile(Datum item, int miliseconds)
        {
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
                    Thread.Sleep(miliseconds);
                    result = GetMethods.GetItemsOnOffers();
                    if (result != null)
                        getIdCount = 5;
                    getIdCount++;
                }
                    //continue;
                item = result != null ? result.data.FirstOrDefault(x => x.asset_id == asset_id) : null;
                double itemPrice = 0;
                //?.data?.FirstOrDefault(x => x.asset_id == asset_id);
                if (item != null) 
                {
                    string itemName = item.steam_item.steam_market_hash_name;
                    var priceStatus = true;
                    itemPrice = item.price;
                    while (priceStatus)
                    {
                        Thread.Sleep(miliseconds);
                        var lowestPrice = GetMethods.ItemFiyatGetir(itemName).Result;
                        double doubleLowestPrice = lowestPrice != null ? double.Parse(lowestPrice.price, System.Globalization.CultureInfo.InvariantCulture) : item.steam_item.suggested_price;
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
                    Console.WriteLine("Item id NULL, fiyat tekrar setleniyor... \n");
                    itemPrice = Convert.ToDouble(suggestedPriceString);
                    Thread.Sleep(miliseconds);
                    try
                    {
                        await PostMethods.MakeOffer(asset_id, suggestedPriceString);
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


        public  static void Print2(object obj)
        {
            int count = 4;
            Thread.Sleep(4000);
            Console.WriteLine("Selam"); 


        }

        public static void CheckThreadState()
        {

        }
    }
}

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
    static class FiyatSetlemeMetodlari
    {

        static int count = 0;
        public static string FirstPriceSet(Datum item, String baslangicFiyati, String minimumFiyat,string asset_id) // return --> "E" "H" "satildi"
        {
            if (item == null)
            {
                Console.WriteLine("İtem null geliyor\n");
                return "";
            }
            try
            {
                Console.WriteLine($"Item satışa koymayi deniyor.. __{item?.steam_item?.steam_market_hash_name}__\n");
                bool dongu = true;
                int index = 0;
                bool offerResult = false;
                while (!offerResult && index <= 3)
                {
                    offerResult = PostMethods.IlkFiyatSetleme(asset_id, baslangicFiyati, item.steam_item.steam_market_hash_name);
                    if (offerResult)
                    {
                        Console.WriteLine($"Item ilk kez satisa kondu __{item?.steam_item?.steam_market_hash_name}__\n");
                        offerResult = true;
                        return "";
                    }
                    else
                    {
                        Console.WriteLine($"Item ilk kez satisa konamadi 3 s sonra tekrar denenecek __{item?.steam_item?.steam_market_hash_name}__\n");
                        Thread.Sleep(3000);
                    }
                    index++;
                    if (index > 3)
                    {
                        Thread.Sleep(300000); // 5 dakika
                    }
                    if (index == 10)
                    {
                        offerResult = true;
                        return item.asset_id;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"SatIşa koyma başarısız, error {e.Message}");
            }
            return "";
        }

        public static bool FiyatGuncelle(double? minFiyat, Datum item, int miliseconds)
        {
            double suggestedPrice = item.steam_item.suggested_price;
            double? altLimit = minFiyat; 
            string itemId = item.asset_id.ToString();
            string itemName = item.steam_item.steam_market_hash_name;
            double doubleLowestPrice = GetMethods.ItemFiyatGetir(item.steam_item.steam_market_hash_name).Result;
            //string lowestPrice = lowestPriceObject != 0 ? lowestPriceObject.ToString() : Convert.ToString(suggestedPrice);
            //double doubleLowestPrice = lowestPrice != null ? lowestPriceObject : suggestedPrice;

            if (doubleLowestPrice == 0)
            {
                Console.WriteLine("LOWEST PRICE 0 geliyor.\n");
                return false;
            }

            var myItemPrice = item.price;
            if (myItemPrice == 0)
                Console.WriteLine("İtem fiyatı sıfır __{itemName}__\n");

            var newPrice = doubleLowestPrice - 0.01;
            string newPriceString = newPrice.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            if (newPrice < altLimit)
            {
                item.alt_limit++;
                Console.WriteLine($"Alt limite takıldı, 1 dk bekleme başladı__{itemName}__\n");
                Thread.Sleep(60000);
                Console.WriteLine($"1 dk bekleme bitt, başlangıc fiyatına setlenecek __{itemName}__\n ");
                var result_ = PostMethods.MakeOffer(item, item.baslangic_fiyati.ToString(), miliseconds);
                return false;
            }
            if ((myItemPrice < doubleLowestPrice || myItemPrice == 0)) 
            {
                Console.WriteLine($"İtem en düşük fiyat __{itemName}__\n");
                Thread.Sleep(item.interval_time); /* fiyat check dongusune girmeyecegi icin burada beklenityoruz */
                return false;
            }
            var result = PostMethods.MakeOffer(item, newPriceString, miliseconds);
            if(result?.status == "success") {
                Console.WriteLine($"İtem fiyati degisti, yeni fiyat: {newPriceString} eski fiyat: {doubleLowestPrice} __{itemName}__ \n");
                return true;
            };
            return false;

        }
    }
}

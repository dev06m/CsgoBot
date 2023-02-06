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
    static class PriceSetMethods
    {

        static int count = 0;
        public static void FirstPriceSet(Datum item, String baslangicFiyati, String minimumFiyat,string asset_id)
        {
            Console.WriteLine($"Item ilk kez satisa konuyor, fiyat setleniyor... --  {item?.steam_item?.steam_market_hash_name}\n");
            try
            {
                var offerResult = PostMethods.IlkFiyatSetleme(asset_id, baslangicFiyati);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //throw;
            }
        }

        public static void PriceUpdate(double itemPrice, Datum item, int miliseconds)
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

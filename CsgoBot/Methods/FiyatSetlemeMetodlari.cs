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
        public static bool FirstPriceSet(Datum item, String baslangicFiyati, String minimumFiyat,string asset_id)
        {
            if (item == null)
            {
                Console.WriteLine("İtem null gelyior\n");
                return false;
            }
            try
            {
                Console.WriteLine($"Item satışa konuyor \"{item?.steam_item?.steam_market_hash_name}\"...\n");

                var offerResult = PostMethods.IlkFiyatSetleme(asset_id, baslangicFiyati);
                return offerResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }

        public static bool FiyatGuncelle(double? minFiyat, Datum item, int miliseconds)
        {
            if (item == null)
                return false;

            string itemName = item.steam_item.steam_market_hash_name;
            bool fiyatDegisikligi = true;

            bool result = true;


            fiyatDegisikligi = PostMethods.SetLowestPrice(minFiyat, item, miliseconds);

            while (fiyatDegisikligi)
            {
                var lowestPrice = GetMethods.ItemFiyatGetir(item.steam_item.steam_market_hash_name).Result;
                double itemPrice = Convert.ToDouble(GetMethods.SatistakiItemFiyatiGetir(itemName));
                if (itemPrice != null && itemPrice != 0)
                {
                    double doubleLowestPrice = lowestPrice != null ? lowestPrice : item.steam_item.suggested_price;
                    if (doubleLowestPrice < itemPrice)
                    {
                        fiyatDegisikligi = false;
                        result = true;
                    }
                    Console.WriteLine($"Dongu icinde Fiyat Ayni | {item.steam_item.steam_market_hash_name} - {doubleLowestPrice} | \n");
                }else
                {
                    fiyatDegisikligi = false;
                }
                Thread.Sleep(miliseconds);
            }
           
            count++;
            return result;
        }
    }
}

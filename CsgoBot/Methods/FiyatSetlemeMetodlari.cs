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
        public static String FirstPriceSet(Datum item, String baslangicFiyati, String minimumFiyat,string asset_id) // return --> "E" "H" "satildi"
        {
            if (item == null)
            {
                Console.WriteLine("İtem null geliyor\n");
                return "E";
            }
            try
            {
                Console.WriteLine($"Item satışa koymayi deniyor.. __{item?.steam_item?.steam_market_hash_name}__\n");

                var offerResult = PostMethods.IlkFiyatSetleme(asset_id, baslangicFiyati);
                if (offerResult == "E")
                    Console.WriteLine($"Item ilk kez satisa kondu __{item?.steam_item?.steam_market_hash_name}__\n");
                return offerResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "E";
            }
            return "E";
        }

        public static bool FiyatGuncelle(double? minFiyat, Datum item, int miliseconds)
        {
            double suggestedPrice = item.steam_item.suggested_price;
            double? altLimit = minFiyat; 
            string itemId = item.asset_id.ToString();
            string itemName = item.steam_item.steam_market_hash_name;
            double lowestPriceObject = GetMethods.ItemFiyatGetir(item.steam_item.steam_market_hash_name).Result;
            string lowestPrice = lowestPriceObject != null ? lowestPriceObject.ToString() : Convert.ToString(suggestedPrice);
            double doubleLowestPrice = lowestPrice != null ? lowestPriceObject : suggestedPrice;

            var myItemPrice = item.price;

            var newPrice = doubleLowestPrice - 0.01;
            string newPriceString = newPrice.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            if (newPrice < altLimit)
            {
                item.alt_limit++;
                Console.WriteLine($"Alt limite takıldı, fiyat 20. denemede ({item.alt_limit}) başlangıc fiyatına setlenecek __{itemName}__\n");
                if (item.alt_limit == 20)
                {
                    var result_ = PostMethods.MakeOffer(item, item.baslangic_fiyati.ToString(), miliseconds);
                    Console.WriteLine($"Alt limite takıldığı için başlangıç fiyatına setlendi, 3dk uykuya geçiyor...  ({itemName})\n");
                    Thread.Sleep(180000);
                    Console.WriteLine($"Uyandı yarış devam ediyor...  ({itemName})\n");
                    item.alt_limit = 0;
                }
                return false;
            }
            if ((myItemPrice < doubleLowestPrice || myItemPrice == 0)) 
            {
                Console.WriteLine($"İtem en düşük fiyat ya da fiyatı sıfır __{itemName}__\n");
                return false;
            }
            var result = PostMethods.MakeOffer(item, newPriceString, miliseconds);
            if(result.status == "success") {
                Console.WriteLine($"İtem fiyati degisti, yeni fiyat: {newPriceString} eski fiyat: {doubleLowestPrice} __{itemName}__ \n");
                return true;
            };
            return false;

        }
    }
}

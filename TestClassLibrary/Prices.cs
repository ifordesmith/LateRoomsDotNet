using System;
using System.Collections;
using System.Text;

namespace TestClassLibrary
{
    public class Specials
    {
        //public String SKU { get; set; }
        public int Qty { get; set; }
        public double BulkPrice { get; set; }
    }
    public class Prices
    {
            /// <summary>
            /// Simple Hashtable to hold current prices 
            /// </summary>
            private Hashtable priceTable;
            private Hashtable specialPrices;

            public Prices()
            {
                //Initialise the price table with some dummy data !
                priceTable = new Hashtable();
                specialPrices = new Hashtable();
                //We could initialise the table in the constructor here or allow the external system to set this up
                //either way is valid but maybe better to have the system start with something in it !
                AddUpdatePrice("A", 50);
                AddUpdatePrice("B", 30);
                AddUpdatePrice("C", 20);
                AddUpdatePrice("D", 15);

                AddUpdateSpecial("A", 3, 130);
                AddUpdateSpecial("B", 2, 45);

        }

        //Helper Methods to Add or Updated New Prices
        public void AddUpdatePrice(String sku, Double price)
            {
                ///Set the price of the SKU or Create a new entry for this SKU
                priceTable[sku] = price;
            }
            public Double GetPrice(String sku)
            {
                try
                {
                    if (!priceTable.ContainsKey(sku))
                    {
                        ///Raise an invalid Item - has no price !
                        throw new Exception("Unknown SKU "+sku);
                    }
                    return Convert.ToDouble(priceTable[sku]);
                }
                catch (Exception ex)
                {
                    throw;
                }
                    
            }

            //We could do this a number of ways allowing for possibility of tiered bulk prices on the same SKU
            //But for purposes of this we shall assume one SKU entry and use this as the key for the sake of speed !
            public void AddUpdateSpecial(String sku,int qty,double bulk_price)
            {
                Specials newSpecial = new Specials();
                //newSpecial.SKU = sku;
                newSpecial.Qty = qty;
                newSpecial.BulkPrice = bulk_price;
                specialPrices[sku] = newSpecial;
            }

            public Specials GetSpecial(String sku)
            {
                return (Specials) specialPrices[sku];
            }

            
        }
 
}

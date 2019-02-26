using System;
using System.Collections;
using System.Text;



namespace TestClassLibrary
{
    public class Transaction
    {
        public String SKU { get; set; }
        public Double price { get; set; }
        public bool markedforDeletion { get; set; }
        public bool SpecialOffer { get; set; }
    }

    public class Basket:ICheckout
    {
        private Hashtable basket;
       
        /// <summary>
        /// Run the initialiser
        /// </summary>
        public Basket(Prices prices)
        {
            basket = new Hashtable();
            Console.WriteLine("Basket Initialised {0}",basket.Count);
            this.prices = prices;
            

        }
        public Prices prices { get; set; }

        private Hashtable GroupBasket()
        {
            //Run over the basket contents to group and count by SKU.  Compare the results with specials for a given SKU.
            //If there is a special for the SKU with the given quantity delete the invidual transactions and create a new transaction for the bulk price
            Hashtable sku_count = new Hashtable();
            foreach (DictionaryEntry pair in basket)
            {
                Transaction trans = (Transaction)pair.Value;
                if (trans.markedforDeletion || trans.SpecialOffer)
                {
                    ///We do not want to count these transactions so skip them
                    continue;
                }
                int curVal = 0;
                if (sku_count.ContainsKey(trans.SKU))
                {
                    curVal = (int)sku_count[trans.SKU];
                }
                sku_count[trans.SKU] = curVal + 1;

            }
            return sku_count;

        }
        private bool PruneTransactions(String SKU,int qty)
        {
            int qty_removed = 0;
            bool pruningRequired = false;
            foreach(DictionaryEntry pair in basket)
            {
                Transaction trans = (Transaction)pair.Value;
                if (trans.SKU==SKU && !trans.SpecialOffer && qty_removed<qty && !trans.markedforDeletion)
                {
                    ///Remove this transaction as it's being replaced by a special !
                    trans.markedforDeletion = true;
                    pruningRequired = true;
                    qty_removed += 1;
                }
            }
            return pruningRequired;
        }
        private void IterateBasket()
        {
            bool basketNeedsCleaning = true;

            while (basketNeedsCleaning)
            {
                Hashtable groupedSKUs = GroupBasket();
                ///Are there any special prices for this group 
                ///Assume the basket does not need cleaning further after this new group
                ///
                basketNeedsCleaning = false;
                foreach (DictionaryEntry pair in groupedSKUs)
                {
                    string SKU = (String)pair.Key;
                    int count = (int)pair.Value;
                    Specials special = prices.GetSpecial(SKU);
                    if (special != null)
                    {
                        Console.WriteLine("Special for SKU {0} {1}", SKU, special.Qty);
                        if (count >= special.Qty)
                        {
                            basketNeedsCleaning = true;
                            bool pruningNeeded = PruneTransactions(SKU, special.Qty);

                            ///We have a match on some or all of the transactions to go in and mark QTY transactions to be deleted for this SKU
                            Transaction newTransaction = new Transaction();
                            newTransaction.SKU = SKU;
                            newTransaction.price = special.BulkPrice;
                            newTransaction.SpecialOffer = true;
                            basket[basket.Count] = newTransaction;
                        }
                    }
                }
            }

        }

        //Implement Public ICheckout interface Scan.
        public void Scan(String SKU)
        {
            try
            {

                int tRef = basket.Count;
                Transaction transaction = new Transaction();

                transaction.SKU = SKU;
                transaction.price = prices.GetPrice(SKU);
                transaction.markedforDeletion = false;
                basket[tRef] = transaction;
                Console.WriteLine("SKU Scanned {0}", SKU);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid SKU {0} Scanned !, Item not Added to Basket",SKU);
            }
        }
        //Implement Public Interface ICheckout GetTotalPrice
        public int GetTotalPrice()
        {
            ///Run over the current basket looking for special bulk pricing and adjust accordingly
            IterateBasket();
            Double total = SumBasket();
            return Convert.ToInt32(total);
        }

        public void ResetBasket()
        {
            basket.Clear();
        }
        private double SumBasket()
        {
            Double total = 0;
            foreach(DictionaryEntry pair in basket)
            {
                Transaction trans = (Transaction)pair.Value;
                if (!trans.markedforDeletion) {
                    Console.WriteLine("Basket Entry SKU:{0} Price:{1} Special:{2}",trans.SKU,trans.price,trans.SpecialOffer);
                    total += trans.price;
                }
            }
            return total;
        }

    }
}

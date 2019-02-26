using System;
using System.Collections;
using System.Linq;


namespace TestClassLibrary
{
    public interface ICheckout
    {
        void Scan(string Item);
        int GetTotalPrice();
    }

    public class Tester
    {
        private Prices prices; 
        private Basket basket;

        private void test1()
        {
            ///Put in a single Scan on one known product
            basket.ResetBasket();
            basket.Scan("A");
            int tp = basket.GetTotalPrice();
            Console.WriteLine("Test 1 - Total Price {0}", tp);

        }
        private void test2()
        {
            ///Put in a single Scan on unknown product
            basket.ResetBasket();
            basket.Scan("G");
            int tp = basket.GetTotalPrice();
            Console.WriteLine("Test 2 - Total Price {0}", tp);

        }
        private void test3()
        {
            ///Put in mulitple scans and ensure we get 4 A (3 at discount) plus 2 B's fully discounted and a D
            basket.ResetBasket();
            basket.Scan("A");
            basket.Scan("D");
            basket.Scan("B");
            basket.Scan("A");
            basket.Scan("B");
            basket.Scan("A");
            basket.Scan("A");
       
            int tp = basket.GetTotalPrice();
            Console.WriteLine("Test 3 - Total Price {0} should be 240.", tp);

        }

        public Tester()
        {
            prices = new Prices();
            basket = new Basket(prices);
            test1();
            test2();
            test3();

            /* NB 
            Can be set or updated outside as in here or instantiated directly
            prices.AddUpdatePrice("A", 50);
            prices.AddUpdatePrice("B", 30);
            prices.AddUpdatePrice("C", 20);
            prices.AddUpdatePrice("D", 15);

            prices.AddUpdateSpecial("A", 3, 130);
            prices.AddUpdateSpecial("B", 4, 45);
            */

       
          

        }
    }
 }

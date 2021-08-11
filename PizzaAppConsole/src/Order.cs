using System.Collections.Generic;

namespace PizzaApp
{
    public class Order
    {
        public int NumOfPizzas { get; set; }
        public List<Pizza> ListOfPizzas { get; set; }
        public bool AddPizza(Pizza tobeAddedPizza)
        {
            try
            {
                ListOfPizzas.Add(tobeAddedPizza);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}

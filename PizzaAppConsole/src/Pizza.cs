using System.Collections.Generic;

namespace PizzaApp
{
    public class Pizza
    {
        private float Price = 0;
        public TypeXPrice Topping { get; set; }
        public TypeXPrice Size { get; set; }
        public TypeXPrice Side { get; set; }
        public float CalculatePrice()
        {
            if (this.Price == 0)
                this.Price += this.Topping.Price + this.Size.Price + this.Side.Price;
            return this.Price;
        }
    }
}

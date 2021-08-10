using System.Collections.Generic;

namespace PizzaApp
{
    public record TypeXPrice(string Type, float Price);
    public class PizzaModel
    {
        public List<TypeXPrice> Toppings { get; set; }
        public List<TypeXPrice> Sizes { get; set; }
        public List<TypeXPrice> Sides { get; set; }
    }
}

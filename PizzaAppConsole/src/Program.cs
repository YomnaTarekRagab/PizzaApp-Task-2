using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Spectre.Console;
using System.Text.Json;

namespace PizzaApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var baseUrl = "http://localhost:5000/";
            var htppClient = new HttpClient();
            bool begin = true;
            while (begin)
            {
                int numberOfPizzas = WelcomePage();
                int counter = 1;
                var prefOrder = new Order
                {
                    NumOfPizzas = numberOfPizzas,
                    ListOfPizzas = new List<Pizza>(numberOfPizzas)
                };
                while (counter <= numberOfPizzas)
                {
                    AnsiConsole.Render(new Markup($"[bold red] This is order number {counter} from your {numberOfPizzas} orders:[/] \n"));
                    //--API Call for the menu
                    string components = await htppClient.GetStringAsync(baseUrl + "components");
                    var pizzaMenu = System.Text.Json.JsonSerializer.Deserialize<PizzaModel>(components);
                    TypeXPrice prefTop = null, prefSize = null, prefSide = null;
                    (prefTop, prefSide, prefSize)=ConsoleFn(pizzaMenu);
                    var pizza = new Pizza 
                    {
                        Topping = prefTop,
                        Size = prefSize,
                        Side = prefSide
                    };
                    prefOrder.AddPizza(pizza);
                    //--API Call for order creation
                    if (numberOfPizzas == counter)
                    {
                        var myContent = JsonConvert.SerializeObject(prefOrder);
                        var stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                        var response = await htppClient.PostAsync(baseUrl + "createPizza", stringContent);
                        if (response.IsSuccessStatusCode)
                        {
                            AnsiConsole.Render(new Markup("[bold red] Order created successfully[/] \n"));
                        }
                    }
                    counter++;
                }
            }
        }
        
        static int WelcomePage()
        {
            AnsiConsole.Render(
                new FigletText("PIZZA MENU")
                .Centered()
                .Color(Color.Yellow));
            AnsiConsole.Render(new Markup("[bold yellow] Welcome! Start placing your order below...[/]\n"));
            AnsiConsole.Render(new Markup("[bold yellow] Enter the number of pizzas:[/] \n"));
            Int32.TryParse(Console.ReadLine(), out int n);
            return n;
        }
        
        static (TypeXPrice,TypeXPrice,TypeXPrice) ConsoleFn(PizzaModel pizzaMenu)
        {
            TypeXPrice prefTop = null, prefSize = null, prefSide = null;
            string formatTitle = "[bold green] Available toppings[/] \n";
            List<String> columnNames = new List<string> { "Toppings", "Prices" };
            Menu.PrintMenu(formatTitle, columnNames, pizzaMenu.Toppings);
            AnsiConsole.Render(new Markup("[bold yellow] Your preferred topping from the topping list:[/] \n"));
            prefTop = Menu.InputCheck(pizzaMenu.Toppings, "topping");
            formatTitle = "[bold green] Available sizes[/] \n";
            columnNames.Clear();
            columnNames.Add("Sizes");
            columnNames.Add("Prices");
            Menu.PrintMenu(formatTitle, columnNames, pizzaMenu.Sizes);
            AnsiConsole.Render(new Markup("[bold yellow] Your preferred pizza size from the sizes list:[/]\n"));
            prefSize = Menu.InputCheck(pizzaMenu.Sizes);
            formatTitle = "[bold green] Available sides[/] \n";
            columnNames.Clear();
            columnNames.Add("Sides");
            columnNames.Add("Prices");
            Menu.PrintMenu(formatTitle, columnNames, pizzaMenu.Sides);
            AnsiConsole.Render(new Markup("[bold yellow] Your preferred side from the sides list:[/]\n"));
            prefSide = Menu.InputCheck(pizzaMenu.Sides, "side");
            return (prefTop, prefSize, prefSide);
        }
    }
}
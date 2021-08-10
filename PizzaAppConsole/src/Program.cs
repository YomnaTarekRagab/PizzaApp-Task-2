using System;
using System.IO;
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
            int  numberOfPizzas = WelcomePage();
            int counter =1;
            while (counter <= numberOfPizzas)
            {
                AnsiConsole.Render(new Markup($"[bold red]This is order number {counter} from your {numberOfPizzas} orders:[/] \n"));
                //--API Call for the menu
                string components = await htppClient.GetStringAsync(baseUrl+"components");
                var pizzaMenu = System.Text.Json.JsonSerializer.Deserialize<PizzaModel>(components);
                TypeXPrice prefTop = null, prefSize = null, prefSide = null;
                ConsoleFn(pizzaMenu, ref prefTop, ref prefSize, ref prefSide);
                System.Console.WriteLine( $"Pref top is {prefTop} and size is {prefSize} and side is {prefSide}");
                //--API Call for order creation
                // var formContent = new FormUrlEncodedContent(new[]
                // {
                //     new KeyValuePair<string, string>("comment", "aykalam"), 
                //     new KeyValuePair<string, string>("questionId", "questionIdaykalam") 
                // });
                // var response = await htppClient.PostAsync($"{baseUrl} +createPizza",formContent);
                counter++;
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
        static void ConsoleFn(PizzaModel pizzaMenu, ref TypeXPrice prefTop, ref TypeXPrice prefSize, ref TypeXPrice prefSide)
            {
                string formatTitle = "[bold green]Available toppings[/] \n";
                List<String> columnNames = new List<string> { "Toppings", "Prices" };
                Menu.PrintMenu(formatTitle, columnNames, pizzaMenu.Toppings);
                AnsiConsole.Render(new Markup("[bold yellow] Your preferred topping from the topping list:[/] \n"));
                prefTop = Menu.InputCheck(pizzaMenu.Toppings, "topping");
                formatTitle = "[bold green]Available sizes[/] \n";
                columnNames.Clear();
                columnNames.Add("Sizes");
                columnNames.Add("Prices");
                Menu.PrintMenu(formatTitle, columnNames, pizzaMenu.Sizes);
                AnsiConsole.Render(new Markup("[bold yellow]Your preferred pizza size from the sizes list:[/]\n"));
                prefSize = Menu.InputCheck(pizzaMenu.Sizes);
                formatTitle = "[bold green]Available sides[/] \n";
                columnNames.Clear();
                columnNames.Add("Sides");
                columnNames.Add("Prices");
                Menu.PrintMenu(formatTitle, columnNames, pizzaMenu.Sides);
                AnsiConsole.Render(new Markup("[bold yellow]Your preferred side from the sides list:[/]\n"));
                prefSide = Menu.InputCheck(pizzaMenu.Sides, "side");
            }
    }
}

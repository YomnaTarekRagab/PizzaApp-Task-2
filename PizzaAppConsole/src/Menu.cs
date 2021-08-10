using System;
using System.Collections.Generic;
using Spectre.Console;

namespace PizzaApp
{
    public class Menu
    {
        public static void PrintMenu(string formatTitle, List<string> columnNames, List<TypeXPrice> items)
        {
            //--Title
            AnsiConsole.Render(new Markup(formatTitle));
            var table = new Table();
            //--Columns
            foreach (string columnName in columnNames)
                table.AddColumn(new TableColumn(columnName).Centered());
            //--Rows
            foreach (var item in items)
                table.AddRow(new Markup($"[bold Red]{item.Type}[/]"), new Markup($"[bold Red] ${item.Price}[/]"));
            AnsiConsole.Render(table);
        }

        public static TypeXPrice InputCheck(List<TypeXPrice> inputArray, string type = "")
        {
            bool check = false;
            string chosen = "";
            TypeXPrice chosenItem = null;
            while (!check)
            {
                chosen = Console.ReadLine();
                foreach (var item in inputArray)
                {
                    //--Toppings,Size And Sides Check
                    string myType = (string)(object)item.Type;
                    if (string.Equals(chosen, myType, StringComparison.OrdinalIgnoreCase))
                    {
                        chosenItem = item;
                        if (type == "topping")
                            AnsiConsole.Render(new Markup($"[bold blue] Chosen Topping is: {chosen}[/]\n"));
                        else if (type == "side")
                            AnsiConsole.Render(new Markup($"[bold blue]Chosen Side is: {chosen}[/]\n"));
                        else if (type == "size")
                            AnsiConsole.Render(new Markup($"[bold blue]Chosen Size is: {chosen}[/]\n"));
                        check = true;
                    }
                }
            }
            return chosenItem;
        }
    }
}

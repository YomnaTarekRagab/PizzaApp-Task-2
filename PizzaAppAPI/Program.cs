using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using PizzaApp;
using System.Text.Json;

var app = WebApplication.Create();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/components", async () =>
{
   var t1 = await ReadOrdersList("Files/PizzaMenu.json");
   return t1;
});

app.MapPost("/createPizza", async ([FromBody] Order receivedOrder) =>
{
    receivedOrder.OrderPrice();
    string path = Path.Combine(Directory.GetCurrentDirectory(), "Files/orders.json");
    var options = new JsonSerializerOptions 
    { 
        WriteIndented = true,
        IncludeFields = true
    };
    var orderJson = System.Text.Json.JsonSerializer.Serialize(receivedOrder, options);
    await File.AppendAllTextAsync(path, orderJson);
    return new OkObjectResult(receivedOrder);
});

static async Task<string> ReadOrdersList(string fileName)
{
    string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
    string jsonString = await File.ReadAllTextAsync(path);
    return jsonString;
}

await app.RunAsync();
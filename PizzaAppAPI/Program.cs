using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.MapGet("/components", async () =>
{
    string path = Path.Combine(Directory.GetCurrentDirectory(), "Files/PizzaMenu.json");
    string jsonString = await File.ReadAllTextAsync(path);
    return jsonString;
});
// app.MapPost("/createPizza", async ([FromBody Order order]) =>
// {

// });
await app.RunAsync();

// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");
HttpClient client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:7147/");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));
HttpResponseMessage response = await client.GetAsync(
                "/Home/Index");
Console.WriteLine(response.StatusCode);
Console.ReadLine();

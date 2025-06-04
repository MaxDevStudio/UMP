using System.CommandLine;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("UMP CLI");
        var marketCommand = new Command("market", "Market commands");
        var listCommand = new Command("list", "List items");

        listCommand.SetHandler(async () =>
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7177/api/market");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        });

        marketCommand.AddCommand(listCommand);
        rootCommand.AddCommand(marketCommand);

        return await rootCommand.InvokeAsync(args);
    }
}
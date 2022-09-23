// See https://aka.ms/new-console-template for more information
using OpenAI;
using OpenAIParquetGenerator.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Let's start the OpenAI Parquet/CSV Generation!" + Environment.NewLine);
        Console.WriteLine("Please, insert your OpenAI API key:");

        var apiKey = Console.ReadLine();

        if (apiKey is null)
        {
            Console.WriteLine("Insert a valid API Key");
            return;
        }

        Console.WriteLine("Do you want to connect MongoDB to retrieve Parquet Generation Metadata? (y/n)");
        var connectMongoDb = Console.ReadLine();

        if (!string.IsNullOrEmpty(connectMongoDb) &&
            connectMongoDb.ToLower() == "y")
        {
            Console.WriteLine("Please, pass the connection string for MongoDB");
            var connectionString = Console.ReadLine();


        }

        //You can choose your OpenAI Engine here.
        var openAi = new OpenAiIntegrator("/Resources/Metadata.json", Engine.Davinci, apiKey);
        Console.WriteLine((await openAi.GenerateParquetAsync()).Item2);
    }
}
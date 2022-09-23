// See https://aka.ms/new-console-template for more information
using OpenAI;
using OpenAIParquetGenerator.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Let's start the OpenAI Parquet/CSV Generation!");
        Console.WriteLine("Please, insert your OpenAI API key:");

        var apiKey = Console.ReadLine();

        if (apiKey is null)
        {
            Console.WriteLine("Insert a valid API Key");
            return;
        }

        //You can choose your OpenAI Engine here.
        var openAi = new OpenAiIntegrator("/Resources/Metadata.json", Engine.Davinci, apiKey);
        Console.WriteLine((await openAi.GenerateParquetAsync()).Item2);
    }
}
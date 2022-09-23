using Newtonsoft.Json;
using OpenAI;
using OpenAIParquetGenerator.Misc;
using OpenAIParquetGenerator.Resources;
using Parquet;
using Parquet.Data;
using System.Diagnostics;

namespace OpenAIParquetGenerator.Services
{
    /// <summary>
    /// Service that integrate with OpenAI using the public nuget packages.
    /// </summary>
    public class OpenAiIntegrator
    {
        private readonly OpenAIAPI api;
        private readonly string currentDirectory = Directory.GetCurrentDirectory();

        private readonly Metadata Metadata;

        public OpenAiIntegrator(string metadataPath, Engine engineType, string apiKey)
        {
            Console.WriteLine("Initiating OpenAIIntegrator");

            api = new OpenAIAPI(apiKey, engineType);

            Console.WriteLine("Validating Metadata");
            Metadata = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(currentDirectory + metadataPath));
        }

        /// <summary>
        /// Generate a Parquet following the instructions in the Metadata file.
        /// </summary>
        /// <returns>Bool - if process finished successfully; String - Message of success or error</returns>
        public async Task<(bool, string)> GenerateParquetAsync()
        {
            try
            {
                Console.WriteLine($"Generating {Metadata.RowCount} rows");

                using (Stream fileStream = File.Create(currentDirectory + "/test.parquet"))
                {
                    var dataColumns = new List<DataColumn>();

                    foreach (ColumnInfo column in Metadata.Columns)
                    {
                        Array? chunckedStringResult = null;

                        try
                        {
                            switch (column.Type)
                            {
                                case ColumnValueType.DateTime:
                                    Console.WriteLine($"Generating random DateTime for the column {column.OutputName}");

                                    chunckedStringResult = GetRandomDateTimeEnumerable(
                                    column.StartDate,
                                    column.EndDate)
                                    .ToArray();

                                    break;

                                case ColumnValueType.Int:
                                    Console.WriteLine($"Generating random Int for the column {column.OutputName}");

                                    chunckedStringResult = GetRandomIntEnumerable(
                                        column.StartNumber.GetValueOrDefault(),
                                        column.EndNumber.GetValueOrDefault())
                                        .ToArray();

                                    break;

                                default:
                                    Console.WriteLine($"Generating random int for the column {column.OutputName}");

                                    chunckedStringResult = await GetValuesFromOpenAI(column);

                                    break;
                            }

                            var columnToAppend = ConvertToDataColumn(column.OutputName, column.Type, chunckedStringResult);
                            dataColumns.Add(columnToAppend);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            continue;
                        }
                    }

                    var schema = new Schema(dataColumns.Select(d => d.Field).ToArray());

                    using (var parquetWriter = new ParquetWriter(schema, fileStream))
                    {
                        // create a new row group in the file
                        using (ParquetRowGroupWriter groupWriter = parquetWriter.CreateRowGroup())
                        {
                            dataColumns.ForEach(dataCol => groupWriter.WriteColumn(dataCol));
                        }
                    }
                }

                return (true, "Executed Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (false, ex.Message);
            }
        }

        private IEnumerable<int> GetRandomIntEnumerable(int startNumber, int endNumber)
        {
            var gen = new Random();
            var result = new List<int>();

            for (int i = 0; i < Metadata.RowCount; i++)
                result.Add(gen.Next(startNumber, endNumber));

            return result;
        }

        private async Task<Array> GetValuesFromOpenAI(ColumnInfo column)
        {
            var currentPrompt = MetadataHelper.GetPromptForRandomGeneration(
                                               promptValue: column.PromptValue,
                                               Metadata.RowCount);

            Console.WriteLine($"Working with column \"{column.OutputName}\" and with prompt \"{column.PromptValue}\"");

            var watch = new Stopwatch();

            watch.Start();

            var response = await api.Completions.CreateCompletionAsync(
                prompt: currentPrompt,
                top_p: 1,
                presencePenalty: 0,
                frequencyPenalty: 0,
                temperature: 0.7,
                max_tokens: 20 * Metadata.RowCount);

            watch.Stop();

            Console.WriteLine($"Elapsed time in Miliseconds: {watch.ElapsedMilliseconds}ms");

            var result = response
                .ToString()
                .Replace("\n", "")
                .Replace("(", "")
                .Replace(")", "")
                .Split(";")
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .ToArray();

            return result;
        }

        private IEnumerable<DateTimeOffset> GetRandomDateTimeEnumerable(
            DateTime startDateTime,
            DateTime endDateTime)
        {
            var gen = new Random();
            var result = new List<DateTimeOffset>();
            var range = (endDateTime.Subtract(startDateTime)).Days;

            for (int i = 0; i < Metadata.RowCount; i++)
            {
                var tempDate = new DateTime(startDateTime.Ticks);

                result.Add(tempDate.AddDays(gen.Next(range)));
            }

            return result;
        }

        private static DataColumn ConvertToDataColumn(
            string outputColumnName,
            string targetType,
            Array arrayValues)
        {
            return targetType.ToLower().Trim() switch
            {
                "string" => new DataColumn(new DataField<string>(outputColumnName), arrayValues),
                "int" => new DataColumn(new DataField<int>(outputColumnName), arrayValues),
                "datetime" => new DataColumn(new DataField<DateTime>(outputColumnName), arrayValues),
                "bool" => new DataColumn(new DataField<bool>(outputColumnName), arrayValues),
                _ => throw new Exception($"Type not supported for column: {outputColumnName}"),
            };
        }
    }
}
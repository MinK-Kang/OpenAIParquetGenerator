namespace OpenAIParquetGenerator.Misc
{
    public static class MetadataHelper
    {
        public static string GetPromptForRandomGeneration(string promptValue, int qty) =>
            $"Generate randomly {qty} {promptValue} separated with semicolon";

        public static Type GetFieldType(string type)
        {
            return type.ToLower().Trim() switch
            {
                "string" => typeof(string),
                "int" => typeof(int),
                "datetime" => typeof(DateTime),
                _ => throw new ArgumentException($"Type not supported : {type}"),
            };
        }
    }
}
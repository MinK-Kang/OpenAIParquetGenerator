namespace OpenAIParquetGenerator.Domain
{
    public class Metadata
    {
        /// <summary>
        /// Number of rows to be generated in the execution
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// List containing details of each column
        /// </summary>
        public List<ColumnInfo>? Columns { get; set; }

        /// <summary>
        /// Output file format. Parquet or CSV.
        /// </summary>
        public string FileFormat { get; set; }
    }

    public class ColumnInfo
    {
        /// <summary>
        /// Column name in the final output file
        /// </summary>
        public string? OutputName { get; set; }

        /// <summary>
        /// Prompt to be used with OpenAI
        /// </summary>
        public string? PromptValue { get; set; }

        /// <summary>
        /// Value type in the final output file. String, Int, Double, Bool and DateTime.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Start Date to be used to generate random DateTime
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End Date to be used to generate random DateTime
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Max output token of OpenAI response. If you are expecting long string response for each row,
        /// inform a custom value (the limit is 4000).
        /// If not informed will be used 20 * RowCount
        /// </summary>
        public int? MaxTokens { get; set; }

        /// <summary>
        /// Start number to be used in the random Int generator
        /// </summary>
        public int? StartNumber { get; set; }

        /// <summary>
        /// End number to be used in the random Int generator
        /// </summary>
        public int? EndNumber { get; set; }
    }
}
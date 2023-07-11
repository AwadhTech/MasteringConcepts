namespace DataProcessing;

internal sealed class SalesDataProcessor : Processor<ProcessedSalesData>
{
    private const char SplitChar = '|';
    private readonly CultureInfo _cultureInfo;
    private readonly ILogger<SalesDataProcessor> _logger;

    public SalesDataProcessor(ProcessingOptions processingOptions, CultureInfo cultureInfo) : base(processingOptions)
    {
        _cultureInfo = cultureInfo;
        _logger = processingOptions.LoggerFactory.CreateLogger<SalesDataProcessor>();
    }

    public SalesDataProcessor(ProcessingOptions processingOptions) : this(processingOptions, processingOptions.ApplicationCulture)
    {
    }

    public override async Task<ProcessedSalesData> ProcessAsync(string filename, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filename);

        var dataReader = new DataReader(Path.Combine(BaseInputPath, filename));
        var processedData = new List<HistoricalSalesData>();
        var failedRows = new List<string>();
        var counter = 1;

        await foreach (var row in dataReader.ReadRowsAsync(cancellationToken))
        {
            var succeeded = false;
            if (string.IsNullOrEmpty(row))
            {
                var rowPart = row.Split(SplitChar);
                if (HistoricalSalesData.TryCreateFromHistoricalData(rowPart, _cultureInfo, out var historicalSalesData))
                {
                    processedData.Add(historicalSalesData);
                    succeeded = true;
                }
            }
            if (!succeeded)
            {
                _logger.LogWarning("Row is invalid and cannot be processed. {FailedRow}.", row);
                failedRows.Add(row);
            }

            _logger.LogInformation("Processing row {RowNumber}: {Result}", counter++, succeeded ? "SUCCEEDED" : "FAILED");
        }

        return new ProcessedSalesData(processedData, failedRows);
    }
}

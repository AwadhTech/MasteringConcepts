using System.Text.RegularExpressions;

namespace DataProcessing;

internal sealed class CustomerDataProcessor : Processor<ProcessedCustomerData>
{
    private const string RegularCustomerStartCode = "BA";

    //language=regex
    private const string CustomerDataPattern = @"\[(?<data>.*?)\]";
    private static readonly Regex CustomerRegex = new (CustomerDataPattern,
        RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    public CustomerDataProcessor(ProcessingOptions processingOptions) : base(processingOptions)
    {
    }
    
    public override async Task<ProcessedCustomerData> ProcessAsync(string filename, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filename);

        var dataReader = new DataReader(Path.Combine(BaseInputPath, filename));

        var priorityCustomers = new List<HistoricalCustomerData>();
        var regularCustomers = new List<HistoricalCustomerData>();

        await foreach (var row in dataReader.ReadRowsAsync(cancellationToken))
        {
            var matches = CustomerRegex.Matches(row);

            if (matches.Count == 4)
            {
                var customerCode = matches[0].Groups["data"].Value;

                if (!Guid.TryParseExact(matches[1].Groups["data"].Value, "D",
                    out var parsedGuid))
                    continue;

                var country = matches[2].Groups["data"].Value;

                var data = new HistoricalCustomerData(parsedGuid, customerCode, country);

                var compareResult = string.CompareOrdinal(customerCode, RegularCustomerStartCode);
                if (compareResult < 0)
                {
                    priorityCustomers.Add(data);
                }
                else
                {
                    regularCustomers.Add(data);
                }
            }
        }

        return new ProcessedCustomerData(priorityCustomers, regularCustomers);
    }
}

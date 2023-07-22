namespace DataProcessing.Reporting;

internal class CustomerDataUniqueCountriesWriter : DataWriter<IEnumerable<HistoricalCustomerData>>
{
    private readonly StringComparer _stringComparer;

    public CustomerDataUniqueCountriesWriter(ProcessingOptions options, CultureInfo cultureInfo) : base(options)
    {
        _stringComparer = StringComparer.Create(cultureInfo, true);
    }

    public CustomerDataUniqueCountriesWriter(ProcessingOptions options) : this(options, options.ApplicationCulture)
    {
    }

    protected override Task WriteAsyncCore(
        string pathAndFileName, 
        IEnumerable<HistoricalCustomerData> data, 
        CancellationToken cancellationToken = default)
    {
        var countries = new SortedSet<string>(data.Select(d => d.Country), _stringComparer);
        
        // TODO - Implementation

        return Task.CompletedTask;
    }
}

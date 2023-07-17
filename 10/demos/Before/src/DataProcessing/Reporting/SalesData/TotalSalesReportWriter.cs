namespace DataProcessing.Reporting;

internal sealed class TotalSalesReportWriter : DataWriter<IEnumerable<HistoricalSalesData>>
{
    public TotalSalesReportWriter(ProcessingOptions options) : base(options)
    {
    }

    protected override Task WriteAsyncCore(
        string pathAndFileName, 
        IEnumerable<HistoricalSalesData> salesData, 
        CancellationToken cancellationToken = default)
    {
        var totalSales = ElectricalEngineeringTotalCalculator.CalculateTotalSales(salesData);
        
        // TODO - Implementation

        return Task.CompletedTask;
    }
}
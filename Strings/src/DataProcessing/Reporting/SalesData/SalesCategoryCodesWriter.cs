﻿namespace DataProcessing.Reporting.SalesData;

internal sealed class SalesCategoryCodesWriter : DataWriter<IEnumerable<HistoricalSalesData>>
{
    public SalesCategoryCodesWriter(ProcessingOptions options) : base(options)
    {
    }

    protected override Task WriteAsyncCore(
        string pathAndFileName, 
        IEnumerable<HistoricalSalesData> salesData, 
        CancellationToken cancellationToken = default)
    {
        var sortedCodes = salesData
            .Select(c => c.Category.Code)
            .Distinct()
            .OrderBy(c => c);
        return Task.CompletedTask;
    }
}

namespace DataProcessing.Reporting;

internal class SalesDataSummaryReportWriter : DataWriter<IEnumerable<HistoricalSalesData>>
{
    public SalesDataSummaryReportWriter(ProcessingOptions options) : base(options)
    {
    }

    protected override async Task WriteAsyncCore(
        string pathAndFileName, 
        IEnumerable<HistoricalSalesData> data, 
        CancellationToken cancellationToken = default)
    {
        var formattedOutput = ProduceReportString(data, cancellationToken);

        foreach (var writer in OutputWriters)
        {
            await writer.WriteDataAsync(formattedOutput, pathAndFileName, cancellationToken);
        }
    }

    private string ProduceReportString(
        IEnumerable<HistoricalSalesData> salesData, 
        CancellationToken cancellationToken = default)
    {
        //var formattedOutput = "Data exported by " + SessionContext.Forename + " " +
        //    SessionContext.Surname + Environment.NewLine;

        var formattedOutput = $"Date exported by {SessionContext.Forename} {SessionContext.Surname}{Environment.NewLine}";

        foreach (var item in salesData.OrderBy(d => d.UtcSalesDateTime))
        {
            //formattedOutput += "Date: " + item.UtcSalesDateTime.ToString("D", Options.ApplicationCulture) +
            //    Environment.NewLine;
            //formattedOutput += string.Format(Options.ApplicationCulture, "Date: {0:D}{1}", item.UtcSalesDateTime,
            //    Environment.NewLine);
            //formattedOutput += "Product Name: " + item.ProductName +
            //    Environment.NewLine;
            //formattedOutput += "Product SKU: " + item.ProductSku +
            //    Environment.NewLine;
            formattedOutput += $"Date: {item.UtcSalesDateTime.ToString("D", Options.ApplicationCulture)}{Environment.NewLine}";

            //FormattableString dateString = $"Date: {item.UtcSalesDateTime:D}{Environment.NewLine}";
            //formattedOutput += dateString.ToString(Options.ApplicationCulture);

            formattedOutput += $"Product Name: {item.ProductName}{Environment.NewLine}";
            formattedOutput += $"Product SKU: {item.ProductSku}{Environment.NewLine}";
        }

        return formattedOutput;
    }
}

namespace DataProcessing;

internal static class ElectricalEngineeringTotalCalculator
{
    private const string ElectricalEngineeringCode = "ENG001";

    public static decimal CalculateTotalSales(IEnumerable<HistoricalSalesData> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var totalSales = 0m;

        foreach (var item in data)
        {
            if (ElectricalEngineeringCode.Equals(item.Category.Code, 
                StringComparison.OrdinalIgnoreCase))
            {
                totalSales += item.TotalPrice;
            }
        }

        return totalSales;
    }
}

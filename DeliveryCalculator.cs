namespace OrderApp;

public class DeliveryCalculator
{
    public decimal CalculateShippingFee(decimal orderSubtotal, string regionCode)
    {
        if (orderSubtotal >= 1500m)
        {
            return 0m;
        }

        decimal baseRate = regionCode switch
        {
            "KYIV" => 75m,
            "LVIV" => 85m,
            "ODESA" => 90m,
            "DNIPRO" => 90m,
            "KHARKIV" => 95m,
            _ => 120m
        };

        return baseRate;
    }

    public int EstimateDeliveryDays(string regionCode, bool expressDelivery)
    {
        int standardDays = regionCode switch
        {
            "KYIV" => 1,
            "LVIV" or "ODESA" or "DNIPRO" => 2,
            _ => 3
        };

        if (expressDelivery && standardDays > 1)
        {
            return standardDays - 1;
        }

        return standardDays;
    }

    public bool IsEligibleForFreeShipping(decimal subtotal) => subtotal >= 1500m;
}

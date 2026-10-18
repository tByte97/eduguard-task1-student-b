namespace OrderApp;

public class OrderItem
{
    public string Sku { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}

public class Order
{
    public string OrderId { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = [];
    public decimal Subtotal => Items.Sum(i => i.UnitPrice * i.Quantity);
    public string PromoCode { get; set; } = string.Empty;
}

public class OrderService
{
    private readonly DeliveryCalculator _deliveryCalculator = new();

    // Розрахунок кінцевої суми замовлення клієнта
    public decimal CalculateFinalTotal(Order order, string destinationRegion, bool isVipCustomer)
    {
        if (order == null || order.Items.Count == 0)
        {
            throw new ArgumentException("Order cannot be empty or null.");
        }

        decimal discount = CalculateDiscount(order.Subtotal, order.PromoCode, isVipCustomer);
        decimal delivery = _deliveryCalculator.CalculateShippingFee(order.Subtotal, destinationRegion);
        decimal total = order.Subtotal - discount + delivery;

        if (total < 0)
        {
            return 0;
        }

        return Math.Round(total, 2);
    }

    public decimal CalculateDiscount(decimal subtotal, string promoCode, bool isVipCustomer)
    {
        decimal rate = 0.0m;

        if (isVipCustomer)
        {
            rate += 0.05m;
        }

        if (string.Equals(promoCode, "SUMMER2026", StringComparison.OrdinalIgnoreCase))
        {
            rate += 0.10m;
        }
        else if (string.Equals(promoCode, "WELCOME", StringComparison.OrdinalIgnoreCase))
        {
            rate += 0.05m;
        }

        if (subtotal > 1000m)
        {
            rate += 0.07m;
        }

        if (rate > 0.30m)
        {
            rate = 0.30m;
        }

        return subtotal * rate;
    }

    public bool IsOrderValid(Order order) => order != null && order.Items.Count > 0;
}

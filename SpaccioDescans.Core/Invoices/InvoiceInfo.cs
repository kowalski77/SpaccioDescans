namespace SpaccioDescans.Core.Invoices;

public record InvoiceInfo(HeaderInfo Header, CustomerInfo Customer, IEnumerable<OrderDetailInfo> OrderDetails, PaymentInfo Payment);

public class HeaderInfo
{
    public long InvoiceId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string FiscalId { get; init; } = string.Empty;

    public string Address { get; init; } = string.Empty;
}

public class CustomerInfo
{
    public string Name { get; init; } = string.Empty;

    public string Address { get; init; } = string.Empty;

    public string Nif { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;
}

public class OrderDetailInfo
{
    public int Quantity { get; init; }

    public string ProductDescription { get; init; } = string.Empty;

    public double NetPrice { get; init; }

    public double Discount { get; init; }

    public double Total { get; init; }
}

public class PaymentInfo
{
    public double Cash { get; init; }

    public double CreditCard { get; init; }

    public double Financed { get; init; }

    public double Net { get; init; }

    public double Total { get; init; }

    public double Pending { get; init; }
}

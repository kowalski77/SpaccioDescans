namespace SpaccioDescans.Core.Application.Services;

public class HeaderInfo
{
    public long InvoiceId { get; init; }

    public string? Name { get; init; }

    public string? FiscalId { get; init; }

    public string? Address { get; init; }
}

public class CustomerInfo
{
    public string? Name { get; init; }

    public string? Address { get; init; }

    public string? Nif { get; init; }

    public string? Phone { get; init; }
}

public class OrderDetailInfo
{
    public int Quantity { get; init; }

    public string? ProductDescription { get; init; }

    public double NetPrice { get; init; }

    public double Discount { get; init; }

    public double Total { get; init; }
}

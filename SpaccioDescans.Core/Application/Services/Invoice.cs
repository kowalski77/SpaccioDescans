namespace SpaccioDescans.Core.Application.Services;

public class Header
{
    public int InvoiceId { get; init; }

    public string? Name { get; init; }

    public string? FiscalId { get; init; }

    public string? Address { get; init; }

    public string? City { get; init; }
}

public class CustomerInfo
{
    public string? Name { get; init; }

    public string? Address { get; init; }

    public string? Nif { get; init; }

    public string? Phone { get; init; }
}

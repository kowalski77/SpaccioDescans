using SpaccioDescans.Core.Services;

namespace SpaccioDescans.Web.Invoices;

public sealed class InvoiceFactory
{
    private readonly string filePath = Path.Combine("Files", "invoices.xls");

    public MemoryStream GetInvoiceStream(InvoiceInfo info, InvoiceType invoiceType)
    {
        ArgumentNullException.ThrowIfNull(info);

        using var invoiceBuilder = InvoiceBuilder.Create(this.filePath, GetInvoiceParser(invoiceType));

        var stream = invoiceBuilder
            .AddHeader(info.Header)
            .AddCustomer(info.Customer)
            .AddOrderDetails(info.OrderDetails)
            .AddPayment(info.Payment)
            .Build();

        return stream;
    }

    private static InvoiceParser GetInvoiceParser(InvoiceType invoiceType)
    {
        return invoiceType switch
        {
            InvoiceType.DeliveryNote => new DeliveryNoteParser(),
            InvoiceType.CustomerInvoice => new CustomerInvoiceParser(),
            _ => throw new ArgumentException($"Unknown invoice type: {invoiceType}"),
        };
    }
}

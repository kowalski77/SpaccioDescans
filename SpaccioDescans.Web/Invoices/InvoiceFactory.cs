using SpaccioDescans.Core.Services;

namespace SpaccioDescans.Web.Invoices;

public sealed class InvoiceFactory
{
    private readonly string filePath = Path.Combine("Files", "invoices.xls");

    private readonly IInvoiceBuilder invoiceBuilder;

    public InvoiceFactory(IInvoiceBuilder invoiceBuilder)
    {
        this.invoiceBuilder = invoiceBuilder ?? throw new ArgumentNullException(nameof(invoiceBuilder));
    }

    public MemoryStream GetInvoiceStream(InvoiceInfo info, InvoiceType invoiceType)
    {
        ArgumentNullException.ThrowIfNull(info);

        var stream = invoiceBuilder
            .SetParser(GetInvoiceParser(invoiceType))
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

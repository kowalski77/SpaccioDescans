using SpaccioDescans.Core.Services;

namespace SpaccioDescans.Web.Invoices;

public sealed class InvoiceFactory
{
    private readonly IInvoiceBuilder invoiceBuilder;

    public InvoiceFactory(IInvoiceBuilder invoiceBuilder)
    {
        this.invoiceBuilder = invoiceBuilder ?? throw new ArgumentNullException(nameof(invoiceBuilder));
    }

    public MemoryStream GetInvoiceStream(InvoiceInfo info, InvoiceType invoiceType)
    {
        ArgumentNullException.ThrowIfNull(info);

        var stream = invoiceBuilder
            .AddHeader(info.Header)
            .AddCustomer(info.Customer)
            .AddOrderDetails(info.OrderDetails)
            .AddPayment(info.Payment)
            .WithParser(GetInvoiceParser(invoiceType))
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

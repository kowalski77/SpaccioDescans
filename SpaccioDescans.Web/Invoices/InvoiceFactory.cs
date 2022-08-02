namespace SpaccioDescans.Web.Invoices;

public class InvoiceFactory
{
    private readonly Dictionary<InvoiceType, IInvoiceProvider> strategies;

    public InvoiceFactory(Dictionary<InvoiceType, IInvoiceProvider> strategies)
    {
        this.strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
    }

    public IInvoiceProvider CreateInvoiceProvider(InvoiceType invoiceType)
    {
        return this.strategies.TryGetValue(invoiceType, out var strategy) ?
            strategy :
            throw new InvalidOperationException("Could not create the invoice provider");
    }
}

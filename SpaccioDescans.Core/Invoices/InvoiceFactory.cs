namespace SpaccioDescans.Core.Invoices;

public class InvoiceFactory
{
    private readonly Dictionary<Type, IInvoice> strategies;

    public InvoiceFactory(Dictionary<Type, IInvoice> strategies)
    {
        this.strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
    }

    public MemoryStream GetInvoice<T>(InvoiceInfo info)
        where T : IInvoice
    {
        ArgumentNullException.ThrowIfNull(info);

        if(this.strategies.TryGetValue(typeof(T), out var strategy))
        {
            return strategy.Create(info);
        }
        
        throw new InvalidOperationException($"No strategy registered for type: {typeof(T)}");
    }
}

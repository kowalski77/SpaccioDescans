using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Invoices;

public abstract class Invoice : IInvoice
{
    private readonly IWorkbookFacade invoiceFacade;

    protected Invoice(IWorkbookFacade invoiceFacade)
    {
        this.invoiceFacade = invoiceFacade ?? throw new ArgumentNullException(nameof(invoiceFacade));
    }

    protected IWorksheet Worksheet { get; private set; } = default!;

    protected abstract int WorksheetNumber { get; }

    public abstract void AddHeader(HeaderInfo header);

    public abstract void AddCustomer(CustomerInfo customerInfo);

    public abstract void AddOrderDetails(IEnumerable<OrderDetailInfo> orderInfos);

    public abstract void AddPayment(PaymentInfo paymentInfo);

    public MemoryStream Create(InvoiceInfo invoiceInfo)
    {
        ArgumentNullException.ThrowIfNull(invoiceInfo);
        
        this.Worksheet = this.invoiceFacade.Workbook.Worksheets[this.WorksheetNumber];

        this.AddHeader(invoiceInfo.Header);
        this.AddCustomer(invoiceInfo.Customer);
        this.AddOrderDetails(invoiceInfo.OrderDetails);
        this.AddPayment(invoiceInfo.Payment);

        using var stream = new MemoryStream();

        this.invoiceFacade.Workbook.SaveAs(stream);
        
        return stream;
    }
}

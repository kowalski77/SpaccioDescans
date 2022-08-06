namespace SpaccioDescans.Core.Invoices;

public interface IInvoice
{
    MemoryStream Create(InvoiceInfo invoiceInfo);
}

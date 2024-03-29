﻿namespace SpaccioDescans.Core.Invoices;

public abstract class Invoice : IInvoice
{
    protected Invoice(IWorkbookFacade invoiceFacade)
    {
        this.WorkbookFacade = invoiceFacade ?? throw new ArgumentNullException(nameof(invoiceFacade));
    }

    protected abstract int WorksheetNumber { get; }

    public IWorkbookFacade WorkbookFacade { get; }

    public abstract void AddHeader(HeaderInfo header);

    public abstract void AddCustomer(CustomerInfo customerInfo);

    public abstract void AddOrderDetails(IEnumerable<OrderDetailInfo> orderInfos);

    public abstract void AddPayment(PaymentInfo paymentInfo);

    public MemoryStream Create(InvoiceInfo invoiceInfo)
    {
        ArgumentNullException.ThrowIfNull(invoiceInfo);

        this.WorkbookFacade.SetWorksheetNumber(this.WorksheetNumber);

        this.AddHeader(invoiceInfo.Header);
        this.AddCustomer(invoiceInfo.Customer);
        this.AddOrderDetails(invoiceInfo.OrderDetails);
        this.AddPayment(invoiceInfo.Payment);

        var stream = this.WorkbookFacade.Save();

        return stream;
    }
}

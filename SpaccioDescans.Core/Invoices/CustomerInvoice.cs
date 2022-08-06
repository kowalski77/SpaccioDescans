namespace SpaccioDescans.Core.Invoices;

public class CustomerInvoice : Invoice
{
    public CustomerInvoice(IWorkbookFacade invoiceFacade)
        : base(invoiceFacade)
    {
    }

    protected override int WorksheetNumber => 4;

    public override void AddHeader(HeaderInfo header)
    {
        ArgumentNullException.ThrowIfNull(header);

        this.InvoiceFacade.AddText("B4", header.Name);
        this.InvoiceFacade.AddText("L4", header.FiscalId);
        this.InvoiceFacade.AddNumber("N4", header.InvoiceId);
        this.InvoiceFacade.AddText("B6", header.Address);
    }

    public override void AddCustomer(CustomerInfo customerInfo)
    {
        ArgumentNullException.ThrowIfNull(customerInfo);

        this.InvoiceFacade.AddText("M10", customerInfo.Name);
        this.InvoiceFacade.AddText("M11", customerInfo.Nif);
        this.InvoiceFacade.AddText("M13", customerInfo.Address);
        this.InvoiceFacade.AddText("M14", customerInfo.Phone);
    }

    public override void AddOrderDetails(IEnumerable<OrderDetailInfo> orderInfos)
    {
        ArgumentNullException.ThrowIfNull(orderInfos);

        var row = 19;
        foreach (var orderInfo in orderInfos)
        {
            this.InvoiceFacade.AddNumber($"B{row}", orderInfo.Quantity);
            this.InvoiceFacade.AddText($"F{row}", orderInfo.ProductDescription);
            this.InvoiceFacade.AddNumber($"M{row}", orderInfo.NetPrice);
            this.InvoiceFacade.AddNumber($"N{row}", orderInfo.Discount);
            this.InvoiceFacade.AddNumber($"R{row}", orderInfo.Total);
            row++;
        }
    }

    public override void AddPayment(PaymentInfo paymentInfo)
    {
        ArgumentNullException.ThrowIfNull(paymentInfo);

        this.InvoiceFacade.AddNumber("R44", paymentInfo.Net);
        this.InvoiceFacade.AddNumber("R49", paymentInfo.Total);
    }
}

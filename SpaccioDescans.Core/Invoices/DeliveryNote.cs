namespace SpaccioDescans.Core.Invoices;

public class DeliveryNote : Invoice
{
    public DeliveryNote(IWorkbookFacade invoiceFacade) 
        : base(invoiceFacade)
    {
    }

    protected override int WorksheetNumber => 5;

    public override void AddHeader(HeaderInfo header)
    {
        ArgumentNullException.ThrowIfNull(header);

        this.InvoiceFacade.AddNumber("S7", (int)header.InvoiceId);
        this.InvoiceFacade.AddText("C3", header.Name);
        this.InvoiceFacade.AddText("G4", header.FiscalId);
        this.InvoiceFacade.AddText("C6", header.Address);
    }

    public override void AddCustomer(CustomerInfo customerInfo)
    {
        ArgumentNullException.ThrowIfNull(customerInfo);

        this.InvoiceFacade.AddText("N10", customerInfo.Name);
        this.InvoiceFacade.AddText("N11", customerInfo.Nif);
        this.InvoiceFacade.AddText("N12", customerInfo.Address);
        this.InvoiceFacade.AddText("N13", customerInfo.Phone);
    }

    public override void AddOrderDetails(IEnumerable<OrderDetailInfo> orderInfos)
    {
        ArgumentNullException.ThrowIfNull(orderInfos);

        var row = 16;
        foreach (var orderInfo in orderInfos)
        {
            this.InvoiceFacade.AddNumber($"C{row}", orderInfo.Quantity);
            this.InvoiceFacade.AddText($"E{row}", orderInfo.ProductDescription);
            this.InvoiceFacade.AddNumber($"N{row}", orderInfo.NetPrice);
            this.InvoiceFacade.AddNumber($"O{row}", orderInfo.Discount);
            this.InvoiceFacade.AddNumber($"S{row}", orderInfo.Total);

            row++;
        }
    }

    public override void AddPayment(PaymentInfo paymentInfo)
    {
        ArgumentNullException.ThrowIfNull(paymentInfo);

        this.InvoiceFacade.AddNumber("S38", paymentInfo.Net);
        this.InvoiceFacade.AddNumber("S40", paymentInfo.Total);
        this.InvoiceFacade.AddNumber("S43", paymentInfo.Pending);
        this.InvoiceFacade.AddNumber("H40", paymentInfo.Cash);
        this.InvoiceFacade.AddNumber("H41", paymentInfo.CreditCard);
        this.InvoiceFacade.AddNumber("H42", paymentInfo.Financed);
    }
}

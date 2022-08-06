namespace SpaccioDescans.Core.Invoices;

public class DeliveryNote : Invoice
{
    public DeliveryNote(IWorkbookFacade workbookFacade) 
        : base(workbookFacade)
    {
    }

    protected override int WorksheetNumber => 5;

    public override void AddHeader(HeaderInfo header)
    {
        ArgumentNullException.ThrowIfNull(header);

        this.WorkbookFacade.AddNumber("S7", (int)header.InvoiceId);
        this.WorkbookFacade.AddText("C3", header.Name);
        this.WorkbookFacade.AddText("G4", header.FiscalId);
        this.WorkbookFacade.AddText("C6", header.Address);
    }

    public override void AddCustomer(CustomerInfo customerInfo)
    {
        ArgumentNullException.ThrowIfNull(customerInfo);

        this.WorkbookFacade.AddText("N10", customerInfo.Name);
        this.WorkbookFacade.AddText("N11", customerInfo.Nif);
        this.WorkbookFacade.AddText("N12", customerInfo.Address);
        this.WorkbookFacade.AddText("N13", customerInfo.Phone);
    }

    public override void AddOrderDetails(IEnumerable<OrderDetailInfo> orderInfos)
    {
        ArgumentNullException.ThrowIfNull(orderInfos);

        var row = 16;
        foreach (var orderInfo in orderInfos)
        {
            this.WorkbookFacade.AddNumber($"C{row}", orderInfo.Quantity);
            this.WorkbookFacade.AddText($"E{row}", orderInfo.ProductDescription);
            this.WorkbookFacade.AddNumber($"N{row}", orderInfo.NetPrice);
            this.WorkbookFacade.AddNumber($"O{row}", orderInfo.Discount);
            this.WorkbookFacade.AddNumber($"S{row}", orderInfo.Total);

            row++;
        }
    }

    public override void AddPayment(PaymentInfo paymentInfo)
    {
        ArgumentNullException.ThrowIfNull(paymentInfo);

        this.WorkbookFacade.AddNumber("S38", paymentInfo.Net);
        this.WorkbookFacade.AddNumber("S40", paymentInfo.Total);
        this.WorkbookFacade.AddNumber("S43", paymentInfo.Pending);
        this.WorkbookFacade.AddNumber("H40", paymentInfo.Cash);
        this.WorkbookFacade.AddNumber("H41", paymentInfo.CreditCard);
        this.WorkbookFacade.AddNumber("H42", paymentInfo.Financed);
    }
}

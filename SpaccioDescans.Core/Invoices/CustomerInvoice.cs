namespace SpaccioDescans.Core.Invoices;

public class CustomerInvoice : Invoice
{
    public CustomerInvoice(IWorkbookFacade workbookFacade)
        : base(workbookFacade)
    {
    }

    protected override int WorksheetNumber => 4;

    public override void AddHeader(HeaderInfo header)
    {
        ArgumentNullException.ThrowIfNull(header);

        this.WorkbookFacade.AddText("B4", header.Name);
        this.WorkbookFacade.AddText("L4", header.FiscalId);
        this.WorkbookFacade.AddNumber("N4", header.InvoiceId);
        this.WorkbookFacade.AddText("B6", header.Address);
    }

    public override void AddCustomer(CustomerInfo customerInfo)
    {
        ArgumentNullException.ThrowIfNull(customerInfo);

        this.WorkbookFacade.AddText("M10", customerInfo.Name);
        this.WorkbookFacade.AddText("M11", customerInfo.Nif);
        this.WorkbookFacade.AddText("M13", customerInfo.Address);
        this.WorkbookFacade.AddText("M14", customerInfo.Phone);
    }

    public override void AddOrderDetails(IEnumerable<OrderDetailInfo> orderInfos)
    {
        ArgumentNullException.ThrowIfNull(orderInfos);

        var row = 19;
        foreach (var orderInfo in orderInfos)
        {
            this.WorkbookFacade.AddNumber($"B{row}", orderInfo.Quantity);
            this.WorkbookFacade.AddText($"F{row}", orderInfo.ProductDescription);
            this.WorkbookFacade.AddNumber($"M{row}", orderInfo.NetPrice);
            this.WorkbookFacade.AddNumber($"N{row}", orderInfo.Discount);
            this.WorkbookFacade.AddNumber($"R{row}", orderInfo.Total);
            row++;
        }
    }

    public override void AddPayment(PaymentInfo paymentInfo)
    {
        ArgumentNullException.ThrowIfNull(paymentInfo);

        this.WorkbookFacade.AddNumber("R44", paymentInfo.Net);
        this.WorkbookFacade.AddNumber("R49", paymentInfo.Total);
    }
}

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

        this.Worksheet.Range["B4"].Text = header.Name;
        this.Worksheet.Range["L4"].Text = header.FiscalId;
        this.Worksheet.Range["N4"].Number = header.InvoiceId;
        this.Worksheet.Range["B6"].Text = header.Address;
    }

    public override void AddCustomer(CustomerInfo customerInfo)
    {
        ArgumentNullException.ThrowIfNull(customerInfo);

        this.Worksheet.Range["M10"].Text = customerInfo.Name;
        this.Worksheet.Range["M11"].Text = customerInfo.Nif;
        this.Worksheet.Range["M13"].Text = customerInfo.Address;
        this.Worksheet.Range["M14"].Text = customerInfo.Phone;
    }

    public override void AddOrderDetails(IEnumerable<OrderDetailInfo> orderInfos)
    {
        ArgumentNullException.ThrowIfNull(orderInfos);

        var row = 19;
        foreach (var orderInfo in orderInfos)
        {
            this.Worksheet.Range[$"B{row}"].Number = orderInfo.Quantity;
            this.Worksheet.Range[$"F{row}"].Text = orderInfo.ProductDescription;
            this.Worksheet.Range[$"M{row}"].Number = orderInfo.NetPrice;
            this.Worksheet.Range[$"N{row}"].Number = orderInfo.Discount;
            this.Worksheet.Range[$"R{row}"].Number = orderInfo.Total;
            row++;
        }
    }

    public override void AddPayment(PaymentInfo paymentInfo)
    {
        ArgumentNullException.ThrowIfNull(paymentInfo);

        this.Worksheet.Range["R44"].Number = paymentInfo.Net;
        this.Worksheet.Range["R49"].Number = paymentInfo.Total;
    }
}

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

        this.Worksheet.Range["S7"].Number = header.InvoiceId;
        this.Worksheet.Range["C3"].Text = header.Name;
        this.Worksheet.Range["G4"].Text = header.FiscalId;
        this.Worksheet.Range["C6"].Text = header.Address;
    }

    public override void AddCustomer(CustomerInfo customerInfo)
    {
        ArgumentNullException.ThrowIfNull(customerInfo);

        this.Worksheet.Range["N10"].Text = customerInfo.Name;
        this.Worksheet.Range["N11"].Text = customerInfo.Nif;
        this.Worksheet.Range["N12"].Text = customerInfo.Address;
        this.Worksheet.Range["N13"].Text = customerInfo.Phone;
    }

    public override void AddOrderDetails(IEnumerable<OrderDetailInfo> orderInfos)
    {
        ArgumentNullException.ThrowIfNull(orderInfos);

        var row = 16;
        foreach (var orderInfo in orderInfos)
        {
            this.Worksheet.Range[$"C{row}"].Number = orderInfo.Quantity;
            this.Worksheet.Range[$"E{row}"].Text = orderInfo.ProductDescription;
            this.Worksheet.Range[$"N{row}"].Number = orderInfo.NetPrice;
            this.Worksheet.Range[$"O{row}"].Number = orderInfo.Discount;
            this.Worksheet.Range[$"S{row}"].Number = orderInfo.Total;
            row++;
        }
    }

    public override void AddPayment(PaymentInfo paymentInfo)
    {
        ArgumentNullException.ThrowIfNull(paymentInfo);

        this.Worksheet.Range["S38"].Number = paymentInfo.Net;
        this.Worksheet.Range["S40"].Number = paymentInfo.Total;
        this.Worksheet.Range["S43"].Number = paymentInfo.Pending;
        this.Worksheet.Range["H40"].Number = paymentInfo.Cash;
        this.Worksheet.Range["H41"].Number = paymentInfo.CreditCard;
        this.Worksheet.Range["H42"].Number = paymentInfo.Financed;
    }
}

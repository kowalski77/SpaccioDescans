using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Services;

public abstract class InvoiceParser
{
    protected InvoiceParser(int worksheetNumber)
    {
        this.WorksheetNumber = worksheetNumber;
    }

    public int WorksheetNumber { get; set; }

    public abstract void ParseHeader(IWorksheet worksheet, HeaderInfo header);

    public abstract void ParseCustomer(IWorksheet worksheet, CustomerInfo customerInfo);

    public abstract void ParseOrderDetail(IWorksheet worksheet, IEnumerable<OrderDetailInfo> orderInfos);

    public abstract void ParsePayment(IWorksheet worksheet, PaymentInfo paymentInfo);
}

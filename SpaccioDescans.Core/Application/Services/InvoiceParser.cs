using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Application.Services;

public abstract class InvoiceParser
{
    protected InvoiceParser(int worksheetNumber)
    {
        this.WorksheetNumber = worksheetNumber;
    }

    public int WorksheetNumber { get; set; }

    public abstract void ParseHeader(IWorksheet worksheet, Header header);

    public abstract void ParseCustomer(IWorksheet worksheet, CustomerInfo customerInfo);
}

using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Services;

public sealed class InvoiceBuilder : IInvoiceBuilder
{
    private readonly ExcelEngine excelEngine;
    private readonly FileStream fileStream;
    private readonly IApplication application;
    private readonly IWorkbook workbook;
    
    private IWorksheet worksheet;
    private InvoiceParser invoiceParser;

    public InvoiceBuilder(string invoiceTemplatePath)
    {
        if (string.IsNullOrWhiteSpace(invoiceTemplatePath))
        {
            throw new ArgumentException($"'{nameof(invoiceTemplatePath)}' cannot be null or whitespace.", nameof(invoiceTemplatePath));
        }

        this.excelEngine = new ExcelEngine();
        this.application = excelEngine.Excel;
        this.application.DefaultVersion = ExcelVersion.Excel97to2003;

        this.fileStream = new FileStream(invoiceTemplatePath, FileMode.Open);
        this.workbook = this.application.Workbooks.Open(this.fileStream, ExcelParseOptions.ParseWorksheetsOnDemand);

        // default parser is DeliverNote
        this.invoiceParser = new DeliveryNoteParser();
        this.worksheet = workbook.Worksheets[this.invoiceParser.WorksheetNumber];
    }

    public IInvoiceBuilder SetParser(InvoiceParser invoiceParser)
    {
        this.invoiceParser = invoiceParser;
        this.worksheet = workbook.Worksheets[this.invoiceParser.WorksheetNumber];

        return this;
    }

    public IInvoiceBuilder AddHeader(HeaderInfo header)
    {
        ArgumentNullException.ThrowIfNull(header);

        this.invoiceParser.ParseHeader(this.worksheet, header);

        return this;
    }

    public IInvoiceBuilder AddCustomer(CustomerInfo customerInfo)
    {
        ArgumentNullException.ThrowIfNull(customerInfo);

        this.invoiceParser.ParseCustomer(this.worksheet, customerInfo);

        return this;
    }

    public IInvoiceBuilder AddOrderDetails(IEnumerable<OrderDetailInfo> orderDetailInfos)
    {
        ArgumentNullException.ThrowIfNull(orderDetailInfos);

        this.invoiceParser.ParseOrderDetail(this.worksheet, orderDetailInfos);

        return this;
    }

    public IInvoiceBuilder AddPayment(PaymentInfo payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        this.invoiceParser.ParsePayment(this.worksheet, payment);

        return this;
    }

    public MemoryStream Build()
    {
        using var stream = new MemoryStream();

        workbook!.SaveAs(stream);

        return stream;
    }

    public void Dispose()
    {
        this.workbook?.Close();
        this.excelEngine?.Dispose();
        this.fileStream?.Dispose();
    }
}

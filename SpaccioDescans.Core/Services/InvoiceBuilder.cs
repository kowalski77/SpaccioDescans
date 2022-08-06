using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Services;

public sealed class InvoiceBuilder : IInvoiceBuilder
{
    private readonly ExcelEngine excelEngine;
    private readonly FileStream fileStream;
    private readonly IApplication application;
    private readonly IWorkbook workbook;

    private readonly List<Action> worksheetActions = new();

    private IWorksheet worksheet = default!;
    private InvoiceParser invoiceParser = default!;
    private Action? parserAction;

    public InvoiceBuilder(string invoiceTemplatePath)
    {
        if (string.IsNullOrWhiteSpace(invoiceTemplatePath))
        {
            throw new ArgumentException($"'{nameof(invoiceTemplatePath)}' cannot be null or whitespace.", nameof(invoiceTemplatePath));
        }

        this.excelEngine = new ExcelEngine();
        this.application = this.excelEngine.Excel;
        this.application.DefaultVersion = ExcelVersion.Excel97to2003;

        this.fileStream = new FileStream(invoiceTemplatePath, FileMode.Open);
        this.workbook = this.application.Workbooks.Open(this.fileStream, ExcelParseOptions.ParseWorksheetsOnDemand);

        this.SetDefaultParser();
    }

    public IInvoiceBuilder WithParser(InvoiceParser invoiceParser)
    {
        void ParserAction()
        {
            this.invoiceParser = invoiceParser;
            this.worksheet = this.workbook.Worksheets[this.invoiceParser.WorksheetNumber];
        }

        this.parserAction = ParserAction;

        return this;
    }

    public IInvoiceBuilder AddHeader(HeaderInfo header)
    {
        ArgumentNullException.ThrowIfNull(header);

        this.worksheetActions.Add(() => this.invoiceParser.ParseHeader(this.worksheet, header));

        return this;
    }

    public IInvoiceBuilder AddCustomer(CustomerInfo customerInfo)
    {
        ArgumentNullException.ThrowIfNull(customerInfo);

        this.worksheetActions.Add(() => this.invoiceParser.ParseCustomer(this.worksheet, customerInfo));

        return this;
    }

    public IInvoiceBuilder AddOrderDetails(IEnumerable<OrderDetailInfo> orderDetailInfos)
    {
        ArgumentNullException.ThrowIfNull(orderDetailInfos);

        this.worksheetActions.Add(() => this.invoiceParser.ParseOrderDetail(this.worksheet, orderDetailInfos));

        return this;
    }

    public IInvoiceBuilder AddPayment(PaymentInfo payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        this.worksheetActions.Add(() => this.invoiceParser.ParsePayment(this.worksheet, payment));

        return this;
    }

    public MemoryStream Build()
    {
        this.parserAction?.Invoke();
        foreach (var worksheetAction in this.worksheetActions)
        {
            worksheetAction.Invoke();
        }
        
        this.SetDefaultParser();
        this.worksheetActions.Clear();

        using var stream = new MemoryStream();

        this.workbook!.SaveAs(stream);

        return stream;
    }

    public void Dispose()
    {
        this.workbook?.Close();
        this.excelEngine?.Dispose();
        this.fileStream?.Dispose();
    }

    private void SetDefaultParser()
    {
        this.invoiceParser = new DeliveryNoteParser();
        this.worksheet = this.workbook.Worksheets[this.invoiceParser.WorksheetNumber];
    }
}

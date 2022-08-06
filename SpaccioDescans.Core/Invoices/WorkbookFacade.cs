using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Invoices;

public sealed class WorkbookFacade : IWorkbookFacade, IDisposable
{
    private readonly ExcelEngine excelEngine;
    private readonly FileStream fileStream;

    private readonly Lazy<IWorkbook> workbookLazy;

    public WorkbookFacade(InvoiceSettings invoiceSettings)
    {
        ArgumentNullException.ThrowIfNull(invoiceSettings);

        this.excelEngine = new ExcelEngine();
        var application = this.excelEngine.Excel;
        application.DefaultVersion = ExcelVersion.Excel97to2003;

        this.fileStream = new FileStream(invoiceSettings.TemplatePath, FileMode.Open);
        this.workbookLazy = new Lazy<IWorkbook>(() => application.Workbooks.Open(this.fileStream, ExcelParseOptions.ParseWorksheetsOnDemand));
    }

    public IWorkbook Workbook => this.workbookLazy.Value;
    
    public void Dispose()
    {
        this.Workbook?.Close();
        this.excelEngine?.Dispose();
        this.fileStream?.Dispose();
    }
}

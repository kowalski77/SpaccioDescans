using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Invoices;

public sealed class SyncfusionWorkbookFacade : IWorkbookFacade, IDisposable
{
    private int worksheetNumber;

    private readonly ExcelEngine excelEngine;
    private readonly FileStream fileStream;

    private readonly Lazy<IWorkbook> workbookLazy;

    public SyncfusionWorkbookFacade(InvoiceSettings invoiceSettings)
    {
        ArgumentNullException.ThrowIfNull(invoiceSettings);

        this.excelEngine = new ExcelEngine();
        var application = this.excelEngine.Excel;
        application.DefaultVersion = ExcelVersion.Excel97to2003;

        this.fileStream = new FileStream(invoiceSettings.TemplatePath, FileMode.Open);
        this.workbookLazy = new Lazy<IWorkbook>(() => application.Workbooks.Open(this.fileStream, ExcelParseOptions.ParseWorksheetsOnDemand));
    }

    public IWorkbook Workbook => this.workbookLazy.Value;

    public void SetWorksheetNumber(int worksheetNumber)
    {
        this.worksheetNumber = worksheetNumber;
    }

    public void AddText(string cell, string text)
    {
        var worksheet = this.Workbook.Worksheets[this.worksheetNumber];
        worksheet.Range[cell].Text = text;
    }

    public void AddNumber(string cell, double number)
    {
        var worksheet = this.Workbook.Worksheets[this.worksheetNumber];
        worksheet.Range[cell].Number = number;
    }

    public MemoryStream Save()
    {
        using var stream = new MemoryStream();

        this.Workbook.SaveAs(stream);

        return stream;
    }

    public void Dispose()
    {
        this.Workbook?.Close();
        this.excelEngine?.Dispose();
        this.fileStream?.Dispose();
    }
}

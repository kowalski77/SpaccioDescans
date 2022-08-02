using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Application.Services;

public sealed class InvoiceBuilder : IInvoiceBuilder
{
    private readonly ExcelEngine? excelEngine;
    private readonly FileStream? fileStream;
    private readonly IApplication? application;
    private readonly IWorkbook? workbook;
    
    private IWorksheet? worksheet;

    private InvoiceBuilder(string invoiceTemplatePath)
    {
        this.excelEngine = new ExcelEngine();
        this.application = excelEngine.Excel;
        this.application.DefaultVersion = ExcelVersion.Excel97to2003;
        
        this.fileStream = new FileStream(invoiceTemplatePath, FileMode.Open);
        this.workbook = this.application.Workbooks.Open(this.fileStream);
        this.worksheet = workbook.Worksheets[0];
    }

    public static IInvoiceBuilder Create(string invoiceTemplatePath)
    {
        return new InvoiceBuilder(invoiceTemplatePath);
    }

    public IInvoiceBuilder SetExcelVersion(ExcelVersion version)
    {
        ArgumentNullException.ThrowIfNull(version);

        this.application!.DefaultVersion = version;

        return this;
    }

    public IInvoiceBuilder SetWorksheet(int worksheet)
    {
        this.worksheet = this.workbook!.Worksheets[worksheet];

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
        this.excelEngine?.Dispose();
        this.fileStream?.Dispose();
    }
}

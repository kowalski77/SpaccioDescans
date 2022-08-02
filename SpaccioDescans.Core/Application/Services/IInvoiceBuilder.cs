using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Application.Services;

public interface IInvoiceBuilder : IDisposable
{
    IInvoiceBuilder SetExcelVersion(ExcelVersion version);
    
    IInvoiceBuilder SetWorksheet(int worksheet);

    MemoryStream Build();
    IInvoiceBuilder AddHeader(Header header);
}

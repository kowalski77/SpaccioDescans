using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Application.Services;

public interface IInvoiceBuilder : IDisposable
{
    IInvoiceBuilder SetExcelVersion(ExcelVersion version);

    IInvoiceBuilder AddHeader(Header header);

    IInvoiceBuilder AddCustomer(CustomerInfo customerInfo);

    MemoryStream Build();
}

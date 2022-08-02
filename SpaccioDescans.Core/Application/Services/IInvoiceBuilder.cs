using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Application.Services;

public interface IInvoiceBuilder : IDisposable
{
    IInvoiceBuilder SetExcelVersion(ExcelVersion version);

    IInvoiceBuilder AddHeader(HeaderInfo header);

    IInvoiceBuilder AddCustomer(CustomerInfo customerInfo);

    IInvoiceBuilder AddOrderDetails(IEnumerable<OrderDetailInfo> orderDetailInfos);

    MemoryStream Build();
}

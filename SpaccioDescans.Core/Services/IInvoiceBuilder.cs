using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Services;

public interface IInvoiceBuilder : IDisposable
{
    IInvoiceBuilder SetParser(InvoiceParser invoiceParser);

    IInvoiceBuilder AddHeader(HeaderInfo header);

    IInvoiceBuilder AddCustomer(CustomerInfo customerInfo);

    IInvoiceBuilder AddOrderDetails(IEnumerable<OrderDetailInfo> orderDetailInfos);

    IInvoiceBuilder AddPayment(PaymentInfo payment);

    MemoryStream Build();
}

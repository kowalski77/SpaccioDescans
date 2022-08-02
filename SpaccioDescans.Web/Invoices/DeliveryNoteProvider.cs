using SpaccioDescans.Core.Application.Services;
using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.Web.Pages.Orders.ViewModels;

namespace SpaccioDescans.Web.Invoices;

public sealed class DeliveryNoteProvider : InvoiceProvider, IInvoiceProvider
{
    public MemoryStream GetInvoiceStream(Store store, OrderViewModel order)
    {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(order);

        var headerInfo = GetHeaderInfoAsync(store, order);
        var customerInfo = GetCustomerInfo(order);
        var orderDetailInfo = GetOrderDetailInfo(order);

        using var invoiceBuilder = InvoiceBuilder.Create(this.FilePath, new DeliveryNoteParser());

        var stream = invoiceBuilder
            .AddHeader(headerInfo)
            .AddCustomer(customerInfo)
            .AddOrderDetails(orderDetailInfo)
            .Build();

        return stream;
    }

    private static HeaderInfo GetHeaderInfoAsync(Store store, OrderViewModel order)
    {
        return new HeaderInfo
        {
            Name = store.Owner,
            Address = store.Address,
            FiscalId = store.Nif,
            InvoiceId = order.Id
        };
    }

    private static CustomerInfo GetCustomerInfo(OrderViewModel order)
    {
        return new CustomerInfo
        {
            Nif = order.CustomerInfo.Nif,
            Name = order.CustomerInfo.Name,
            Address = order.CustomerInfo.Address,
            Phone = order.CustomerInfo.Phone
        };
    }

    private static IEnumerable<OrderDetailInfo> GetOrderDetailInfo(OrderViewModel order)
    {
        return order.OrderDetail.Select(detail => new OrderDetailInfo
        {
            Quantity = detail.Quantity,
            ProductDescription = detail.Name,
            Discount = (double)detail.Discount,
            NetPrice = (double)detail.Price,
            Total = (double)detail.Total
        });
    }
}

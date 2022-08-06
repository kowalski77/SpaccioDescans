using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.Core.Invoices;
using SpaccioDescans.Web.Pages.Orders.ViewModels;

namespace SpaccioDescans.Web.Invoices;

public static class InvoiceMappers
{
    public static InvoiceInfo Map(OrderViewModel order, Store store)
    {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(order);

        return new InvoiceInfo(
            MapHeaderInfo(order, store), 
            MapCustomerInfo(order), 
            MapOrderDetailsInfo(order), 
            MapPaymentInfo(order));
    }

    
    private static HeaderInfo MapHeaderInfo(OrderViewModel order, Store store)
    {
        return new HeaderInfo
        {
            Name = store.Owner,
            Address = store.Address,
            FiscalId = store.Nif,
            InvoiceId = order.Id
        };
    }

    private static CustomerInfo MapCustomerInfo(OrderViewModel order)
    {
        return new CustomerInfo
        {
            Nif = order.CustomerInfo.Nif,
            Name = order.CustomerInfo.Name,
            Address = order.CustomerInfo.Address,
            Phone = order.CustomerInfo.Phone
        };
    }

    private static IEnumerable<OrderDetailInfo> MapOrderDetailsInfo(OrderViewModel order)
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

    private static PaymentInfo MapPaymentInfo(OrderViewModel order)
    {
        return new PaymentInfo
        {
            Cash = (double)order.CashAmount,
            CreditCard = (double)order.CreditCardAmount,
            Financed = (double)order.FinancedAmount,
            Net = (double)order.NetAmount,
            Total = (double)order.TotalAmount,
            Pending = (double)order.PendingAmount
        };
    }
}

using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.Web.Pages.Orders.ViewModels;

namespace SpaccioDescans.Web.Invoices;

public interface IInvoiceProvider
{
    MemoryStream GetInvoiceStream(Store store, OrderViewModel order);
}

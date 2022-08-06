using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaccioDescans.Core.Invoices;

namespace SpaccioDescans.Web.Invoices;

public static class InvoiceExtensions
{
    public static IServiceCollection AddInvoiceService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IWorkbookFacade, WorkbookFacade>(_ =>
        {
            var invoiceSettings = configuration.GetSection(nameof(InvoiceSettings)).Get<InvoiceSettings>();
            return new WorkbookFacade(invoiceSettings);
        });

        services.AddScoped(sp =>
        {
            var workbookFacade = sp.GetRequiredService<IWorkbookFacade>();
            Dictionary<Type, IInvoice> strategies = new()
            {
                { typeof(DeliveryNote), new DeliveryNote(workbookFacade)},
                { typeof(CustomerInvoice), new CustomerInvoice(workbookFacade)}
            };

            return new InvoiceFactory(strategies);
        });

        return services;
    }
}

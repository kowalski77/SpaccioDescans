using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Web.Pages.Orders.ViewModels;

public static class OrderViewModelMapper
{
    public static OrderViewModel AsOrderViewModel(this OrderEditDto orderEditDto)
    {
        ArgumentNullException.ThrowIfNull(orderEditDto);
        
        return new OrderViewModel
        {
            Id = orderEditDto.Id,
            CustomerInfo = new CustomerInfoViewModel
            {
                Name = orderEditDto.Customer.Name,
                Address = orderEditDto.Customer.Address,
                Nif = orderEditDto.Customer.Nif,
                Phone = orderEditDto.Customer.Phone
            },
        };
    }
}

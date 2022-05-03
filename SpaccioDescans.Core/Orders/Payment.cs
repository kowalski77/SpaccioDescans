using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Orders;

public class Payment : Entity
{
    public Payment(decimal amount, PaymentMethod paymentMethod)
    {
        this.Amount = amount;
        this.PaymentMethod = paymentMethod;
    }

    public int Id { get; private set; }

    public decimal Amount { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }
}
using EGold.Resources;

namespace EGold.Services;

public interface IStripeService
{
    Task<CustomerResource> CreateCustomer(CreateCustomerResource resource, CancellationToken cancellationToken);
    Task<ChargeResource> CreateCharge(CreateChargeResource resource, CancellationToken cancellationToken);
    Task<IEnumerable<ChargeResource>> GetChargeHistory(string customerId, CancellationToken cancellationToken);
}
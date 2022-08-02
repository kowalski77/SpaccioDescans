namespace SpaccioDescans.Core.Domain.Stores;

public class StoreDto
{
    public string Owner { get; init; } = default!;

    public string Nif { get; init; } = default!;

    public string Email { get; init; } = default!;

    public string Address { get; init; } = default!;
}

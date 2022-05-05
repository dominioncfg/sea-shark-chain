namespace SeaSharkChain.Tests;

public record DummyUser
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}
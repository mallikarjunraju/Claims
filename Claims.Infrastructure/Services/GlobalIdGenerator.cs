namespace Claims.Infrastructure.Services;

public class GlobalIdGenerator : IGlobalIdGenerator
{
    public Guid GenerateId() => Guid.NewGuid();
}

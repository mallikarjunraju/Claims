namespace Claims.Infrastructure.Services;

public interface IGlobalIdGenerator
{
    Guid GenerateId();
}

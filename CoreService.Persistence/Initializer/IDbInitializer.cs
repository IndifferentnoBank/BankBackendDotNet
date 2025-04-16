namespace CoreService.Persistence.Initializer;

public interface IDbInitializer
{
    Task InitializeAsync();
}
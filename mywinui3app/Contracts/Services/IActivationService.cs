namespace mywinui3app.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}

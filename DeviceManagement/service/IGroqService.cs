namespace DeviceManagement.service
{
    public interface IGroqService
    {
        Task<string> SendPromptAsync(string prompt, CancellationToken cancellationToken = default);
    }
}

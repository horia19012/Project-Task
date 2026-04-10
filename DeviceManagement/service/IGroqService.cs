namespace DeviceManagement.service
{
    public interface IGroqService
    {
        /// <summary>
        /// Sends a request to the API.
        /// </summary>
        /// <param name="prompt">User input</param>
        /// <param name="cancellationToken">Token used to cancel the request.</param>
        /// <returns>API response</returns>
        Task<string> SendPromptAsync(string prompt, CancellationToken cancellationToken = default);
    }
}

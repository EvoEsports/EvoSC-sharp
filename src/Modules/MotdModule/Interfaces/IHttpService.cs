namespace EvoSC.Modules.Official.MotdModule.Interfaces;

public interface IHttpService
{
    /// <summary>
    /// Sends a HTTP GET request to the supplied uri
    /// </summary>
    /// <param name="uri">Uri to send the request to</param>
    /// <returns>HTTP GET answer</returns>
    public Task<string> GetAsync(string uri);
}

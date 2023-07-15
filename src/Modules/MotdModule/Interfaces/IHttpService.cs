namespace EvoSC.Modules.Official.MotdModule.Interfaces;

public interface IHttpService
{
    public Task<string> GetAsync(string uri);
}

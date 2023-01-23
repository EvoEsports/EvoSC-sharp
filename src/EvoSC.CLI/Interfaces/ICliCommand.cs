namespace EvoSC.CLI.Interfaces;

public interface ICliCommand
{
    /// <summary>
    /// Execute the command's handler.
    /// </summary>
    /// <param name="cancelToken">Cancel token to cancel the execution if set.</param>
    /// <param name="context">The command's execution context.</param>
    /// <returns></returns>
    public Task ExecuteAsync(CancellationToken cancelToken, CliCommandContext context);
}

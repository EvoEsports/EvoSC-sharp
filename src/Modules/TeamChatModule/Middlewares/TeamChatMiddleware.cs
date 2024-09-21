using EvoSC.Common.Middleware;
using EvoSC.Common.Middleware.Attributes;
using EvoSC.Common.Remote.ChatRouter;
using EvoSC.Modules.Official.TeamChatModule.Config;

namespace EvoSC.Modules.Official.TeamChatModule.Middlewares;

[Middleware(For = PipelineType.ChatRouter)]
public class TeamChatMiddleware(ActionDelegate next, ITeamChatSettings settings)
{
    public Task ExecuteAsync(ChatRouterPipelineContext context)
    {
        if (context.IsTeamMessage)
        {
            context.Recipients = context.Recipients.Where(p =>
            {
                // show message to players in the include group
                if (p.Groups.Any(g => g.Id == settings.IncludeGroup))
                {
                    return true;
                }

                // do not show message to players in exclude group
                if (p.Groups.Any(g => g.Id == settings.ExcludeGroup))
                {
                    return false;
                }

                return p.Team == context.Author.Team;
            }).ToList();
        }
        
        return next(context);
    }
}

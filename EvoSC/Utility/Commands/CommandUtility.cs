using System;
using System.Collections.Generic;
using DefaultEcs;
using EvoSC.Utility.Commands.Parameters;

namespace EvoSC.Utility.Commands
{
    public static class CommandUtility
    {
        /// <summary>
        /// Get the best commands for the current text input
        /// </summary>
        /// <param name="commands">Array of command entities</param>
        /// <param name="input">Text input</param>
        /// <param name="output">Matched commands</param>
        /// <returns>True if there was at least one match</returns>
        public static bool GetBestCommands<TList>(ReadOnlySpan<Entity> commands, string input, TList output)
            where TList : IList<Entity>
        {
            output.Clear();

            var longest = 0;
            foreach (var commandEntity in commands)
            {
                var path = commandEntity.Get<CommandPath>().Value;
                if (!input.StartsWith(path))
                    continue;

                if (path.Length > longest)
                {
                    longest = path.Length;
                    output.Clear();
                }

                if (path.Length == longest)
                    output.Add(commandEntity);
            }

            return output.Count > 0;
        }

        /// <summary>
        /// Get whether or not a command can be executed
        /// </summary>
        /// <param name="commandEntity">The command entity</param>
        /// <param name="input">Input string</param>
        /// <param name="argumentOutput">Output of argument entities</param>
        /// <typeparam name="TList">List of entity that are arguments/parameters</typeparam>
        /// <returns>True if the command can be executed</returns>
        public static bool CanExecuteCommand<TList>(Entity commandEntity, string input, TList argumentOutput)
            where TList : IList<Entity>
        {
            argumentOutput.Clear();
            
            var path = commandEntity.Get<CommandPath>().Value;
            var trimmedText = input[path.Length..].TrimStart();

            return CanExecute(commandEntity, trimmedText, argumentOutput);
        }

        // TODO: add a way to customize it (so that the users can create new parameter type)
        private static ParameterBase[] s_parameters =
        {
            new NumberParameter(), new BooleanParameter(), new TextParameter()
        };

        private static bool CanExecute<TList>(Entity commandEntity, string text, TList argumentOutput)
            where TList : IList<Entity>
        {
            var targetArguments = commandEntity.Get<CommandArguments>();

            var span = text.AsSpan();
            for (var arg = 0; arg < targetArguments.Count; arg++)
            {
                var target = targetArguments[arg];
                var ent = commandEntity.World.CreateEntity();
                ent.Set(target.Type);

                var success = false;
                foreach (var parameter in s_parameters)
                {
                    if (parameter.IsTypeValid(target.Type)
                        && parameter.TryParse(target.Type, span, ent, out span, arg + 1 == targetArguments.Count))
                    {
                        success = true;
                        break;
                    }
                }

                if (!success)
                {
                    ent.Dispose();
                    return false;
                }

                // remove trailing spaces for next argument
                span = span.TrimStart();

                argumentOutput.Add(ent);
            }

            // If either the argument count was invalid or the user has passed more arguments, return false
            if (argumentOutput.Count == targetArguments.Count && span.Trim().IsEmpty)
            {
                return true;
            }

            foreach (var arg in argumentOutput)
                arg.Dispose();
            argumentOutput.Clear();

            return false;
        }
    }
}
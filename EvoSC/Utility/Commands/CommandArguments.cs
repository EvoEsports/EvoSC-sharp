using System;
using System.Collections.Generic;

namespace EvoSC.Utility.Commands
{
    /// <summary>
    /// The arguments of a command
    /// </summary>
    public class CommandArguments : List<CommandArguments.Argument>
    {
        public struct Argument
        {
            public Type Type;
            public string Name;
            public string Description;

            public Argument(Type type, string name = "", string description = "")
            {
                Type = type;
                Name = name;
                Description = description;
            }
        }

        public void Add(Type type, string name = "", string description = "")
        {
            Add(new Argument(type, name, description));
        }
    }
}

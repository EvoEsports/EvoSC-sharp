using System;
using DefaultEcs;

namespace EvoSC.Utility.Commands.Parameters
{
    public abstract class ParameterBase
    {
        public abstract bool IsTypeValid(Type type);

        public abstract bool TryParse(Type type, ReadOnlySpan<char> input, Entity argument, out ReadOnlySpan<char> output, bool isLastArgument);
    }
}
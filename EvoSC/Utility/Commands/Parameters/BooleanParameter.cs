using System;
using System.Globalization;
using DefaultEcs;

namespace EvoSC.Utility.Commands.Parameters
{
    public class BooleanParameter : ParameterBase
    {
        public override bool IsTypeValid(Type type)
        {
            return type == typeof(bool);
        }

        public override bool TryParse(Type type, ReadOnlySpan<char> input, Entity argument, out ReadOnlySpan<char> output, bool isLastArgument)
        {
            var end = input.IndexOf(' ');
            if (end == -1)
                end = input.Length - 1;

            if (end <= 0)
            {
                output = ReadOnlySpan<char>.Empty;
                return false;
            }

            output = input[(end+1)..];

            var span = input[..(end+1)];
            if (bool.TryParse(span, out var result))
            {
                argument.Set(result);
                return true;
            }

            Span<char> alloc = stackalloc char[span.Length];
            span.ToLower(alloc, CultureInfo.InvariantCulture);
            
            if (alloc[0] == '1' || alloc.SequenceEqual("yes") || alloc.SequenceEqual("enable"))
            {
                argument.Set(true);
                return true;
            }
            
            if (alloc[0] == '0' || alloc.SequenceEqual("no") || alloc.SequenceEqual("disable"))
            {
                argument.Set(false);
                return true;
            }

            return false;
        }
    }
}
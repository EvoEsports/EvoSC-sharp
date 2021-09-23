using System;
using DefaultEcs;

namespace EvoSC.Utility.Commands.Parameters
{
    public class NumberParameter : ParameterBase
    {
        public override bool IsTypeValid(Type type)
        {
            return
                // integer
                type == typeof(int)
                || type == typeof(long)
                // floating
                || type == typeof(float)
                || type == typeof(double);
        }

        public override bool TryParse(Type type, ReadOnlySpan<char> input, Entity argument,
            out ReadOnlySpan<char> output, bool isLastArgument)
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
            if (type == typeof(int) && int.TryParse(span, out var resultInt))
            {
                argument.Set(resultInt);
                return true;
            }

            if (type == typeof(long) && long.TryParse(span, out var resultLong))
            {
                argument.Set(resultLong);
                return true;
            }

            if (type == typeof(float) && float.TryParse(span, out var resultFloat))
            {
                argument.Set(resultFloat);
                return true;
            }

            if (type == typeof(double) && double.TryParse(span, out var resultDouble))
            {
                argument.Set(resultDouble);
                return true;
            }

            return false;
        }
    }
}

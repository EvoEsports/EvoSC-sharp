using System;
using DefaultEcs;

namespace EvoSC.Utility.Commands.Parameters
{
    public class TextParameter : ParameterBase
    {
        // TODO: Add type EnclosedString to force text to be in ""

        public override bool IsTypeValid(Type type)
        {
            return type == typeof(string);
        }

        public override bool TryParse(Type type, ReadOnlySpan<char> input, Entity argument,
            out ReadOnlySpan<char> output, bool isLastArgument)
        {
            // Should we accept empty text without "" ?
            if (input.Length == 0)
            {
                output = input;
                return false;
            }

            // other arguments> "Enclosed Text" <other arguments
            if (input.Length >= 2)
            {
                var lastQuote = input[1..].IndexOf('"') + 1;
                if (input[0] == '"' && lastQuote > 0)
                {
                    argument.Set(input.Slice(1, lastQuote - 1).ToString());

                    output = input.Slice(lastQuote + 1);
                    return true;
                }
            }

            // other arguments> Free Text
            if (isLastArgument)
            {
                argument.Set(input.ToString());

                output = ReadOnlySpan<char>.Empty;
                return true;
            }

            // other arguments> Word <other arguments
            var nextSpace = input.IndexOf(' ');
            if (nextSpace > 0)
            {
                argument.Set(input.Slice(0, nextSpace).ToString());

                output = input.Slice(nextSpace);
                return true;
            }

            output = input;
            return false;
        }
    }
}
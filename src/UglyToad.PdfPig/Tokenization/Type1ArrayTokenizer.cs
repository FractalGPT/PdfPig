﻿namespace UglyToad.PdfPig.Tokenization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using IO;
    using Tokens;

    internal class Type1ArrayTokenizer : ITokenizer
    {
        public bool ReadsNextByte { get; } = false;

        public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
        {
            token = null;

            if (currentByte != '{')
            {
                return false;
            }

            var builder = new StringBuilder();

            while (inputBytes.MoveNext())
            {
                if (inputBytes.CurrentByte == '}')
                {
                    break;
                }

                builder.Append((char) inputBytes.CurrentByte);
            }

            var parts = builder.ToString().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

            var tokens = new List<IToken>();

            foreach (var part in parts)
            {
                if (char.IsNumber(part[0]) || part[0] == '-')
                {
                    if (decimal.TryParse(part, NumberStyles.AllowLeadingSign, null, out var value))
                    {
                        tokens.Add(new NumericToken(value));
                    }
                    else
                    {
                        tokens.Add(OperatorToken.Create(part));
                    }

                    continue;
                }

                if (part[0] == '/')
                {
                    tokens.Add(new NameToken(part.Substring(1)));
                    continue;
                }

                if (part[0] == '(' && part[part.Length - 1] == ')')
                {
                    tokens.Add(new StringToken(part));
                    continue;
                }

                tokens.Add(OperatorToken.Create(part));

            }

            token = new ArrayToken(tokens);

            return true;
        }
    }
}

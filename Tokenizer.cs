using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class Tokenizer
{
    private static readonly Regex tokenPatterns = new Regex(@"\s*(numba|text|reveal|=|;|\(|\)|[a-zA-Z_]\w*|\d+|""[^""]*"")|//.*$", RegexOptions.Multiline);

    public static List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        int lineNumber = 1;
        foreach (Match match in tokenPatterns.Matches(input))
        {
            string value = match.Value.Trim();
            if (value.StartsWith("//")) continue; // Ignore comments
            if (value == "numba")
                tokens.Add(new Token(TokenType.NUMBA, value, lineNumber));
            else if (value == "text")
                tokens.Add(new Token(TokenType.TEXT, value, lineNumber));
            else if (value == "reveal")
                tokens.Add(new Token(TokenType.REVEAL, value, lineNumber));
            else if (value == "=")
                tokens.Add(new Token(TokenType.ASSIGN, value, lineNumber));
            else if (value == ";")
                tokens.Add(new Token(TokenType.SEMICOLON, value, lineNumber));
            else if (value == "(")
                tokens.Add(new Token(TokenType.LPAREN, value, lineNumber));
            else if (value == ")")
                tokens.Add(new Token(TokenType.RPAREN, value, lineNumber));
            else if (Regex.IsMatch(value, @"^\d+$"))
                tokens.Add(new Token(TokenType.NUMBER, value, lineNumber));
            else if (Regex.IsMatch(value, @"^""[^""]*""$"))
                tokens.Add(new Token(TokenType.TEXT, value.Trim('"'), lineNumber));
            else if (Regex.IsMatch(value, @"^[a-zA-Z_]\w*$"))
                tokens.Add(new Token(TokenType.IDENTIFIER, value, lineNumber));
            else
                tokens.Add(new Token(TokenType.UNKNOWN, value, lineNumber));

            if (value.Contains("\n"))
                lineNumber++;
        }
        return tokens;
    }
}

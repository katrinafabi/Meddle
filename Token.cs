using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum TokenType
{
    NUMBA,      // integer
    TEXT,       // Renamed from STRING
    IDENTIFIER,
    NUMBER,
    CHAR,       // Renamed from TEXT
    REVEAL,     // printout
    ASSIGN,
    SEMICOLON,
    LPAREN,
    RPAREN,
    COMMENT,
    UNKNOWN
}

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int LineNumber { get; }

    public Token(TokenType type, string value, int lineNumber)
    {
        Type = type;
        Value = value;
        LineNumber = lineNumber;
    }

    public override string ToString()
    {
        return $"{Type}({Value})";
    }
}
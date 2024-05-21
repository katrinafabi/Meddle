using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Interpreter
{
    private Dictionary<string, object> variables = new Dictionary<string, object>();

    public void Interpret(List<Token> tokens, TextBox outputTextBox, TextBox errorTextBox)
{
    StringBuilder result = new StringBuilder();
    int lineNumber = 1; // Initialize line number counter

    try
    {
        int i = 0;
        while (i < tokens.Count)
        {
            var token = tokens[i];
            if (token.Type == TokenType.NUMBA || token.Type == TokenType.TEXT)
            {
                var type = token.Type;
                var identifier = tokens[++i];
                var assign = tokens[++i];
                var valueToken = tokens[++i];
                var semicolon = tokens[++i];

                if (identifier.Type != TokenType.IDENTIFIER || assign.Type != TokenType.ASSIGN || semicolon.Type != TokenType.SEMICOLON)
                    throw new Exception($"Syntax error at line {lineNumber}"); // Use the current line number

                if (type == TokenType.NUMBA && valueToken.Type == TokenType.NUMBER)
                    variables[identifier.Value] = int.Parse(valueToken.Value);
                else if (type == TokenType.TEXT && valueToken.Type == TokenType.TEXT)
                    variables[identifier.Value] = valueToken.Value;
                else
                    throw new Exception($"Type mismatch at line {lineNumber}"); // Use the current line number
            }
            else if (token.Type == TokenType.REVEAL)
            {
                var leftParen = tokens[++i];
                var identifier = tokens[++i];
                var rightParen = tokens[++i];
                var semicolon = tokens[++i];

                if (leftParen.Type != TokenType.LPAREN || identifier.Type != TokenType.IDENTIFIER || rightParen.Type != TokenType.RPAREN || semicolon.Type != TokenType.SEMICOLON)
                    throw new Exception($"Syntax error at line {lineNumber}"); // Use the current line number

                if (variables.ContainsKey(identifier.Value))
                    result.AppendLine(variables[identifier.Value].ToString());
                else
                    throw new Exception($"Variable {identifier.Value} not defined at line {lineNumber}"); // Use the current line number
            }
            else
            {
                throw new Exception($"Unknown statement at line {lineNumber}"); // Use the current line number
            }
            i++;
            lineNumber++; // Increment line number after processing each token
        }

        outputTextBox.Text = result.ToString();
        errorTextBox.Text = "";
    }
    catch (Exception ex)
    {
        errorTextBox.Text = $"Error: {ex.Message}";
    }
}

}

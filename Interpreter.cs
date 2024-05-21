using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Interpreter
{
    private Dictionary<string, object> variables = new Dictionary<string, object>();

    public void Interpret(List<Token> tokens, TextBox outputTextBox)
    {
        StringBuilder result = new StringBuilder();
        int lineNumber = 1;

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
                        throw new Exception($"Syntax error at line {lineNumber}");

                    if (type == TokenType.NUMBA && valueToken.Type == TokenType.NUMBER)
                        variables[identifier.Value] = int.Parse(valueToken.Value);
                    else if (type == TokenType.TEXT && valueToken.Type == TokenType.TEXT)
                        variables[identifier.Value] = valueToken.Value;
                    else
                        throw new Exception($"Type mismatch at line {lineNumber}");
                }
            }

            outputTextBox.Text = result.ToString();
        }
        catch (Exception ex)
        {
            outputTextBox.Text = $"Error: {ex.Message}";
        }
    }
}
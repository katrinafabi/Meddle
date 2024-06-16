﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Middle
{
    public partial class Main : Form
    {
        private HashSet<string> keywords = new HashSet<string> { "num", "chat", "display", "if", "else", "switch"};
        private List<string> errors = new List<string>();
        private Dictionary<string, (string type, string value)> variables = new Dictionary<string, (string type, string value)>();

        public Main()
        {
            InitializeComponent();
            runbtn.Click += runbtn_Click;
            run2btn.Click += runbtn_Click;
            debugbtn.Click += debugbtn_Click;
            debug2btn.Click += debug2btn_Click;
            stopbtn.Click += stopbtn_Click;
            stop2btn.Click += stop2btn_Click;
        }

        private void runbtn_Click(object sender, EventArgs e)
        {
            errors.Clear();
            variables.Clear();
            errorlist.Text = "";
            outputbox.Text = "";

            string code = codebox.Text;
            string[] lines = code.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> statements = ParseStatements(lines);

            foreach (string statement in statements)
            {
                string trimmedStatement = statement.Trim();
                if (!string.IsNullOrEmpty(trimmedStatement))
                {
                    ProcessStatement(trimmedStatement);
                }
            }

            DisplayErrors();
        }

        private List<string> ParseStatements(string[] lines)
        {
            List<string> statements = new List<string>();
            StringBuilder currentStatement = new StringBuilder();
            int ifElseBlockDepth = 0;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("if") || trimmedLine.StartsWith("else"))
                {
                    ifElseBlockDepth++;
                }
                if (trimmedLine == "}")
                {
                    ifElseBlockDepth--;
                }
                currentStatement.AppendLine(trimmedLine);
                if (ifElseBlockDepth == 0)
                {
                    statements.Add(currentStatement.ToString());
                    currentStatement.Clear();
                }
            }

            return statements;
        }

        private void CheckForErrors()
        {
            errors.Clear();
            variables.Clear();
            errorlist.Text = "";

            string code = codebox.Text;
            string[] lines = code.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> statements = ParseStatements(lines);

            foreach (string statement in statements)
            {
                string trimmedStatement = statement.Trim();
                if (!string.IsNullOrEmpty(trimmedStatement))
                {
                    CheckStatementForErrors(trimmedStatement);
                }
            }

            DisplayErrors();
        }

        private void ProcessStatement(string statement)
        {
            if (statement.StartsWith("chat") || statement.StartsWith("num"))
            {
                string[] tokens = statement.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length != 3 || keywords.Contains(tokens[1]) || !tokens[2].StartsWith("="))
                {
                    if (keywords.Contains(tokens[1]))
                    {
                        errors.Add("Error: '" + tokens[1] + "' is a keyword and cannot be used as a variable name in statement: " + statement);
                    }
                    else
                    {
                        errors.Add("Error: Invalid syntax in statement: " + statement);
                    }
                    return;
                }

                string variableType = tokens[0];
                string variableName = tokens[1];
                string valueExpression = tokens[2].Substring(1).Trim();

                string value = EvaluateExpression(valueExpression);

                if (variableType == "num" && !int.TryParse(value, out _))
                {
                    errors.Add($"Error: Invalid value for num type in statement: {statement}");
                }
                else if (variableType == "chat" && (!value.StartsWith("\"") || !value.EndsWith("\"")))
                {
                    errors.Add($"Error: Invalid value for chat type in statement: {statement}");
                }
                else
                {
                    variables[variableName] = (variableType, value);
                }
            }
            else if (statement.StartsWith("display"))
            {
                int startIndex = statement.IndexOf('(');
                int endIndex = statement.IndexOf(')');
                if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
                {
                    string[] variableNames = statement.Substring(startIndex + 1, endIndex - startIndex - 1).Split(',');
                    foreach (string variableName in variableNames)
                    {
                        string trimmedVariableName = variableName.Trim();
                        if (!variables.ContainsKey(trimmedVariableName))
                        {
                            errors.Add("Error: Undefined variable used in statement: " + statement);
                        }
                        else
                        {
                            DisplayOutput(variables[trimmedVariableName].value);
                        }
                    }
                }
                else
                {
                    errors.Add("Error: Invalid syntax in statement: " + statement);
                }
            }
            else if (statement.StartsWith("if"))
            {
                ProcessIfElseStatement(statement);
            }
           
            else
            {
                errors.Add("Error: Invalid syntax in statement: " + statement);
            }
        }

       

        private void ProcessDeclarationStatement(string statement)
         {
              string[] tokens = statement.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
              if (tokens.Length != 3 || keywords.Contains(tokens[0]) || !tokens[1].All(char.IsLetter) || !tokens[2].StartsWith("="))
           {
        if (keywords.Contains(tokens[0]))
        {
            errors.Add("Error: '" + tokens[0] + "' is a keyword and cannot be used as a variable type in statement: " + statement);
        }
        else if (!tokens[1].All(char.IsLetter))
        {
            errors.Add("Error: Invalid variable name in statement: " + statement);
        }
        else
        {
            errors.Add("Error: Invalid syntax in statement: " + statement);
        }
        return;
    }

    string variableType = tokens[0];
    string variableName = tokens[1];
    string valueExpression = tokens[2].Substring(1).Trim();

    string value = EvaluateExpression(valueExpression);

    if (variableType == "num" && !int.TryParse(value, out _))
    {
        errors.Add($"Error: Invalid value for num type in statement: {statement}");
    }
    else if (variableType == "chat" && (!value.StartsWith("\"") || !value.EndsWith("\"")))
    {
        errors.Add($"Error: Invalid value for chat type in statement: {statement}");
    }
    else
    {
        variables[variableName] = (variableType, value);
    }
}

        private void CheckStatementForErrors(string statement)
        {
            if (statement.StartsWith("chat") || statement.StartsWith("num"))
            {
                string[] tokens = statement.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length != 3 || keywords.Contains(tokens[1]) || !tokens[2].StartsWith("="))
                {
                    if (keywords.Contains(tokens[1]))
                    {
                        errors.Add("Error: '" + tokens[1] + "' is a keyword and cannot be used as a variable name in statement: " + statement);
                    }
                    else
                    {
                        errors.Add("Error: Invalid syntax in statement: " + statement);
                    }
                }
                else
                {
                    string variableType = tokens[0];
                    string variableName = tokens[1];
                    string valueExpression = tokens[2].Substring(1).Trim();

                    string value = EvaluateExpression(valueExpression);

                    if (variableType == "num" && !int.TryParse(value, out _))
                    {
                        errors.Add($"Error: Invalid value for num type in statement: {statement}");
                    }
                    else if (variableType == "chat" && (!value.StartsWith("\"") || !value.EndsWith("\"")))
                    {
                        errors.Add($"Error: Invalid value for chat type in statement: {statement}");
                    }
                    else
                    {
                        variables[variableName] = (variableType, value);
                    }
                }
            }
            else if (statement.StartsWith("display"))
            {
                int startIndex = statement.IndexOf('(');
                int endIndex = statement.IndexOf(')');
                if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
                {
                    string[] variableNames = statement.Substring(startIndex + 1, endIndex - startIndex - 1).Split(',');
                    foreach (string variableName in variableNames)
                    {
                        string trimmedVariableName = variableName.Trim();
                        if (!variables.ContainsKey(trimmedVariableName))
                        {
                            errors.Add("Error: Undefined variable used in statement: " + statement);
                        }
                    }
                }
                else
                {
                    errors.Add("Error: Invalid syntax in statement: " + statement);
                }
            }
            else if (statement.StartsWith("if"))
            {
                CheckIfElseStatementForErrors(statement);
            }
            else
            {
                errors.Add("Error: Invalid syntax in statement: " + statement);
            }
        }

        private void ProcessIfElseStatement(string statement)
        {
            int ifIndex = statement.IndexOf("if");
            int elseIndex = statement.IndexOf("else");
            string condition = statement.Substring(ifIndex + 2, statement.IndexOf('{') - ifIndex - 2).Trim();
            string ifBlock = statement.Substring(statement.IndexOf('{') + 1, elseIndex - statement.IndexOf('{') - 1).Trim();
            string elseBlock = statement.Substring(statement.IndexOf('{', elseIndex) + 1, statement.LastIndexOf('}') - statement.IndexOf('{', elseIndex) - 1).Trim();

            bool conditionResult = EvaluateCondition(condition);
            List<string> statementsToProcess = conditionResult ? ParseStatements(ifBlock.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)) : ParseStatements(elseBlock.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));

            foreach (string stmt in statementsToProcess)
            {
                ProcessStatement(stmt.Trim());
            }
        }

        private void CheckIfElseStatementForErrors(string statement)
        {
            int ifIndex = statement.IndexOf("if");
            int elseIndex = statement.IndexOf("else");
            string condition = statement.Substring(ifIndex + 2, statement.IndexOf('{') - ifIndex - 2).Trim();
            string ifBlock = statement.Substring(statement.IndexOf('{') + 1, elseIndex - statement.IndexOf('{') - 1).Trim();
            string elseBlock = statement.Substring(statement.IndexOf('{', elseIndex) + 1, statement.LastIndexOf('}') - statement.IndexOf('{', elseIndex) - 1).Trim();

            if (!EvaluateCondition(condition))
            {
                errors.Add($"Error: Invalid condition '{condition}'");
            }

            List<string> statementsToCheck = ParseStatements(ifBlock.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)).Concat(ParseStatements(elseBlock.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))).ToList();

            foreach (string stmt in statementsToCheck)
            {
                CheckStatementForErrors(stmt.Trim());
            }
        }

        private bool EvaluateCondition(string condition)
        {
            condition = ReplaceVariablesInExpression(condition);
            try
            {
                var table = new DataTable();
                table.Columns.Add("condition", typeof(bool), condition);
                DataRow row = table.NewRow();
                table.Rows.Add(row);
                return (bool)row["condition"];
            }
            catch
            {
                errors.Add($"Error: Invalid condition '{condition}'");
                return false;
            }
        }

        private string EvaluateExpression(string expression)
        {
            if (expression.StartsWith("\"") && expression.EndsWith("\""))
            {
                return expression;
            }
            expression = ReplaceVariablesInExpression(expression);
            try
            {
                var table = new DataTable();
                table.Columns.Add("expression", typeof(string), expression);
                DataRow row = table.NewRow();
                table.Rows.Add(row);
                return ((string)row["expression"]).ToString();
            }
            catch
            {
                errors.Add($"Error: Invalid expression '{expression}'");
                return "0";
            }
        }

        private string ReplaceVariablesInExpression(string expression)
        {
            foreach (var variable in variables)
            {
                if (variable.Value.type == "num")
                {
                    expression = expression.Replace(variable.Key, variable.Value.value);
                }
            }
            return expression;
        }

        private void DisplayErrors()
        {
            errorlist.Text = string.Join(Environment.NewLine, errors);
        }

        private void DisplayOutput(string value)
        {
            if (value.StartsWith("\"") && value.EndsWith("\"") && value.Length > 2)
            {
                string word = value.Substring(1, value.Length - 2);
                outputbox.AppendText(word + Environment.NewLine);
            }
            else
            {
                outputbox.AppendText(value + Environment.NewLine);
            }
        }

        private void debugbtn_Click(object sender, EventArgs e)
        {
            CheckForErrors();
        }

        private void debug2btn_Click(object sender, EventArgs e)
        {
            CheckForErrors();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            outputbox.Text = string.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartPage startpageForm = new StartPage();
            startpageForm.ShowDialog();
            this.Hide();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            UpdateLineNumbers();
        }

        private void UpdateLineNumbers()
        {
            textBox1.Clear();
            int linesCount = codebox.Lines.Length;
            for (int i = 1; i <= linesCount; i++)
            {
                textBox1.AppendText(i.ToString() + Environment.NewLine);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                saveFileDialog.Title = "Save File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    string code = codebox.Text;

                    try
                    {
                        System.IO.File.WriteAllText(filePath, code);
                        MessageBox.Show("File saved successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}");
                    }
                }
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            codebox.ScrollToCaret();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            StartPage startpageForm = new StartPage();
            startpageForm.ShowDialog();
            this.Hide();
        }

        private void run2btn_Click(object sender, EventArgs e)
        {

        }

        private void ClearErrors()
        {
            errorlist.Text = "";
        }

        private void stopbtn_Click(object sender, EventArgs e)
        {
            ClearErrors();
        }

        private void stop2btn_Click(object sender, EventArgs e)
        {
            ClearErrors();
        }
    }
}

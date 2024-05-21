using System;
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
        public Main()
        {
            InitializeComponent();
            
            richTextBox2.TextChanged += richTextBox2_TextChanged;
            richTextBox2.Text = @"// Welcome to Meddle!
                // numba  = int
                // text = string
                // if you want to print out use reveal, for example

                text x = ""If in doubt..Sir, may i go out?"";
                   reveal(x);";
            UpdateLineNumbers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string code = richTextBox2.Text;

            try
            {
                var tokens = Tokenizer.Tokenize(code);
                var interpreter = new Interpreter();
                interpreter.Interpret(tokens, textBox2, textBox3);
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartPage startpageForm = new StartPage();
            startpageForm.ShowDialog();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string code = richTextBox2.Text;

            try
            {
                var tokens = Tokenizer.Tokenize(code);
                var interpreter = new Interpreter();
                interpreter.Interpret(tokens, textBox2, textBox3);
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message;
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            UpdateLineNumbers();
        }

        private void UpdateLineNumbers()
        {
            textBox1.Clear();
            int linesCount = richTextBox2.Lines.Length;
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
                    string code = richTextBox2.Text;

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
            richTextBox2.ScrollToCaret();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            isDebugging = true;
            string code = richTextBox2.Text;
            try
            {
                var tokens = Tokenizer.Tokenize(code);
                List<string> errorMessages = new List<string>();
                foreach (var token in tokens)
                {
                    Console.WriteLine($"Token: {token.Value}, Type: {token.Type}, LineNumber: {token.LineNumber}");

                    if (token.Type == TokenType.UNKNOWN)
                    {
                        errorMessages.Add($"Syntax error: Unknown statement at line {token.LineNumber}");
                    }
                }
                if (errorMessages.Count > 0)
                {
                    foreach (string errorMessage in errorMessages)
                    {
                        textBox3.AppendText(errorMessage + Environment.NewLine);
                    }
                    MessageBox.Show("Syntax errors found. Code execution halted for debugging.");
                }
                else
                {
                    var interpreter = new Interpreter();
                    interpreter.Interpret(tokens, textBox2, textBox3);
                }
            }
            catch (Exception ex)
            {
                textBox3.Text = $"Error: {ex.Message}";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string code = richTextBox2.Text;
            try
            {
                var tokens = Tokenizer.Tokenize(code);
                List<string> errorMessages = new List<string>();
                foreach (var token in tokens)
                {
                    Console.WriteLine($"Token: {token.Value}, Type: {token.Type}, LineNumber: {token.LineNumber}");

                    if (token.Type == TokenType.UNKNOWN)
                    {
                        errorMessages.Add($"Syntax error: Unknown statement at line {token.LineNumber}");
                    }
                }
                if (errorMessages.Count > 0)
                {
                    foreach (string errorMessage in errorMessages)
                    {
                        textBox3.AppendText(errorMessage + Environment.NewLine);
                    }
                    MessageBox.Show("Syntax errors found. Code execution halted for debugging.");
                }
                else
                {
                    var interpreter = new Interpreter();
                    interpreter.Interpret(tokens, textBox2, textBox3);
                }
            }
            catch (Exception ex)
            {
                textBox3.Text = $"Error: {ex.Message}";
            }
        }

        private bool isDebugging = false;

        private void button8_Click(object sender, EventArgs e)
        {
            if (isDebugging)
            {
                isDebugging = false;
                textBox3.Clear();
            }
            string code = richTextBox2.Text;
            try
            {
                var tokens = Tokenizer.Tokenize(code);
                var interpreter = new Interpreter();
                interpreter.Interpret(tokens, textBox2, textBox3);
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (isDebugging)
            {
                isDebugging = false;
                textBox3.Clear();
            }
            string code = richTextBox2.Text;
            try
            {
                var tokens = Tokenizer.Tokenize(code);
                var interpreter = new Interpreter();
                interpreter.Interpret(tokens, textBox2, textBox3);
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message;
            }
        }



        private void button9_Click(object sender, EventArgs e)
        {
            StartPage startpageForm = new StartPage();
            startpageForm.ShowDialog();
            this.Hide();
        }
    }
}

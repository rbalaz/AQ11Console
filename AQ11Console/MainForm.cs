using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace AQ11Console
{
    public partial class MainForm : Form
    {
        List<Example> examples;

        public MainForm()
        {
            InitializeComponent();
        }

        private void dataLoader_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataLoader loader = new DataLoader(openFileDialog1.FileName);
                examples = loader.loadData();
                loadExamplesIntoDataGrid();
            }
        }

        private void loadExamplesIntoDataGrid()
        {
            Example prototype = examples[0];
            List<Attribute> attributes = prototype.attributes;
            DataTable dataTable = new DataTable();
            foreach (Attribute attribute in attributes)
            {
                dataTable.Columns.Add(new DataColumn(attribute.name, typeof(string)));
            }
            dataTable.Columns.Add(new DataColumn("Class", typeof(string)));

            foreach (Example example in examples)
            {
                List<string> attributeValues = new List<string>();
                foreach (Attribute attribute in example.attributes)
                {
                    attributeValues.Add(attribute.value);
                }
                attributeValues.Add(example.groupClass);

                dataTable.Rows.Add(attributeValues.ToArray());
            }

            dataView.DataSource = dataTable;
        }

        private void learnButton_Click(object sender, EventArgs e)
        {
            string groupClass = classBox.Text;
            if (groupClass == "")
            {
                MessageBox.Show("Class for learning was not set.", "Class error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Inference inference = new Inference(examples, groupClass);
                Rule rule = inference.ruleInference();
                string[] rules = rule.printRules();
                if (rules.Length == 1)
                    ruleLabel.Text = "Rule: ";
                else
                    ruleLabel.Text = "Rules: ";
                ruleText.Text = "";
                for (int i = 0; i < rules.Length; i++)
                {
                    if (i < rules.Length - 1)
                        ruleText.Text += rules[i] + "\n";
                    else
                        ruleText.Text += rules[i];
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            examples = null;
            dataView.DataSource = null;
            dataView.Invalidate();
            classBox.Text = "";
            ruleLabel.Text = "Rule: ";
            ruleText.Text = "";
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }
    }
}

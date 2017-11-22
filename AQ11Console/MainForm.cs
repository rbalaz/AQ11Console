using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            string path = Environment.CurrentDirectory;
            openFileDialog1.Filter = "Text|*.txt|CSV file|*.csv|All|*.*";
            openFileDialog1.InitialDirectory = Path.Combine(path, @"Data");
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
            if (examples == null)
            {
                MessageBox.Show("No data available.", "Data error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (groupClass == "")
            {
                MessageBox.Show("Class for learning was not set.", "Class error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Inference inference = new Inference(examples, groupClass);
                Rule rule = inference.ruleInference();
                string[] rules = rule.printRules();
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(new DataColumn("Rule", typeof(string)));
                for (int i = 0; i < rules.Length; i++)
                {
                    dataTable.Rows.Add(rules[i]);
                }
                ruleView.DataSource = dataTable;
                ruleView.Columns[0].Width = 900;
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            examples = null;
            dataView.DataSource = null;
            dataView.Invalidate();
            classBox.Text = "";
            ruleView.DataSource = null;
            ruleView.Invalidate();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }
    }
}

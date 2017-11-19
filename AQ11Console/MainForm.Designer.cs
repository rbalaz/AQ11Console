namespace AQ11Console
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataView = new System.Windows.Forms.DataGridView();
            this.dataLoader = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.learnButton = new System.Windows.Forms.Button();
            this.ruleLabel = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.classBox = new System.Windows.Forms.TextBox();
            this.classLabel = new System.Windows.Forms.Label();
            this.ruleText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataView
            // 
            this.dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataView.Location = new System.Drawing.Point(12, 21);
            this.dataView.Name = "dataView";
            this.dataView.Size = new System.Drawing.Size(420, 378);
            this.dataView.TabIndex = 0;
            // 
            // dataLoader
            // 
            this.dataLoader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dataLoader.Location = new System.Drawing.Point(3, 3);
            this.dataLoader.Name = "dataLoader";
            this.dataLoader.Size = new System.Drawing.Size(140, 43);
            this.dataLoader.TabIndex = 1;
            this.dataLoader.Text = "Load data";
            this.dataLoader.UseVisualStyleBackColor = true;
            this.dataLoader.Click += new System.EventHandler(this.dataLoader_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.dataLoader);
            this.flowLayoutPanel1.Controls.Add(this.learnButton);
            this.flowLayoutPanel1.Controls.Add(this.resetButton);
            this.flowLayoutPanel1.Controls.Add(this.quitButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(471, 21);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(145, 197);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // learnButton
            // 
            this.learnButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.learnButton.Location = new System.Drawing.Point(3, 52);
            this.learnButton.Name = "learnButton";
            this.learnButton.Size = new System.Drawing.Size(140, 43);
            this.learnButton.TabIndex = 2;
            this.learnButton.Text = "Learn";
            this.learnButton.UseVisualStyleBackColor = true;
            this.learnButton.Click += new System.EventHandler(this.learnButton_Click);
            // 
            // ruleLabel
            // 
            this.ruleLabel.AutoSize = true;
            this.ruleLabel.Font = new System.Drawing.Font("Courier New", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ruleLabel.Location = new System.Drawing.Point(9, 414);
            this.ruleLabel.Name = "ruleLabel";
            this.ruleLabel.Size = new System.Drawing.Size(62, 18);
            this.ruleLabel.TabIndex = 3;
            this.ruleLabel.Text = "Rule: ";
            // 
            // resetButton
            // 
            this.resetButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.resetButton.Location = new System.Drawing.Point(3, 101);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(140, 43);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.quitButton.Location = new System.Drawing.Point(3, 150);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(140, 43);
            this.quitButton.TabIndex = 4;
            this.quitButton.Text = "Quit";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // classBox
            // 
            this.classBox.Location = new System.Drawing.Point(549, 234);
            this.classBox.Name = "classBox";
            this.classBox.Size = new System.Drawing.Size(54, 20);
            this.classBox.TabIndex = 4;
            // 
            // classLabel
            // 
            this.classLabel.AutoSize = true;
            this.classLabel.Font = new System.Drawing.Font("Courier New", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.classLabel.Location = new System.Drawing.Point(485, 234);
            this.classLabel.Name = "classLabel";
            this.classLabel.Size = new System.Drawing.Size(62, 18);
            this.classLabel.TabIndex = 5;
            this.classLabel.Text = "Class:";
            // 
            // ruleText
            // 
            this.ruleText.AutoSize = true;
            this.ruleText.Font = new System.Drawing.Font("Courier New", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ruleText.Location = new System.Drawing.Point(64, 414);
            this.ruleText.Name = "ruleText";
            this.ruleText.Size = new System.Drawing.Size(0, 18);
            this.ruleText.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 486);
            this.Controls.Add(this.ruleText);
            this.Controls.Add(this.classLabel);
            this.Controls.Add(this.classBox);
            this.Controls.Add(this.ruleLabel);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "AQ11 demonstration application";
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataView;
        private System.Windows.Forms.Button dataLoader;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button learnButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.Label ruleLabel;
        private System.Windows.Forms.TextBox classBox;
        private System.Windows.Forms.Label classLabel;
        private System.Windows.Forms.Label ruleText;
    }
}
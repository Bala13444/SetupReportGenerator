namespace SetupReportGenerator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblXml = new Label();
            txtXmlPath = new TextBox();
            btnBrowse = new Button();
            dgvRecipes = new DataGridView();
            btnSave = new Button();
            btnReport = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvRecipes).BeginInit();
            SuspendLayout();
            // 
            // lblXml
            // 
            lblXml.AutoSize = true;
            lblXml.Location = new Point(709, 264);
            lblXml.Name = "lblXml";
            lblXml.Size = new Size(52, 15);
            lblXml.TabIndex = 0;
            lblXml.Text = "XML File";
            // 
            // txtXmlPath
            // 
            txtXmlPath.Enabled = false;
            txtXmlPath.Location = new Point(768, 259);
            txtXmlPath.Name = "txtXmlPath";
            txtXmlPath.Size = new Size(199, 23);
            txtXmlPath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(376, 247);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(86, 35);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "Browse XML";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // dgvRecipes
            // 
            dgvRecipes.BackgroundColor = SystemColors.ButtonHighlight;
            dgvRecipes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRecipes.Location = new Point(376, 288);
            dgvRecipes.Name = "dgvRecipes";
            dgvRecipes.Size = new Size(591, 150);
            dgvRecipes.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(376, 444);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(103, 32);
            btnSave.TabIndex = 8;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnReport
            // 
            btnReport.Location = new Point(883, 444);
            btnReport.Name = "btnReport";
            btnReport.Size = new Size(84, 32);
            btnReport.TabIndex = 9;
            btnReport.Text = "View Report";
            btnReport.UseVisualStyleBackColor = true;
            btnReport.Click += btnReport_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 224, 196);
            ClientSize = new Size(1412, 620);
            Controls.Add(btnReport);
            Controls.Add(btnSave);
            Controls.Add(dgvRecipes);
            Controls.Add(btnBrowse);
            Controls.Add(txtXmlPath);
            Controls.Add(lblXml);
            Name = "Form1";
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dgvRecipes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblXml;
        private TextBox txtXmlPath;
        private Button btnBrowse;
        private DataGridView dgvRecipes;
        private Button btnExcel;
        private Button btnPdf;
        private Button btnExit;
        private Button btnSave;
        private Button btnReport;
    }
}

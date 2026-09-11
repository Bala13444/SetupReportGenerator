namespace SetupReportGenerator
{
    partial class SetupReportViewerForm
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
            pnlButtons = new Panel();
            panel1 = new Panel();
            grpreport = new GroupBox();
            dtpEndate = new DateTimePicker();
            dtpStartDate = new DateTimePicker();
            lblstartdate = new Label();
            btnCancle = new Button();
            btnViewReport = new Button();
            lblConfirmPassword = new Label();
            lblPassword = new Label();
            cmbReciptNo = new ComboBox();
            panel1.SuspendLayout();
            grpreport.SuspendLayout();
            SuspendLayout();
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.White;
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(0, 514);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(800, 10);
            pnlButtons.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(grpreport);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 512);
            panel1.TabIndex = 2;
            // 
            // grpreport
            // 
            grpreport.BackColor = Color.CornflowerBlue;
            grpreport.Controls.Add(dtpEndate);
            grpreport.Controls.Add(dtpStartDate);
            grpreport.Controls.Add(lblstartdate);
            grpreport.Controls.Add(btnCancle);
            grpreport.Controls.Add(btnViewReport);
            grpreport.Controls.Add(lblConfirmPassword);
            grpreport.Controls.Add(lblPassword);
            grpreport.Controls.Add(cmbReciptNo);
            grpreport.Location = new Point(217, 114);
            grpreport.Name = "grpreport";
            grpreport.Size = new Size(409, 250);
            grpreport.TabIndex = 9;
            grpreport.TabStop = false;
            // 
            // dtpEndate
            // 
            dtpEndate.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dtpEndate.Format = DateTimePickerFormat.Custom;
            dtpEndate.Location = new Point(178, 73);
            dtpEndate.Name = "dtpEndate";
            dtpEndate.Size = new Size(200, 25);
            dtpEndate.TabIndex = 1;
            dtpEndate.ValueChanged += dtpEndate_ValueChanged;
            // 
            // dtpStartDate
            // 
            dtpStartDate.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dtpStartDate.Format = DateTimePickerFormat.Custom;
            dtpStartDate.Location = new Point(178, 31);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(200, 25);
            dtpStartDate.TabIndex = 0;
            dtpStartDate.ValueChanged += dtpStartDate_ValueChanged;
            // 
            // lblstartdate
            // 
            lblstartdate.AutoSize = true;
            lblstartdate.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblstartdate.Location = new Point(79, 37);
            lblstartdate.Name = "lblstartdate";
            lblstartdate.Size = new Size(74, 17);
            lblstartdate.TabIndex = 6;
            lblstartdate.Text = "Start Date:";
            // 
            // btnCancle
            // 
            btnCancle.BackColor = Color.Teal;
            btnCancle.FlatStyle = FlatStyle.Popup;
            btnCancle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancle.ForeColor = Color.White;
            btnCancle.Location = new Point(310, 199);
            btnCancle.Name = "btnCancle";
            btnCancle.Size = new Size(68, 28);
            btnCancle.TabIndex = 4;
            btnCancle.Text = "Cancel";
            btnCancle.UseVisualStyleBackColor = false;
            btnCancle.Click += btnCancle_Click;
            // 
            // btnViewReport
            // 
            btnViewReport.BackColor = Color.Teal;
            btnViewReport.FlatStyle = FlatStyle.Popup;
            btnViewReport.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnViewReport.ForeColor = Color.White;
            btnViewReport.Location = new Point(178, 199);
            btnViewReport.Name = "btnViewReport";
            btnViewReport.Size = new Size(79, 28);
            btnViewReport.TabIndex = 3;
            btnViewReport.Text = "View Report";
            btnViewReport.UseVisualStyleBackColor = false;
            btnViewReport.Click += btnViewReport_Click;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblConfirmPassword.Location = new Point(74, 118);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(79, 17);
            lblConfirmPassword.TabIndex = 8;
            lblConfirmPassword.Text = "Receipt No:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblPassword.Location = new Point(85, 79);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(68, 17);
            lblPassword.TabIndex = 7;
            lblPassword.Text = "End Date:";
            // 
            // cmbReciptNo
            // 
            cmbReciptNo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReciptNo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbReciptNo.FormattingEnabled = true;
            cmbReciptNo.Location = new Point(178, 118);
            cmbReciptNo.Name = "cmbReciptNo";
            cmbReciptNo.Size = new Size(200, 25);
            cmbReciptNo.TabIndex = 2;
            // 
            // SetupReportViewerForm
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = Color.White;
            ClientSize = new Size(800, 524);
            Controls.Add(panel1);
            Controls.Add(pnlButtons);
            Name = "SetupReportViewerForm";
            Text = "SetupReportViewerForm";
            WindowState = FormWindowState.Maximized;
            Load += SetupReportViewerForm_Load;
            panel1.ResumeLayout(false);
            grpreport.ResumeLayout(false);
            grpreport.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlButtons;
        private Button btnPdf;
        private Button btnExcel;
        private Panel panel1;
        private ComboBox cmbReciptNo;
        private Button btnViewReport;
        private Button btnCancle;
        private GroupBox grpreport;
        private Label lblstartdate;
        private Label lblConfirmPassword;
        private Label lblPassword;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndate;
    }
}
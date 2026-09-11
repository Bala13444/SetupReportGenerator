namespace SetupReportGenerator
{
    partial class ReportVeiwer
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
            webViewReport = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)webViewReport).BeginInit();
            SuspendLayout();
            // 
            // webViewReport
            // 
            webViewReport.AllowExternalDrop = true;
            webViewReport.CreationProperties = null;
            webViewReport.DefaultBackgroundColor = Color.White;
            webViewReport.Dock = DockStyle.Fill;
            webViewReport.Location = new Point(0, 0);
            webViewReport.Name = "webViewReport";
            webViewReport.Size = new Size(1023, 533);
            webViewReport.TabIndex = 3;
            webViewReport.ZoomFactor = 1D;
            webViewReport.Click += webViewReport_Click;
            // 
            // ReportVeiwer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1023, 533);
            Controls.Add(webViewReport);
            Name = "ReportVeiwer";
            Text = "ReportVeiwer";
            ((System.ComponentModel.ISupportInitialize)webViewReport).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public Microsoft.Web.WebView2.WinForms.WebView2 webViewReport;
       
    }
}
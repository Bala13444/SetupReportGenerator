using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SetupReportGenerator
{
    public partial class ReportVeiwer : Form
    {
        // Class-level field holding the report HTML.
        private string _html;

        public ReportVeiwer()
        {
            InitializeComponent();

            // Make sure the WebView2 control always fills the form,
            // regardless of what's set in the designer.
            webViewReport.Dock = DockStyle.Fill;

            // Open big by default instead of the small designer size.
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Shown += ReportVeiwer_Shown;
        }

        // Just stores the HTML — the actual navigation happens once,
        // in Shown, after WebView2 is guaranteed ready and the form
        // is on screen.
        public void LoadReport(string html)
        {
            _html = html;
        }

        private async void ReportVeiwer_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_html))
                return; // nothing to show

            await webViewReport.EnsureCoreWebView2Async();

            // Use a unique file name each time so WebView2 never serves
            // a cached copy of a previous report.
            string filePath = Path.Combine(Path.GetTempPath(), $"SetupReport_{Guid.NewGuid():N}.html");
            File.WriteAllText(filePath, _html, Encoding.UTF8);

            webViewReport.Source = new Uri(filePath);
        }

        private void webViewReport_Click(object sender, EventArgs e)
        {

        }
    }
}
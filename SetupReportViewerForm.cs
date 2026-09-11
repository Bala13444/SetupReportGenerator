using SetupReportGenerator.ViewModels;
using System;
using System.Windows.Forms;

namespace SetupReportGenerator
{
    public partial class SetupReportViewerForm : Form
    {
        private readonly SetupReportViewerViewModel viewModel;

        public SetupReportViewerForm()
        {
            InitializeComponent();

            viewModel = new SetupReportViewerViewModel();
            BindControls();
            WireViewModelEvents();
        }

        private void BindControls()
        {
            dtpStartDate.DataBindings.Add("Value", viewModel, nameof(viewModel.StartDate),
                true, DataSourceUpdateMode.OnPropertyChanged);

            dtpEndate.DataBindings.Add("Value", viewModel, nameof(viewModel.EndDate),
                true, DataSourceUpdateMode.OnPropertyChanged);

            cmbReciptNo.DataSource = viewModel.Receipts;
            cmbReciptNo.DisplayMember = "RecipeName";
            cmbReciptNo.ValueMember = "RecipeId";
            cmbReciptNo.DataBindings.Add("SelectedValue", viewModel, nameof(viewModel.SelectedRecipeId),
                true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void WireViewModelEvents()
        {
            viewModel.ReportGenerated += html =>
            {
                ReportVeiwer frm = new ReportVeiwer();
                frm.LoadReport(html);
                frm.ShowDialog();
            };

            viewModel.CloseRequested += () => this.Close();
        }

        // ---- Designer-wired event handlers: one-line delegations to the ViewModel ----

        private void SetupReportViewerForm_Load(object sender, EventArgs e)
        {
            dtpStartDate.Format = DateTimePickerFormat.Custom;
            dtpStartDate.CustomFormat = "dd/MM/yyyy";

            dtpEndate.Format = DateTimePickerFormat.Custom;
            dtpEndate.CustomFormat = "dd/MM/yyyy";
        }

        private void btnViewReport_Click(object sender, EventArgs e)
            => viewModel.ViewReportCommand.Execute();

        private void btnCancle_Click(object sender, EventArgs e)
            => viewModel.CancelCommand.Execute();

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            // Handled by the two-way DataBindings above, which update
            // viewModel.StartDate and trigger LoadReceiptNos() itself.
        }

        private void dtpEndate_ValueChanged(object sender, EventArgs e)
        {
            // Handled by the two-way DataBindings above.
        }

        private void btnExcel_Click(object sender, EventArgs e)
            => viewModel.ExportExcelCommand.Execute();

        private void btnPdf_Click(object sender, EventArgs e)
            => viewModel.ExportPdfCommand.Execute();
    }
}

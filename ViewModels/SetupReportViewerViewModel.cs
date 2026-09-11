using System;
using System.ComponentModel;
using SetupReportGenerator.Commands;
using SetupReportGenerator.DAL;
using SetupReportGenerator.Models;
using SetupReportGenerator.Services;

namespace SetupReportGenerator.ViewModels
{
    public class SetupReportViewerViewModel : ViewModelBase
    {
        private readonly RecipeDAL recipeDal = new RecipeDAL();
        private readonly ReportHtmlService reportHtmlService = new ReportHtmlService();

        private DateTime startDate = DateTime.Today;
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (SetProperty(ref startDate, value))
                    LoadReceiptNos();
            }
        }

        private DateTime endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                if (SetProperty(ref endDate, value))
                    LoadReceiptNos();
            }
        }

        public BindingList<RecipeHeader> Receipts { get; } = new BindingList<RecipeHeader>();

        private int selectedRecipeId;
        public int SelectedRecipeId
        {
            get => selectedRecipeId;
            set => SetProperty(ref selectedRecipeId, value);
        }

        public RelayCommand ViewReportCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand ExportExcelCommand { get; }
        public RelayCommand ExportPdfCommand { get; }

        public event Action<string> ReportGenerated;
        public event Action CloseRequested;

        public SetupReportViewerViewModel()
        {
            ViewReportCommand = new RelayCommand(ViewReport);
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke());

            // Excel/PDF export were disabled/commented out in the original
            // form too - kept as no-op hooks so the View has somewhere to
            // wire them once that feature is finished.
            ExportExcelCommand = new RelayCommand(() => { });
            ExportPdfCommand = new RelayCommand(() => { });

            LoadReceiptNos();
        }

        public void LoadReceiptNos()
        {
            var receipts = recipeDal.GetReceiptNos(StartDate.Date, EndDate.Date);

            Receipts.Clear();
            foreach (var receipt in receipts)
            {
                Receipts.Add(receipt);
            }

            if (Receipts.Count > 0)
                SelectedRecipeId = Receipts[0].RecipeId;
        }

        private void ViewReport()
        {
            string html = reportHtmlService.GenerateReport(StartDate.Date, EndDate.Date, SelectedRecipeId);
            ReportGenerated?.Invoke(html);
        }
    }
}

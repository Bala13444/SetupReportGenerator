using System;
using System.ComponentModel;
using SetupReportGenerator.Commands;
using SetupReportGenerator.DAL;
using SetupReportGenerator.Models;
using SetupReportGenerator.Services;

namespace SetupReportGenerator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly XmlService xmlService = new XmlService();
        private readonly RecipeDAL recipeDal = new RecipeDAL();
        private readonly SetupDAL setupDal = new SetupDAL();

        private RecipeHeader currentRecipe;
        public RecipeHeader CurrentRecipe
        {
            get => currentRecipe;
            private set => SetProperty(ref currentRecipe, value);
        }

        private string xmlFilePath;
        public string XmlFilePath
        {
            get => xmlFilePath;
            private set => SetProperty(ref xmlFilePath, value);
        }

        /// <summary>
        /// BindingList (not a plain List) so the DataGridView bound to it
        /// in the View reacts automatically to Clear()/Add() without the
        /// View needing to reset DataSource itself.
        /// </summary>
        public BindingList<SetupDetail> SetupDetails { get; } = new BindingList<SetupDetail>();

        public RelayCommand BrowseCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand ViewReportCommand { get; }

        // View-facing events
        public event Action<string> RequestOpenXmlDialog; // View shows OpenFileDialog, then calls LoadXmlFile
        public event Action<string> ShowInfoMessage;
        public event Action DataSaved;
        public event Action RequestShowReportViewer;

        public MainViewModel()
        {
            BrowseCommand = new RelayCommand(() => RequestOpenXmlDialog?.Invoke(XmlFilePath));
            SaveCommand = new RelayCommand(Save);
            ViewReportCommand = new RelayCommand(() => RequestShowReportViewer?.Invoke());
        }

        /// <summary>
        /// Called by the View once the user has picked a file via the
        /// OpenFileDialog it owns. All parsing/business logic stays here.
        /// </summary>
        public void LoadXmlFile(string filePath)
        {
            XmlFilePath = filePath;

            CurrentRecipe = xmlService.ReadRecipeHeader(filePath);

            SetupDetails.Clear();
            foreach (var detail in xmlService.ReadSetupDetails(filePath))
            {
                SetupDetails.Add(detail);
            }
        }

        private void Save()
        {
            if (CurrentRecipe == null || SetupDetails.Count == 0)
            {
                ShowInfoMessage?.Invoke("Please browse and load an XML file first.");
                return;
            }

            int recipeId = recipeDal.InsertRecipe(CurrentRecipe);

            foreach (SetupDetail detail in SetupDetails)
            {
                detail.RecipeId = recipeId;
                setupDal.InsertSetupDetail(detail);
            }

            ShowInfoMessage?.Invoke("Data Saved Successfully.");

            XmlFilePath = null;
            CurrentRecipe = null;
            SetupDetails.Clear();

            DataSaved?.Invoke();
        }
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SetupReportGenerator.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels. Implements INotifyPropertyChanged so
    /// WinForms controls can data-bind straight to ViewModel properties
    /// (e.g. txtUsername.DataBindings.Add("Text", viewModel, "Username"))
    /// and pick up changes automatically, in both directions.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

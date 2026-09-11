using System;

namespace SetupReportGenerator.Commands
{
    /// <summary>
    /// A minimal, reusable command implementation for WinForms MVVM.
    /// A ViewModel exposes a RelayCommand instead of a method; a Form
    /// wires a button's Click event to Command.Execute() once, in one
    /// line, instead of containing the logic itself.
    /// </summary>
    public class RelayCommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : this(_ => execute(), canExecute == null ? (Func<object, bool>)null : _ => canExecute())
        {
        }

        public bool CanExecute(object parameter = null) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter = null) => execute(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

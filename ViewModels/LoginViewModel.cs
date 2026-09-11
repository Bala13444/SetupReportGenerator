using System;
using SetupReportGenerator.Commands;
using SetupReportGenerator.DAL;

namespace SetupReportGenerator.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly UserDAL userDal = new UserDAL();

        private string username;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public RelayCommand LoginCommand { get; }
        public RelayCommand SignUpCommand { get; }
        public RelayCommand ExitCommand { get; }

        // The View subscribes to these instead of the ViewModel touching
        // MessageBox / Form navigation directly.
        public event Action<string> ValidationFailed;
        public event Action LoginSucceeded;
        public event Action LoginFailed;
        public event Action NavigateToSignUp;
        public event Action ExitRequested;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            SignUpCommand = new RelayCommand(() => NavigateToSignUp?.Invoke());
            ExitCommand = new RelayCommand(() => ExitRequested?.Invoke());
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ValidationFailed?.Invoke("Enter Username");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ValidationFailed?.Invoke("Enter Password");
                return;
            }

            bool success = userDal.LoginUser(Username, Password);

            if (success)
                LoginSucceeded?.Invoke();
            else
                LoginFailed?.Invoke();
        }
    }
}

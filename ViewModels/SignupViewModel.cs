using System;
using SetupReportGenerator.Commands;
using SetupReportGenerator.DAL;

namespace SetupReportGenerator.ViewModels
{
    public class SignupViewModel : ViewModelBase
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

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        public RelayCommand RegisterCommand { get; }
        public RelayCommand NavigateToLoginCommand { get; }

        public event Action<string> ValidationFailed;
        public event Action RegistrationSucceeded;
        public event Action RegistrationFailed;
        public event Action NavigateToLogin;

        public SignupViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
            NavigateToLoginCommand = new RelayCommand(() => NavigateToLogin?.Invoke());
        }

        /// <summary>
        /// Same check the old txtConfirmPassword_Leave handler did, exposed
        /// so the View can call it on the Leave event without owning the logic.
        /// </summary>
        public bool PasswordsMatch() =>
            string.IsNullOrEmpty(Password) || Password == ConfirmPassword;

        private void Register()
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

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ValidationFailed?.Invoke("Confirm Password");
                return;
            }

            if (Password != ConfirmPassword)
            {
                ValidationFailed?.Invoke("Passwords do not match");
                return;
            }

            bool success = userDal.RegisterUser(Username, Password);

            if (success)
                RegistrationSucceeded?.Invoke();
            else
                RegistrationFailed?.Invoke();
        }
    }
}

using SetupReportGenerator.ViewModels;
using System;
using System.Windows.Forms;

namespace SetupReportGenerator
{
    public partial class SignupForm : Form
    {
        private readonly SignupViewModel viewModel;

        public SignupForm()
        {
            InitializeComponent();

            viewModel = new SignupViewModel();
            BindControls();
            WireViewModelEvents();
        }

        private void BindControls()
        {
            txtUsername.DataBindings.Add("Text", viewModel, nameof(viewModel.Username),
                true, DataSourceUpdateMode.OnPropertyChanged);

            txtPassword.DataBindings.Add("Text", viewModel, nameof(viewModel.Password),
                true, DataSourceUpdateMode.OnPropertyChanged);

            txtConfirmPassword.DataBindings.Add("Text", viewModel, nameof(viewModel.ConfirmPassword),
                true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void WireViewModelEvents()
        {
            viewModel.ValidationFailed += message => MessageBox.Show(message);

            viewModel.RegistrationSucceeded += () =>
            {
                MessageBox.Show("Registration Successful");

                LoginForm login = new LoginForm();
                login.Show();
                this.Close();
            };

            viewModel.RegistrationFailed += () => MessageBox.Show("Registration Failed");

            viewModel.NavigateToLogin += () =>
            {
                LoginForm login = new LoginForm();
                login.Show();
                this.Close();
            };
        }

        // ---- Designer-wired event handlers: one-line delegations to the ViewModel ----

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            => viewModel.NavigateToLoginCommand.Execute();

        private void btnRegister_Click(object sender, EventArgs e)
            => viewModel.RegisterCommand.Execute();

        private void txtConfirmPassword_Leave(object sender, EventArgs e)
        {
            if (!viewModel.PasswordsMatch())
            {
                MessageBox.Show("Password and Confirm Password do not match.");
                txtConfirmPassword.Clear();
                txtConfirmPassword.Focus();
            }
        }

        private void SignupForm_Load(object sender, EventArgs e)
        {
            groupBox1.Left = (this.ClientSize.Width - groupBox1.Width) / 2;
            groupBox1.Top = (this.ClientSize.Height - groupBox1.Height) / 2;
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }
    }
}

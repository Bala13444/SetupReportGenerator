using SetupReportGenerator.ViewModels;
using System;
using System.Windows.Forms;

namespace SetupReportGenerator
{
    public partial class LoginForm : Form
    {
        private readonly LoginViewModel viewModel;

        public LoginForm()
        {
            InitializeComponent();

            viewModel = new LoginViewModel();
            BindControls();
            WireViewModelEvents();
        }

        private void BindControls()
        {
            txtUsername.DataBindings.Add("Text", viewModel, nameof(viewModel.Username),
                true, DataSourceUpdateMode.OnPropertyChanged);

            txtPassword.DataBindings.Add("Text", viewModel, nameof(viewModel.Password),
                true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void WireViewModelEvents()
        {
            viewModel.ValidationFailed += message =>
            {
                MessageBox.Show(message);
                (message == "Enter Username" ? (Control)txtUsername : txtPassword).Focus();
            };

            viewModel.LoginSucceeded += () =>
            {
                Form1 frm = new Form1();
                frm.Show();
                this.Hide();
            };

            viewModel.LoginFailed += () =>
            {
                MessageBox.Show("Invalid Username or Password");
                txtPassword.Clear();
                txtPassword.Focus();
            };

            viewModel.NavigateToSignUp += () =>
            {
                SignupForm signup = new SignupForm();
                signup.Show();
                this.Hide();
            };

            viewModel.ExitRequested += () => this.Close();
        }

        // ---- Designer-wired event handlers: one-line delegations to the ViewModel ----

        private void lnkSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            => viewModel.SignUpCommand.Execute();

        private void btnLogin_Click(object sender, EventArgs e)
            => viewModel.LoginCommand.Execute();

        private void btnExit_Click(object sender, EventArgs e)
            => viewModel.ExitCommand.Execute();

        private void LoginForm_Load(object sender, EventArgs e)
        {
            groupBox1.Left = (this.ClientSize.Width - groupBox1.Width) / 2;
            groupBox1.Top = (this.ClientSize.Height - groupBox1.Height) / 2;
        }

        private void lblUsername_Click(object sender, EventArgs e)
        {
        }
    }
}

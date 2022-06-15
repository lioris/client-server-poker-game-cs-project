using Contracts;
using LinqToSql;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;


namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker logInWorker = new BackgroundWorker();
        private readonly BackgroundWorker registerWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            //register to callback singeltone
            ClientCallBack.Instance.addWindow(Constants.LOGIN_WINDOW, this);

            string pathcd = Directory.GetCurrentDirectory();

            // start the game server and the data base server
            if ((Process.GetProcessesByName("DBhost")).Length == 0)
                Process.Start(fileName: "..\\..\\..\\DBhost\\bin\\Debug\\DBhost.exe");

            if ((Process.GetProcessesByName("GameHost")).Length == 0)
                Process.Start(fileName: "..\\..\\..\\GameHost\\bin\\Debug\\GameHost.exe");

            // login methoed 
            logInWorker.DoWork += login_worker_DoWork;
            logInWorker.RunWorkerCompleted += login_worker_RunWorkerCompleted;

            //register methoed
            registerWorker.DoWork += register_worker_DoWork;
            registerWorker.RunWorkerCompleted += register_worker_RunWorkerCompleted;

        }

        private void setDisable()
        {
            txtPassWord.IsReadOnly = true;
            txtUserName.IsReadOnly = true;

            btnLogIn.IsEnabled = false;
            btnRegister.IsEnabled = false;
        }


        private void setEnable()
        {
            txtPassWord.IsReadOnly = false;
            txtUserName.IsReadOnly = false;

            btnLogIn.IsEnabled = true;
            btnRegister.IsEnabled = true;
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            this.setDisable();

            if (!logInWorker.IsBusy)
            {
                logInWorker.RunWorkerAsync();
            }
        }

        private void login_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            IGameService proxy = Proxy.Instance.GetProxy();
            string usernameInvoked = string.Empty;
            string passwordInvoked = string.Empty;

            Dispatcher.BeginInvoke(new Action(delegate
            {
                usernameInvoked = txtUserName.Text;
                passwordInvoked = txtPassWord.Text;
            }));


            Thread.Sleep(100);
            User user = proxy.LogInApp(usernameInvoked, passwordInvoked);

            if (user == null)
                Application.Current.Resources[Constants.CURRENT_USER] = null;
            else
                Application.Current.Resources[Constants.CURRENT_USER] = user;
        }

        private void login_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            User user = (User)Application.Current.Resources[Constants.CURRENT_USER];

            if (user != null)
            {
                MessageBox.Show("conected with user " + user.Name + "and your id is" + user.ID);
                ClientCallBack.Instance.addWindow(Constants.LOBBY_WINDOW, new loby()).Show();
                ClientCallBack.Instance.CloseWindow(Constants.LOGIN_WINDOW);
            }
            else
            {
                MessageBox.Show("Wrong user name = " + txtUserName.Text + " or password = " + txtPassWord.Text + " , try again!");
                this.setEnable();
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            this.setDisable();

            if (!registerWorker.IsBusy)
            {
                registerWorker.RunWorkerAsync();
            }
        }


        private void register_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            IGameService proxy = Proxy.Instance.GetProxy();
            string usernameInvoked = string.Empty;
            string passwordInvoked = string.Empty;

            Dispatcher.BeginInvoke(new Action(delegate
            {
                usernameInvoked = txtUserName.Text;
                passwordInvoked = txtPassWord.Text;
            }));


            Thread.Sleep(100);
            User user = proxy.RegisterApp(usernameInvoked, passwordInvoked);

            if (user == null)
                Application.Current.Resources[Constants.CURRENT_USER] = null;
            else
                Application.Current.Resources[Constants.CURRENT_USER] = user;
        }

        private void register_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            User user = (User)Application.Current.Resources[Constants.CURRENT_USER];

            if (user != null)
            {
                MessageBox.Show("Registerd with user " + user.Name + "and your id is" + user.ID);
                if (!logInWorker.IsBusy)
                    logInWorker.RunWorkerAsync();
            }

            else
            {
                MessageBox.Show("somthing went wrong, try Agian!");
                this.setEnable();
            }
        }


    }
}

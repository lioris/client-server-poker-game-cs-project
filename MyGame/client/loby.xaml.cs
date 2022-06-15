using Contracts;
using LinqToSql;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace client
{
    /// <summary>
    /// Interaction logic for loby.xaml
    /// </summary>
    public partial class loby : Window
    {
        public loby()
        {
            InitializeComponent();

            ShowuserDetails();
            updataTables();

            ClientCallBack.Instance.enterdtoroom += (sender, tData) =>
            {
                Dispatcher.Invoke(() => EnteredToTable(tData));
            };

        }

        public void updataTables()
        {
            tablesData.ItemsSource = Proxy.Instance.GetProxy().RetriveTables();
        }

        internal void updataTables(IEnumerable<LinqToSql.Table> t)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                tablesData.ItemsSource = t;
            }));

        }

        private void ShowuserDetails()
        {
            User user = (User)Application.Current.Resources[Constants.CURRENT_USER];
            lblName.Content = "Name: " + user.Name;
            lblMoney.Content = "Money: " + user.Money;
        }

        private void btnNewTable_Click(object sender, RoutedEventArgs e)
        {
            ClientCallBack.Instance.addWindow(Constants.NEW_TABLE_WINDOW, new NewTable()).Show();
            ClientCallBack.Instance.CloseWindow(Constants.LOBBY_WINDOW);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)Application.Current.Resources[Constants.CURRENT_USER];
            Proxy.Instance.GetProxy().LogOutApp(user.Name);
            Application.Current.Resources[Constants.CURRENT_USER] = null;

            (new MainWindow()).Show();
            ClientCallBack.Instance.CloseWindow(Constants.LOBBY_WINDOW);
        }

        private void tablesData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // iteratively traverse the visual tree
            while ((dep != null) && !(dep is DataGridCell))
                dep = VisualTreeHelper.GetParent(dep);

            if (dep == null)
                return;

            if (dep is DataGridCell)
            {
                DataGridCell cell = dep as DataGridCell;

                // navigate further up the tree
                while ((dep != null) && !(dep is DataGridRow))
                    dep = VisualTreeHelper.GetParent(dep);

                DataGridRow row = dep as DataGridRow;

                int x = row.GetIndex();

            }

            LinqToSql.Table currentTable = (LinqToSql.Table)tablesData.CurrentCell.Item;

            string userName = ((User)Application.Current.Resources[Constants.CURRENT_USER]).Name;

            Proxy.Instance.GetProxy().EnterToTable(currentTable.Id, userName);

        }

        private void EnteredToTable(TableData tableData)
        {
            int tableId = int.Parse(tableData.TableId);
            ClientCallBack.Instance.addWindow(Constants.TABLE_WINDOW, new table(tableId, tableData)).Show();
            ClientCallBack.Instance.CloseWindow(Constants.LOBBY_WINDOW);
        }

    }
}

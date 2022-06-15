using Contracts;
using LinqToSql;
using System.Windows;

namespace client
{
    /// <summary>
    /// Interaction logic for NewTable.xaml
    /// </summary>
    public partial class NewTable : Window
    {
        public NewTable()
        {
            InitializeComponent();

            ClientCallBack.Instance.openNewGameTable += (sender, tData) =>
            {
                Dispatcher.Invoke(() => openNewTable(tData));
            };
        }

        public void openNewTable(TableData tableData)
        {
            int tableId = ClientCallBack.Instance.tableid;
            ClientCallBack.Instance.addWindow(Constants.TABLE_WINDOW, new table(tableId, tableData)).Show();
            ClientCallBack.Instance.CloseWindow(Constants.NEW_TABLE_WINDOW);
        }

        private void setDisable()
        {
            txtTableName.IsReadOnly = true;

            comboBoxNumPlayers.IsReadOnly = true;
            ComboBoxBigBliend.IsReadOnly = true;

            btnCancel.IsEnabled = false;
            btnOpenTable.IsEnabled = false;
        }

        private void setEnable()
        {
            txtTableName.IsReadOnly = false;

            comboBoxNumPlayers.IsReadOnly = false;
            ComboBoxBigBliend.IsReadOnly = false;

            btnCancel.IsEnabled = true;
            btnOpenTable.IsEnabled = true;
        }

        private void btnOpenTable_Click(object sender, RoutedEventArgs e)
        {
            setDisable();

            //open table async
            int numPlayers = int.Parse(comboBoxNumPlayers.SelectionBoxItem.ToString());
            int bigBliend = int.Parse(ComboBoxBigBliend.SelectionBoxItem.ToString());

            string name = ((User)Application.Current.Resources[Constants.CURRENT_USER]).Name;

            Proxy.Instance.GetProxy().OpenNewTable(name, txtTableName.Text, numPlayers, bigBliend);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClientCallBack.Instance.addWindow(Constants.LOBBY_WINDOW, new loby()).Show();
            ClientCallBack.Instance.CloseWindow(Constants.NEW_TABLE_WINDOW);

        }
    }
}

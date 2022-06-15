using Contracts;
using System;
using System.Collections.Generic;
using System.Windows;

namespace client
{
    class ClientCallBack : IClientCallBack
    {
        public event EventHandler<TableData> updateui;
        public event EventHandler<TableData> openNewGameTable;
        public event EventHandler<TableData> enterdtoroom;
        public event EventHandler<string> chat_massage;

        Dictionary<string, Window> list = new Dictionary<string, Window>();
        public int tableid { get; set; }
        public TableData tData { get; set; }

        #region Singelton
        private static ClientCallBack _instance;

        private ClientCallBack()
        {

        }

        public static ClientCallBack Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClientCallBack();

                return _instance;
            }
        }
        #endregion

        // mange windows

        public Window addWindow(string name, Window w)
        {
            if (list.ContainsKey(name))
                list[name] = w;
            else
                list.Add(name, w);

            return w;
        }

        public void CloseWindow(string name)
        {
            if (list.ContainsKey(name))
            {
                if (list[name] != null)
                {
                    list[name].Close();
                    list[name] = null;
                }
            }
        }

        internal Window GetWindow(string p)
        {
            return list[p];
        }

        public void updateChat(string massage)
        {
            var evt = chat_massage;
            if (evt != null)
                evt(this, massage);
        }

        public void newTableOpened(int tableId, TableData tableData)
        {
            this.tableid = tableId;

            var evt = openNewGameTable;
            if (evt != null)
                evt(this, tableData);
        }

        public void EnteredToTheRoom(int tableId, TableData tableData)
        {
            this.tableid = tableId;

            var evt = enterdtoroom;
            if (evt != null)
                evt(this, tableData);

        }

        public void updataLoby1(IEnumerable<LinqToSql.Table> t)
        {
            MessageBox.Show("loby updated");
            if (list.ContainsKey(Constants.LOBBY_WINDOW))
            {
                if (list[Constants.LOBBY_WINDOW] != null)
                    ((loby)list[Constants.LOBBY_WINDOW]).updataTables(t);

            }
        }


        public void updataTable(TableData tableData)
        {
            var evt = updateui;
            if (evt != null)
                evt(this, tableData);
        }

    }
}




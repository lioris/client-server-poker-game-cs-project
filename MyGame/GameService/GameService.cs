using Contracts;
using LinqToSql;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace GameService
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class GameService : IGameService
    {
        ChannelFactory<IDbService> DBchannel;
        public static IDbService DBproxy;
        public static Dictionary<string, IClientCallBack> user_list = new Dictionary<string, IClientCallBack>();
        public static Dictionary<int, TableGame> tables_lists = new Dictionary<int, TableGame>();

        GameService()
        {
            DBchannel = new ChannelFactory<IDbService>("DBServiceEndpoint");
            DBproxy = DBchannel.CreateChannel();
        }


        public User LogInApp(string userName, string Password)
        {
            if (user_list.ContainsKey(userName))
            {
                Console.WriteLine("you already logged");
                return null;
            }

            Console.WriteLine("LOGIN: NAME {0} PASSWORD {1}", userName, Password);
            IDbService DBproxy = DBchannel.CreateChannel();
            User user = DBproxy.login(userName, Password);

            if (user != null)
            {
                Console.WriteLine("ADD TO USER LIST");
                user_list.Add(userName, OperationContext.Current.GetCallbackChannel<IClientCallBack>());
                return user;
            }
            else
                return null;


        }

        public void LogOutApp(string name)
        {
            if (user_list.ContainsKey(name))
                user_list.Remove(name);
        }

        public User RegisterApp(string userName, string Password)
        {
            Console.WriteLine("REGISTER: NAME {0} PASSWORD {1}", userName, Password);
            IDbService DBproxy = DBchannel.CreateChannel();
            return DBproxy.Register(userName, Password);
        }

        public IEnumerable<Table> RetriveTables()
        {
            return DBproxy.RetriveTables();
        }

        public void OpenNewTable(string userName, string tableName, int numPlayers, int bigBliend)
        {
            //creat new table, save the ditalis to db
            // enter the player to the table an put him to a sit
            //update the client about it.

            TableGame newTable = new TableGame(tableName, numPlayers, bigBliend);

            int tableId = DBproxy.insertNewTable(userName, tableName, numPlayers, bigBliend);
            newTable.table_data.TableId = tableId.ToString();

            Console.WriteLine("retrived table and its id is {0}", tableId);

            tables_lists.Add(tableId, newTable);

            newTable.addPlayer(userName, user_list[userName], 0, true);
            DBproxy.updateTable(tableId, newTable.currentPlaying);

            user_list[userName].newTableOpened(tableId, newTable.table_data);

            IEnumerable<Table> t = DBproxy.RetriveTables();
            foreach (KeyValuePair<string, IClientCallBack> client in user_list)
            {
                Console.WriteLine("updated " + client.Key);
                client.Value.updataLoby1(t);
            }

            newTable.Run();
        }


        public void EnterToTable(int tableID, string userName)
        {
            //addToWatchingList, //init table data ,//send to user
            if (tables_lists.ContainsKey(tableID))
            {
                tables_lists[tableID].addPlayer(userName, user_list[userName], -1, false);
                user_list[userName].EnteredToTheRoom(tableID, tables_lists[tableID].table_data);
            }

        }


        public void RetriveTables1()
        {
            foreach (KeyValuePair<string, IClientCallBack> client in user_list)
            {
                IEnumerable<Table> t = DBproxy.RetriveTables();

                Console.WriteLine("updated " + client.Key);
                client.Value.updataLoby1(t);
            }
        }


        public void Sit(int table, int seat, string userName)
        {
            // retrive table - change player status from watching to playing - update table - notify all;
            tables_lists[table].sit(seat, userName, user_list[userName]);
            DBproxy.updateTable(table, tables_lists[table].currentPlaying);

            updateTable(table, tables_lists[table].table_data);

        }


        public void sendMassage(int tableid, string massage)
        {

            foreach (KeyValuePair<int, IClientCallBack> client in tables_lists[tableid].playing_list)
            {
                client.Value.updateChat(massage);
            }

            foreach (var watching in tables_lists[tableid].watching_list)
            {
                watching.updateChat(massage);
            }

        }


        private void updateTable(int tableid, TableData t)
        {
            foreach (KeyValuePair<int, IClientCallBack> client in tables_lists[tableid].playing_list)
                client.Value.updataTable(t);

            foreach (var watching in tables_lists[tableid].watching_list)
                watching.updataTable(t);
        }


        public void Call(int tableid, int seatid)
        {
            tables_lists[tableid].Call(seatid);
            updateTable(tableid, tables_lists[tableid].table_data);
        }

        public void Raise(int tableid, int seatid, int raise)
        {
            tables_lists[tableid].Raise(seatid, raise);
            updateTable(tableid, tables_lists[tableid].table_data);
        }

        public void Fold(int tableid, int seatid)
        {
            tables_lists[tableid].Fold(seatid);

            updateTable(tableid, tables_lists[tableid].table_data);
        }
    }
}

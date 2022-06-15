using LinqToSql;
using System.Collections.Generic;
using System.ServiceModel;



namespace Contracts
{


    [ServiceContract]
    public interface IDbService
    {

        [OperationContract]
        bool isUserExsist(string Name);

        [OperationContract]
        User login(string Name, string password);

        [OperationContract]
        User Register(string Name, string Password);

        [OperationContract(IsOneWay = true)]
        void updataUserMoney(string Name, int money);

        [OperationContract]
        User getUserById(int id);

        [OperationContract]
        User getUserByName(string name);

        [OperationContract]
        IEnumerable<Table> RetriveTables();

        [OperationContract]
        int insertNewTable(string userName, string tableName, int numPlayers, int bigBliend);

        [OperationContract(IsOneWay = true)]
        void updateTable(int tableId, int currentPlaying);

    }
}


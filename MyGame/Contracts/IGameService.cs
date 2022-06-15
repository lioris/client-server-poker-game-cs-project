using LinqToSql;
using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IClientCallBack))]
    public interface IGameService
    {
        [OperationContract]
        User RegisterApp(string userName, string Password);

        [OperationContract]
        User LogInApp(string userName, string Password);

        [OperationContract(IsOneWay = true)]
        void LogOutApp(string name);

        [OperationContract]
        IEnumerable<Table> RetriveTables();

        [OperationContract(IsOneWay = true)]
        void RetriveTables1();

        [OperationContract(IsOneWay = true)]
        void OpenNewTable(string userName, string tableName, int numPlayers, int bigBliend);

        [OperationContract(IsOneWay = true)]
        void EnterToTable(int tableID, string userName);

        [OperationContract(IsOneWay = true)]
        void Sit(int table, int seat, string userName);

        [OperationContract(IsOneWay = true)]
        void sendMassage(int tableid, string massage);


        [OperationContract(IsOneWay = true)]
        void Call(int tableid, int seatid);

        [OperationContract(IsOneWay = true)]
        void Raise(int tableid, int seatid, int raise);

        [OperationContract(IsOneWay = true)]
        void Fold(int tableid, int seatid);
    }
}

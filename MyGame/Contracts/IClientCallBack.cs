using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IClientCallBack
    {
        [OperationContract(IsOneWay = true)]
        void newTableOpened(int tableId, TableData tableData);

        [OperationContract(IsOneWay = true)]
        void EnteredToTheRoom(int tableId, TableData tableData);

        [OperationContract(IsOneWay = true)]
        void updataLoby1(IEnumerable<LinqToSql.Table> t);

        [OperationContract(IsOneWay = true)]
        void updataTable(TableData tableData);

        [OperationContract(IsOneWay = true)]
        void updateChat(string massage);

    }
}

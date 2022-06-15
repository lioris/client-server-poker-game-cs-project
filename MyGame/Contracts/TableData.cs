using System.Runtime.Serialization;

namespace Contracts
{

    [DataContract]
    public class TableData
    {
        [DataMember]
        public string TableName { get; set; }
        [DataMember]
        public string TableId { get; set; }
        [DataMember]
        public int Bank { get; set; }
        [DataMember]
        public int CurrentBet { get; set; }
        [DataMember]
        public Card[] CommunityCards { get; set; }
        [DataMember]
        public int seatTurn { get; set; }
        [DataMember]
        public Seat[] Seats { get; set; }

        public TableData(string tableName, int dealer)
        {
            TableName = tableName;
            Bank = 0;
            CurrentBet = 0;
            CommunityCards = new Card[5];
            for (int i = 0; i < CommunityCards.Length; i++)
                CommunityCards[i] = new Card();

            seatTurn = dealer;
            Seats = new Seat[Constants.MAX_TABLE_SEATS];
        }



    }
}

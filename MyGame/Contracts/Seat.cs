using System.Runtime.Serialization;

namespace Contracts
{
    [DataContract]
    public class Seat
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public Card[] cards;
        [DataMember]
        public bool IsSeatTaken { get; set; }
        [DataMember]
        public bool isFolded;
        [DataMember]
        public int myBet;

        public Seat(int id)
        {
            ID = id;
            IsSeatTaken = true;
            isFolded = false;
            cards = new Card[2];
            cards[0] = new Card();
            cards[1] = new Card();
            myBet = 0;
        }

    }
}

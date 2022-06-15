using System.Runtime.Serialization;

namespace Contracts
{
    [DataContract]
    public static class Shape
    {
        [DataMember]
        public const string Diamonds = "di";
        [DataMember]
        public const string Spades = "sp";
        [DataMember]
        public const string Clubs = "cl";
        [DataMember]
        public const string Hearts = "he";
    }

    public enum FaceValue
    {

        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }

    [DataContract]
    public class Card
    {
        [DataMember]
        private string path;
        [DataMember]
        string CardShape;
        [EnumMember]
        FaceValue Val;
        [DataMember]
        public bool isCardUp { get; set; }

        public Card()
        {
            isCardUp = false;
            path = Constants.DEFULT_PIC_CARD_PATH + Constants.DEFULT_PIC_CARD;
        }

        public void setPath(string shape, FaceValue val)
        {
            this.path = Constants.DEFULT_PIC_CARD_PATH + shape + (int)val + ".gif";
        }

        public string getPath()
        {
            return path;
        }

        public Card(string shape, FaceValue value, bool iscardup)
        {
            this.CardShape = shape;
            this.Val = value;
            this.isCardUp = iscardup;

            setPath(this.CardShape, this.Val);
        }



    }
}

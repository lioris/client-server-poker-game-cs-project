using Contracts;
using System;
using System.Collections.Generic;

namespace GameService
{
    class Deck
    {
        public List<Card> cards = new List<Card>();

        // Returns the card at the given position
        public Card this[int position] { get { return (Card)cards[position]; } }

        public Deck()
        {
            foreach (FaceValue faceVal in Enum.GetValues(typeof(FaceValue)))
            {
                cards.Add(new Card(Shape.Clubs, faceVal, true));
                cards.Add(new Card(Shape.Diamonds, faceVal, true));
                cards.Add(new Card(Shape.Hearts, faceVal, true));
                cards.Add(new Card(Shape.Spades, faceVal, true));
            }

            Shuffle();

        }

        public Card getCard()
        {
            Card c = cards[0];
            cards.RemoveAt(0);
            return c;
        }

        private void Shuffle()
        {
            Random random = new Random();
            for (int i = 0; i < cards.Count; i++)
            {
                int index1 = i;
                int index2 = random.Next(cards.Count);
                SwapCard(index1, index2);
            }
        }

        private void SwapCard(int index1, int index2)
        {
            Card card = cards[index1];
            cards[index1] = cards[index2];
            cards[index2] = card;
        }
    }

}

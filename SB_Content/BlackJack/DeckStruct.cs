using System.Collections.Generic;
using System;

namespace SB_Content.BlackJack
{
    class DeckStruct
    {
        private readonly List<Card> Deck = new ();
        public DeckStruct()
        {
            for (int i = 0; i < 13; i++)
            {
                Card hearts = new(i, Card.CardSuit.Hearts);
                Card spades = new(i, Card.CardSuit.Spades);
                Card clubs = new(i, Card.CardSuit.Clubs);
                Card diamonds = new(i, Card.CardSuit.Diamonds);

                Deck.Add(hearts);
                Deck.Add(spades);
                Deck.Add(clubs);
                Deck.Add(diamonds);
            }
            Shuffle();
        }

        //Deck Shuffler
        private void Shuffle()
        {
            Random rand = new();
            int num1, num2;
            for (int i = 0; i < 10000; i++)
            {
                num1 = rand.Next(0, 52);
                num2 = rand.Next(0, 52);
                (Deck[num2], Deck[num1]) = (Deck[num1], Deck[num2]);
            }
        }

        //Takes the first card from the top of the deck
        public Card TopDeck()
        {
            Card outp = Deck[0];
            Deck.Remove(Deck[0]);
            return outp;
        }

    }
}

using System.Collections.Generic;
using System;

namespace SB_Content.Card_Games
{

    readonly struct DeckStruct
    {
        private readonly List<Card> Deck = [];
        public DeckStruct(bool Joker = false)
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
            if (Joker)
            {
                Deck.Add(new Card(0, Card.CardSuit.Joker));
                Deck.Add(new Card(0, Card.CardSuit.Joker));
            }
            Deck = Shuffle(Deck);
        }
        private static List<Card> Shuffle(List<Card> Deck)
        {
            Random rand = new();
            int num1, num2;
            for (int i = 0; i < 10000; i++)
            {
                num1 = rand.Next(0, 52);
                num2 = rand.Next(0, 52);
                (Deck[num2], Deck[num1]) = (Deck[num1], Deck[num2]);
            }
            return Deck;
        }
        public readonly Card TopDeck()
        {
            Card outp = Deck[0];
            Deck.Remove(Deck[0]);
            return outp;
        }
    }
    public readonly struct Card(int arg, Card.CardSuit suit)
    {
        public enum CardSuit
        {
            Hearts,
            Spades,
            Clubs,
            Diamonds,
            Joker
        }
        public readonly int Value = arg;
        public readonly CardSuit Suit = suit;
    }
}

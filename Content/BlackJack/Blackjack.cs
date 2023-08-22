﻿using Discord.Commands;

namespace SteveBot_Rebuild.Content.BlackJack
{
    public class Card
    {
        public enum CardSuit
        {
            Hearts,
            Spades,
            Clubs,
            Diamonds
        }
        public int Value;
        public CardSuit Suit;

        public Card(int arg, CardSuit suit)
        {
            Value = arg;
            Suit = suit;
        }
    }
    public class Blackjack : ModuleBase<SocketCommandContext>
    {
        readonly Player dealer = new(true);
        readonly Player player = new();
        private string msg = "PLACEHOLDER";
        public Blackjack() { }
        //Automatically plays a game
        public Player PlayGame()
        {
            DeckStruct deck = new();
            //First card per player
            player.TakeCard(deck.TopDeck());
            dealer.TakeCard(deck.TopDeck());
            //Second Card per player
            player.TakeCard(deck.TopDeck());
            dealer.TakeCard(deck.TopDeck());

            while (player.TakeTurn(dealer) && player.CardValue <= 21)
            {
                Card tmp = deck.TopDeck();
                player.TakeCard(tmp);
            }
            while (dealer.TakeTurn(player))
            {
                Card tmp = deck.TopDeck();
                dealer.TakeCard(tmp);
            }
            if (dealer.CardValue == player.CardValue)
            {
                msg = $"Dealer wins!\nDealer: {dealer.CardValue}\nPlayer: {player.CardValue}";
                return dealer;
            }
            if (dealer.CardValue >= player.CardValue && dealer.CardValue < 22)
            {
                msg = $"Dealer Wins!\nDealer: {dealer.CardValue}\nPlayer: {player.CardValue}";
                return dealer;
            }
            else
            {
                if (player.CardValue > 21)
                {
                    msg = $"Dealer Wins!\nDealer: {dealer.CardValue}\nPlayer: {player.CardValue}";
                    return dealer;
                }
                else
                {
                    msg = $"Player Wins!\nPlayer: {player.CardValue}\nDealer: {dealer.CardValue}";
                    return player;
                }
            }
        }
        public string Win() => msg;


    }
}

﻿using System.Collections.Generic;

namespace SB_Content.Card_Games.BlackJack {
    public class Player(bool dealer = false) {
        #region Variables
        private Card? DealHidden = null;
        private readonly bool isdealer = dealer;
        public bool IsDealer { get { return isdealer; } }
        private readonly List<Card> deck = [];
        public int CardValue {
            get {
                int High = 0;
                int low = 0;
                foreach (Card crd in deck) {
                    if (crd.Value == 0) {
                        High += 11;
                        low += 1;
                    } else if (crd.Value >= 9) {
                        High += 10;
                        low += 10;
                    } else {
                        High += crd.Value + 1;
                        low += crd.Value + 1;
                    }
                }
                if (High == low)
                    return High;
                if (High > 21)
                    return low;
                else
                    return High;
            }
        }
        #endregion Variables
        #region Logic
        public void TakeCard(Card card) {
            if (IsDealer && DealHidden == null)
                DealHidden = card;
            else
                deck.Add(card);
        }
        public bool TakeTurn(Player opponent) {
            if (IsDealer)
                return Dturn(opponent);
            else
                return Pturn(opponent);
        }
        private bool Dturn(Player oppo) {
            if (DealHidden is not null) {
                deck.Add(DealHidden.Value);
                DealHidden = null;
            }
            if (CardValue >= oppo.CardValue)
                return false;
            else
                return true;
        }
        private bool Pturn(Player oppo) {
            if (oppo.CardValue <= CardValue)
                return false;
            else
                return true;
        }
        #endregion Logic
    }
}
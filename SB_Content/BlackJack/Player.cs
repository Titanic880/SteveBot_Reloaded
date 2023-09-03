using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB_Content.BlackJack
{
    public class Player
    {
        #region Variables
        private Card? DealHidden = null;
        private readonly bool isdealer = false;
        public bool IsDealer { get { return isdealer; } }
        private readonly List<Card> deck = new();
        public int CardValue
        {
            get
            {
                int High = 0;
                int low = 0;
                foreach (Card crd in deck)
                {
                    if (crd.Value == 0)
                    {
                        High += 11;
                        low += 1;
                    }
                    else if (crd.Value >= 9)
                    {
                        High += 10;
                        low += 10;
                    }
                    else
                    {
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
        public Player(bool dealer = false)
        {
            isdealer = dealer;
        }
        public void TakeCard(Card card)
        {
            if (IsDealer && DealHidden == null)
                DealHidden = card;
            else
                deck.Add(card);
        }


        public bool TakeTurn(Player opponent)
        {
            if (IsDealer)
                return Dturn(opponent);
            else
                return Pturn(opponent);
        }
        private bool Dturn(Player oppo)
        {
            if (DealHidden != null)
            {
                deck.Add(DealHidden);
                DealHidden = null;
            }
            if (CardValue >= oppo.CardValue)
                return false;
            else
                return true;
        }
        private bool Pturn(Player oppo)
        {
            if (oppo.CardValue <= CardValue)
                return false;
            else
                return true;
        }
        #endregion Logic
    }
}
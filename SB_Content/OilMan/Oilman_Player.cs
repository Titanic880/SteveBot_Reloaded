using System;
using System.Collections.Generic;
using Discord;

namespace SB_Content.OilMan
{
    internal class Oilman_Player
    {
        public readonly IUser User;
        public readonly int GameID;
        public int Money { get; private set; }
        private int OwedMoney = 0;
        public int Wealth { get => Money - OwedMoney; }
        public int Income { get; private set; }
        public readonly List<GameTile> OwnedTiles = new();
        public Tuple<Emoji, Emoji>? Player_Color { get; private set; }

        //public Tuple<Emoji, Emoji> PlayerColors;
        public Oilman_Player(IUser User, int gameID)
        {
            this.User = User;
            Money = 100000;
            Income = 0;
            GameID = gameID;
        }
        public void SetColors(Tuple<Emoji, Emoji> Colors)
        {
            Player_Color = Colors;
        }

        public bool Pay(int amount)
        {
            if (Money < amount) return false;
            Money -= amount;
            return true;
        }
        public void StartTurnUpdate()
        {
            Money += Income;
        }
        public void EndTurnUpdate()
        {
            foreach (GameTile tile in OwnedTiles)
                Income += tile.GetTileIncome();
        }
        public override string ToString()
            => User.Username;
    }
}
